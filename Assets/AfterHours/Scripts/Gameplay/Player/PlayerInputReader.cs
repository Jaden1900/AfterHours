using UnityEngine;
using UnityEngine.InputSystem;

namespace AfterHours.Gameplay.Player
{
    public sealed class PlayerInputReader : MonoBehaviour
    {
        private const string PlayerMapName = "Player";
        private const string MoveActionName = "Move";
        private const string LookActionName = "Look";
        private const string JumpActionName = "Jump";

        [SerializeField] private InputActionAsset _inputActions;

        private InputActionMap _playerMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private bool _jumpPressed;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        private void Awake()
        {
            _playerMap = _inputActions.FindActionMap(PlayerMapName, true);
            _moveAction = _playerMap.FindAction(MoveActionName, true);
            _lookAction = _playerMap.FindAction(LookActionName, true);
            _jumpAction = _playerMap.FindAction(JumpActionName, true);
        }

        private void OnEnable()
        {
            if (_playerMap == null)
            {
                return;
            }

            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;
            _lookAction.performed += OnLook;
            _lookAction.canceled += OnLook;
            _jumpAction.performed += OnJump;
            _playerMap.Enable();
        }

        private void OnDisable()
        {
            if (_playerMap == null)
            {
                return;
            }

            _moveAction.performed -= OnMove;
            _moveAction.canceled -= OnMove;
            _lookAction.performed -= OnLook;
            _lookAction.canceled -= OnLook;
            _jumpAction.performed -= OnJump;
            _playerMap.Disable();
            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;
            _jumpPressed = false;
        }

        public bool ConsumeJumpPressed()
        {
            bool jumpPressed = _jumpPressed;
            _jumpPressed = false;
            return jumpPressed;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            _jumpPressed = true;
        }
    }
}
