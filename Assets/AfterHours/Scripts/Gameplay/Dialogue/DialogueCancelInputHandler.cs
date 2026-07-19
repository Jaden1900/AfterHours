using AfterHours.Gameplay.Player;
using UnityEngine;

namespace AfterHours.Gameplay.Dialogue
{
    public sealed class DialogueCancelInputHandler : MonoBehaviour
    {
        [SerializeField] private DialogueController _dialogueController;
        [SerializeField] private PlayerInputReader _inputReader;

        private void Update()
        {
            if (_dialogueController == null || !_dialogueController.IsDialogueOpen || _inputReader == null)
            {
                return;
            }

            if (_inputReader.ConsumeCancelPressed())
            {
                _dialogueController.CloseDialogue();
            }
        }
    }
}
