using System;
using UnityEngine;

namespace AfterHours.Gameplay.Dialogue
{
    public sealed class DialogueController : MonoBehaviour
    {
        public bool IsDialogueOpen { get; private set; }
        public string CurrentSpeakerName { get; private set; }
        public string CurrentDialogueText { get; private set; }

        public event Action<string, string> DialogueOpened;
        public event Action DialogueClosed;

        public bool TryOpenDialogue(string speakerName, string dialogueText)
        {
            if (IsDialogueOpen || string.IsNullOrWhiteSpace(speakerName) || string.IsNullOrWhiteSpace(dialogueText))
            {
                return false;
            }

            CurrentSpeakerName = speakerName.Trim();
            CurrentDialogueText = dialogueText.Trim();
            IsDialogueOpen = true;
            DialogueOpened?.Invoke(CurrentSpeakerName, CurrentDialogueText);
            return true;
        }

        public void CloseDialogue()
        {
            if (!IsDialogueOpen)
            {
                return;
            }

            IsDialogueOpen = false;
            CurrentSpeakerName = null;
            CurrentDialogueText = null;
            DialogueClosed?.Invoke();
        }
    }
}
