using AfterHours.Gameplay.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AfterHours.UI
{
    public sealed class DialoguePresenter : MonoBehaviour
    {
        [SerializeField] private DialogueController _dialogueController;
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private TMP_Text _speakerNameText;
        [SerializeField] private TMP_Text _dialogueBodyText;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            HideDialogue();
        }

        private void OnEnable()
        {
            if (_dialogueController != null)
            {
                _dialogueController.DialogueOpened += ShowDialogue;
                _dialogueController.DialoguePageChanged += ShowPage;
                _dialogueController.DialogueClosed += HideDialogue;
            }

            if (_closeButton != null)
            {
                _closeButton.onClick.AddListener(CloseDialogue);
            }

            if (_continueButton != null)
            {
                _continueButton.onClick.AddListener(ContinueDialogue);
            }

            RefreshPresentation();
        }

        private void OnDisable()
        {
            if (_dialogueController != null)
            {
                _dialogueController.DialogueOpened -= ShowDialogue;
                _dialogueController.DialoguePageChanged -= ShowPage;
                _dialogueController.DialogueClosed -= HideDialogue;
            }

            if (_closeButton != null)
            {
                _closeButton.onClick.RemoveListener(CloseDialogue);
            }

            if (_continueButton != null)
            {
                _continueButton.onClick.RemoveListener(ContinueDialogue);
            }
        }

        private void ShowDialogue(string speakerName, string dialogueText)
        {
            SetText(speakerName, dialogueText);
            SetPanelActive(true);
        }

        private void HideDialogue()
        {
            SetPanelActive(false);
        }

        private void ShowPage(string dialogueText)
        {
            if (_dialogueController == null)
            {
                return;
            }

            SetText(_dialogueController.CurrentSpeakerName, dialogueText);
            SetPanelActive(true);
        }

        private void CloseDialogue()
        {
            _dialogueController?.CloseDialogue();
        }

        private void ContinueDialogue()
        {
            _dialogueController?.Next();
        }

        private void RefreshPresentation()
        {
            bool isDialogueOpen = _dialogueController != null && _dialogueController.IsDialogueOpen;
            if (isDialogueOpen)
            {
                SetText(_dialogueController.CurrentSpeakerName, _dialogueController.CurrentDialogueText);
            }

            SetPanelActive(isDialogueOpen);
        }

        private void SetText(string speakerName, string dialogueText)
        {
            if (_speakerNameText != null)
            {
                _speakerNameText.text = speakerName;
            }

            if (_dialogueBodyText != null)
            {
                _dialogueBodyText.text = dialogueText;
            }
        }

        private void SetPanelActive(bool isActive)
        {
            if (_dialoguePanel != null)
            {
                _dialoguePanel.SetActive(isActive);
            }
        }
    }
}
