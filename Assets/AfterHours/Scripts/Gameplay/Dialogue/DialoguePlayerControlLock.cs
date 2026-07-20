using AfterHours.Gameplay.Player;
using UnityEngine;

namespace AfterHours.Gameplay.Dialogue
{
    /// <summary>Temporarily disables player controls while a dialogue is active.</summary>
    public sealed class DialoguePlayerControlLock : MonoBehaviour
    {
        [SerializeField] private DialogueController _dialogueController;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private ThirdPersonCameraController _cameraController;

        private bool _controlsLocked;
        private bool _movementWasEnabled;
        private bool _cameraWasEnabled;

        private void OnEnable()
        {
            if (_dialogueController != null)
            {
                _dialogueController.DialogueOpened += DisableControls;
                _dialogueController.DialogueClosed += RestoreControls;
            }

            SyncControlState();
        }

        private void OnDisable()
        {
            if (_dialogueController != null)
            {
                _dialogueController.DialogueOpened -= DisableControls;
                _dialogueController.DialogueClosed -= RestoreControls;
            }

            RestoreControls();
        }

        private void DisableControls(string _, string __)
        {
            if (_controlsLocked)
            {
                return;
            }

            _controlsLocked = true;
            if (_playerMovement != null)
            {
                _movementWasEnabled = _playerMovement.enabled;
                _playerMovement.enabled = false;
            }

            if (_cameraController != null)
            {
                _cameraWasEnabled = _cameraController.enabled;
                _cameraController.enabled = false;
            }
        }

        private void RestoreControls()
        {
            if (!_controlsLocked)
            {
                return;
            }

            _controlsLocked = false;
            if (_playerMovement != null)
            {
                _playerMovement.enabled = _movementWasEnabled;
            }

            if (_cameraController != null)
            {
                _cameraController.enabled = _cameraWasEnabled;
            }
        }

        private void SyncControlState()
        {
            if (_dialogueController != null && _dialogueController.IsDialogueOpen)
            {
                DisableControls(null, null);
            }
        }
    }
}
