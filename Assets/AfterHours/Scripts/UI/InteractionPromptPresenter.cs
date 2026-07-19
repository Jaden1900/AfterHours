using AfterHours.Gameplay.Interaction;
using TMPro;
using UnityEngine;

namespace AfterHours.UI
{
    public sealed class InteractionPromptPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerInteractionController _interactionController;
        [SerializeField] private TMP_Text _promptText;

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
                _promptText.text = _interactionController.CurrentPrompt;
            }
        }
    }
}
