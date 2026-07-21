using AfterHours.Gameplay.Dialogue;
using AfterHours.Gameplay.Ending;
using AfterHours.Gameplay.Interaction;
using UnityEngine;

namespace AfterHours.Gameplay.NPCs
{
    public sealed class NPCInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private NPCData _npcData;
        [SerializeField] private DialogueController _dialogueController;
        [SerializeField] private bool _showEndingWhenDialogueCloses;
        [SerializeField] private EndingScreenController _endingScreenController;

        private bool _isWaitingForDialogueToClose;

        public string Prompt => _npcData == null ? string.Empty : _npcData.InteractionPrompt;
        public bool CanInteract => _npcData != null
            && _dialogueController != null
            && !_dialogueController.IsDialogueOpen;

        public void Interact(GameObject interactor)
        {
            if (!CanInteract)
            {
                return;
            }

            if (!_dialogueController.TryOpenDialogue(_npcData.DisplayName, _npcData.InitialDialogueText)
                || !_showEndingWhenDialogueCloses
                || _endingScreenController == null)
            {
                return;
            }

            _isWaitingForDialogueToClose = true;
            _dialogueController.DialogueClosed += ShowEndingAfterDialogueCloses;
        }

        private void OnDisable()
        {
            StopWaitingForDialogueToClose();
        }

        private void ShowEndingAfterDialogueCloses()
        {
            if (!_isWaitingForDialogueToClose)
            {
                return;
            }

            StopWaitingForDialogueToClose();
            _endingScreenController.ShowEnding();
        }

        private void StopWaitingForDialogueToClose()
        {
            if (!_isWaitingForDialogueToClose)
            {
                return;
            }

            _isWaitingForDialogueToClose = false;
            if (_dialogueController != null)
            {
                _dialogueController.DialogueClosed -= ShowEndingAfterDialogueCloses;
            }
        }
    }
}
