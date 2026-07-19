using UnityEngine;

namespace AfterHours.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        private const float GroundedVerticalVelocity = -2f;

        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _rotationSmoothing = 12f;
        [SerializeField] private float _gravity = -20f;
        [SerializeField] private float _jumpHeight = 1.5f;

        private CharacterController _characterController;
        private float _verticalVelocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector2 moveInput = PlayerMovementMath.ClampMoveInput(_inputReader.MoveInput);
            Vector3 movement = GetCameraRelativeMovement(moveInput);

            if (movement.sqrMagnitude > 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSmoothing * Time.deltaTime);
            }

            if (_characterController.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = GroundedVerticalVelocity;
            }

            if (_characterController.isGrounded && _inputReader.ConsumeJumpPressed())
            {
                _verticalVelocity = PlayerMovementMath.CalculateJumpVelocity(_gravity, _jumpHeight);
            }

            _verticalVelocity += _gravity * Time.deltaTime;
            Vector3 velocity = movement * _movementSpeed;
            velocity.y = _verticalVelocity;
            _characterController.Move(velocity * Time.deltaTime);
        }

        private Vector3 GetCameraRelativeMovement(Vector2 input)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.ProjectOnPlane(_cameraTransform.right, Vector3.up).normalized;
            return cameraForward * input.y + cameraRight * input.x;
        }

        private void OnValidate()
        {
            _movementSpeed = Mathf.Max(0f, _movementSpeed);
            _rotationSmoothing = Mathf.Max(0f, _rotationSmoothing);
            _gravity = Mathf.Min(_gravity, -0.01f);
            _jumpHeight = Mathf.Max(0f, _jumpHeight);
        }
    }
}
