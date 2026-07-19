using UnityEngine;

namespace AfterHours.Gameplay.Player
{
    public sealed class ThirdPersonCameraController : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private Transform _target;
        [SerializeField] private float _distance = 5f;
        [SerializeField] private float _height = 1.5f;
        [SerializeField] private float _lookSensitivity = 0.15f;
        [SerializeField] private float _minimumPitch = -30f;
        [SerializeField] private float _maximumPitch = 60f;
        [SerializeField] private float _followSmoothing;

        private float _yaw;
        private float _pitch;

        private void Awake()
        {
            Vector3 angles = transform.eulerAngles;
            _yaw = angles.y;
            _pitch = NormalizePitch(angles.x);
        }

        private void LateUpdate()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Vector2 lookInput = _inputReader.LookInput;
                _yaw += lookInput.x * _lookSensitivity;
                _pitch = ClampPitch(_pitch - lookInput.y * _lookSensitivity, _minimumPitch, _maximumPitch);
            }

            Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            Vector3 desiredPosition = _target.position + Vector3.up * _height - rotation * Vector3.forward * _distance;
            transform.position = _followSmoothing > 0f
                ? Vector3.Lerp(transform.position, desiredPosition, _followSmoothing * Time.deltaTime)
                : desiredPosition;
            transform.rotation = rotation;
        }

        public static float ClampPitch(float pitch, float minimumPitch, float maximumPitch)
        {
            return Mathf.Clamp(pitch, minimumPitch, maximumPitch);
        }

        private static float NormalizePitch(float pitch)
        {
            return pitch > 180f ? pitch - 360f : pitch;
        }

        private void OnValidate()
        {
            _distance = Mathf.Max(0f, _distance);
            _height = Mathf.Max(0f, _height);
            _lookSensitivity = Mathf.Max(0f, _lookSensitivity);
            _maximumPitch = Mathf.Max(_minimumPitch, _maximumPitch);
            _followSmoothing = Mathf.Max(0f, _followSmoothing);
        }
    }
}
