using AfterHours.Gameplay.Interaction;
using AfterHours.Gameplay.Player;
using TMPro;
using UnityEngine;

namespace AfterHours.UI
{
    public sealed class InteractionPromptPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerInteractionController _interactionController;
        [SerializeField] private TMP_Text _promptText;
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private string _promptFormat = "[{0}] {1}";

        private void Update()
        {
            bool hasValidTarget = _interactionController != null && _interactionController.HasValidTarget;
            if (_promptText == null)
            {
                return;
            }

            _promptText.gameObject.SetActive(hasValidTarget);
            if (hasValidTarget)
            {
                _promptText.text = InteractionPromptFormatter.Format(
                    _promptFormat, _inputReader == null ? string.Empty : _inputReader.InteractionBindingDisplayName,
                    _interactionController.CurrentPrompt);
            }
        }
    }
}
