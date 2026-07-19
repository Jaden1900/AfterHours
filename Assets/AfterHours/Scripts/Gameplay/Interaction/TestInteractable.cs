using UnityEngine;

namespace AfterHours.Gameplay.Interaction
{
    // Sandbox-only component for validating the reusable interaction flow.
    public sealed class TestInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt = "Inspect test object";
        [SerializeField] private bool _canInteract = true;
        [SerializeField] private bool _countInteractions = true;

        public string Prompt => _prompt;
        public bool CanInteract => _canInteract;
        public int InteractionCount { get; private set; }

        public void Interact(GameObject interactor)
        {
            if (!CanInteract)
            {
                return;
            }

            if (_countInteractions)
            {
                InteractionCount++;
            }

            Debug.Log($"Test interactable '{name}' was interacted with by '{interactor.name}'.", this);
        }
    }
}
