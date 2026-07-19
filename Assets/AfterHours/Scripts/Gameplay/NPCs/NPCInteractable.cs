using AfterHours.Gameplay.Dialogue;
using AfterHours.Gameplay.Interaction;
using UnityEngine;

namespace AfterHours.Gameplay.NPCs
{
    public sealed class NPCInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private NPCData _npcData;
        [SerializeField] private DialogueController _dialogueController;

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

            _dialogueController.TryOpenDialogue(_npcData.DisplayName, _npcData.InitialDialogueText);
        }
    }
}
