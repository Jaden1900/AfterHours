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
        private const string InteractActionName = "Interact";
        private const string UiMapName = "UI";
        private const string CancelActionName = "Cancel";

        [SerializeField] private InputActionAsset _inputActions;

        private InputActionMap _playerMap;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _interactAction;
        private InputActionMap _uiMap;
        private InputAction _cancelAction;
        private bool _jumpPressed;
        private bool _interactPressed;
        private bool _cancelPressed;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        private void Awake()
        {
            if (_inputActions == null)
            {
                return;
            }

            _playerMap = _inputActions.FindActionMap(PlayerMapName, true);
            _moveAction = _playerMap.FindAction(MoveActionName, true);
            _lookAction = _playerMap.FindAction(LookActionName, true);
            _jumpAction = _playerMap.FindAction(JumpActionName, true);
            _interactAction = _playerMap.FindAction(InteractActionName, true);
            _uiMap = _inputActions.FindActionMap(UiMapName, false);
            _cancelAction = _uiMap?.FindAction(CancelActionName, false);
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
            _interactAction.performed += OnInteract;
            if (_cancelAction != null)
            {
                _cancelAction.performed += OnCancel;
                _uiMap.Enable();
            }

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
            _interactAction.performed -= OnInteract;
            if (_cancelAction != null)
            {
                _cancelAction.performed -= OnCancel;
                _uiMap.Disable();
            }

            _playerMap.Disable();
            MoveInput = Vector2.zero;
            LookInput = Vector2.zero;
            _jumpPressed = false;
            _interactPressed = false;
            _cancelPressed = false;
        }

        public bool ConsumeJumpPressed()
        {
            bool jumpPressed = _jumpPressed;
            _jumpPressed = false;
            return jumpPressed;
        }

        public bool ConsumeInteractPressed()
        {
            bool interactPressed = _interactPressed;
            _interactPressed = false;
            return interactPressed;
        }

        public bool ConsumeCancelPressed()
        {
            bool cancelPressed = _cancelPressed;
            _cancelPressed = false;
            return cancelPressed;
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

        private void OnInteract(InputAction.CallbackContext context)
        {
            _interactPressed = true;
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            _cancelPressed = true;
        }
    }
}
