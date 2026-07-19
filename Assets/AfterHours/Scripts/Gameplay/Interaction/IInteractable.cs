using UnityEngine;

namespace AfterHours.Gameplay.Interaction
{
    public interface IInteractable
    {
        string Prompt { get; }

        bool CanInteract { get; }

        void Interact(GameObject interactor);
    }
}
