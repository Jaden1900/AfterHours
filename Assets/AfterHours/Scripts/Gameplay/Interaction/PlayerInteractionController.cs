using AfterHours.Gameplay.Player;
using UnityEngine;

namespace AfterHours.Gameplay.Interaction
{
    public sealed class PlayerInteractionController : MonoBehaviour
    {
        [SerializeField] private PlayerInputReader _inputReader;
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private float _interactionDistance = 3f;
        [SerializeField] private LayerMask _interactionLayers = ~0;

        public IInteractable CurrentInteractable { get; private set; }
        public string CurrentPrompt { get; private set; }
        public bool HasValidTarget => CurrentInteractable != null;

        private void Update()
        {
            UpdateCurrentInteractable();

            if (_inputReader == null || !_inputReader.ConsumeInteractPressed())
            {
                return;
            }

            CurrentInteractable?.Interact(gameObject);
        }

        private void UpdateCurrentInteractable()
        {
            if (_viewTransform == null)
            {
                ClearCurrentInteractable();
                return;
            }

            Ray ray = new Ray(_viewTransform.position, _viewTransform.forward);
            if (!Physics.Raycast(ray, out RaycastHit hit, _interactionDistance, _interactionLayers, QueryTriggerInteraction.Ignore))
            {
                ClearCurrentInteractable();
                return;
            }

            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if (interactable == null || !interactable.CanInteract)
            {
                ClearCurrentInteractable();
                return;
            }

            CurrentInteractable = interactable;
            CurrentPrompt = interactable.Prompt;
        }

        private void ClearCurrentInteractable()
        {
            CurrentInteractable = null;
            CurrentPrompt = null;
        }

        private void OnValidate()
        {
            _interactionDistance = Mathf.Max(0f, _interactionDistance);
        }
    }
}
