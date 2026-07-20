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
        private InputDevice _lastUsedDevice;

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public string InteractionBindingDisplayName => GetBindingDisplayName(_interactAction, _lastUsedDevice);

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
            RememberInputDevice(context);
            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            RememberInputDevice(context);
            LookInput = context.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            RememberInputDevice(context);
            _jumpPressed = true;
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            RememberInputDevice(context);
            _interactPressed = true;
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            RememberInputDevice(context);
            _cancelPressed = true;
        }

        private static string GetBindingDisplayName(InputAction action, InputDevice lastUsedDevice)
        {
            if (action == null)
            {
                return string.Empty;
            }

            bool preferKeyboardAndMouse = lastUsedDevice == null || IsKeyboardOrMouse(lastUsedDevice);

            string preferredDisplayName = GetBindingDisplayName(action, preferKeyboardAndMouse
                ? IsKeyboardOrMouseBinding
                : binding => IsBindingForDevice(binding, lastUsedDevice));

            return string.IsNullOrEmpty(preferredDisplayName)
                ? GetBindingDisplayName(action, _ => true)
                : preferredDisplayName;
        }

        private static string GetBindingDisplayName(InputAction action, System.Func<InputBinding, bool> isPreferredBinding)
        {
            for (int bindingIndex = 0; bindingIndex < action.bindings.Count; bindingIndex++)
            {
                InputBinding binding = action.bindings[bindingIndex];
                if (binding.isComposite || binding.isPartOfComposite || string.IsNullOrEmpty(binding.effectivePath) ||
                    !isPreferredBinding(binding))
                {
                    continue;
                }

                string displayName = action.GetBindingDisplayString(bindingIndex);
                if (!string.IsNullOrEmpty(displayName))
                {
                    return displayName;
                }
            }

            return string.Empty;
        }

        private static bool IsKeyboardOrMouseBinding(InputBinding binding)
        {
            return IsBindingInGroup(binding, "Keyboard&Mouse") ||
                binding.effectivePath.IndexOf("<Keyboard>", System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                binding.effectivePath.IndexOf("<Mouse>", System.StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsBindingForDevice(InputBinding binding, InputDevice device)
        {
            foreach (InputControl control in InputSystem.FindControls(binding.effectivePath))
            {
                if (control.device == device)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsBindingInGroup(InputBinding binding, string group)
        {
            if (string.IsNullOrEmpty(binding.groups))
            {
                return false;
            }

            string[] groups = binding.groups.Split(';');
            foreach (string bindingGroup in groups)
            {
                if (string.Equals(bindingGroup, group, System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsKeyboardOrMouse(InputDevice device)
        {
            return device is Keyboard || device is Mouse;
        }

        private void RememberInputDevice(InputAction.CallbackContext context)
        {
            _lastUsedDevice = context.control?.device;
        }
    }
}
