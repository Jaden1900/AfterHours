using System;
using UnityEngine;

namespace AfterHours.Gameplay.Interaction
{
    /// <summary>
    /// Opens a doorway by swapping optional visuals and disabling its blocking collider.
    /// </summary>
    public sealed class DoorInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _prompt = "Open Door";
        [SerializeField] private GameObject _closedDoorVisual;
        [SerializeField] private GameObject _openDoorVisual;
        [SerializeField] private Collider _blockingCollider;
        [SerializeField] private bool _startsOpen;

        private bool _isOpen;

        public string Prompt => _isOpen ? null : _prompt;
        public bool CanInteract => !_isOpen;
        public bool IsOpen => _isOpen;

        public event Action DoorOpened;

        private void Awake()
        {
            SetOpenState(_startsOpen);
        }

        public void Interact(GameObject interactor)
        {
            if (!CanInteract)
            {
                return;
            }

            SetOpenState(true);
            DoorOpened?.Invoke();
        }

        private void SetOpenState(bool isOpen)
        {
            _isOpen = isOpen;

            if (_closedDoorVisual != null)
            {
                _closedDoorVisual.SetActive(!isOpen);
            }

            if (_openDoorVisual != null)
            {
                _openDoorVisual.SetActive(isOpen);
            }

            if (_blockingCollider != null)
            {
                _blockingCollider.enabled = !isOpen;
            }
        }
    }
}
