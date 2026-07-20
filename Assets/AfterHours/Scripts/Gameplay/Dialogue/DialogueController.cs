using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AfterHours.Gameplay.Dialogue
{
    public sealed class DialogueController : MonoBehaviour
    {
        public bool IsDialogueOpen { get; private set; }
        public string CurrentSpeakerName { get; private set; }
        public string CurrentDialogueText { get; private set; }
        public int CurrentPageIndex { get; private set; }
        public int PageCount => _pages.Count;
        public bool HasNextPage => IsDialogueOpen && CurrentPageIndex < PageCount - 1;

        private readonly List<string> _pages = new();

        public event Action<string, string> DialogueOpened;
        public event Action<string> DialoguePageChanged;
        public event Action DialogueClosed;

        public bool TryOpenDialogue(string speakerName, string dialogueText)
        {
            return TryOpenDialogue(speakerName, new[] { dialogueText });
        }

        public bool TryOpenDialogue(string speakerName, IEnumerable<string> dialoguePages)
        {
            if (IsDialogueOpen || string.IsNullOrWhiteSpace(speakerName) || dialoguePages == null)
            {
                return false;
            }

            List<string> pages = dialoguePages
                .Where(page => !string.IsNullOrWhiteSpace(page))
                .Select(page => page.Trim())
                .ToList();
            if (pages.Count == 0)
            {
                return false;
            }

            CurrentSpeakerName = speakerName.Trim();
            _pages.AddRange(pages);
            CurrentPageIndex = 0;
            CurrentDialogueText = _pages[CurrentPageIndex];
            IsDialogueOpen = true;
            DialogueOpened?.Invoke(CurrentSpeakerName, CurrentDialogueText);
            return true;
        }

        public void Next()
        {
            if (!IsDialogueOpen)
            {
                return;
            }

            if (!HasNextPage)
            {
                CloseDialogue();
                return;
            }

            CurrentPageIndex++;
            CurrentDialogueText = _pages[CurrentPageIndex];
            DialoguePageChanged?.Invoke(CurrentDialogueText);
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
            CurrentPageIndex = 0;
            _pages.Clear();
            DialogueClosed?.Invoke();
        }
    }
}
