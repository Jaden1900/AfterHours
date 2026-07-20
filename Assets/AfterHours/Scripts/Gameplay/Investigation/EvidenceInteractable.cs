using System;
using AfterHours.Gameplay.Interaction;
using UnityEngine;

namespace AfterHours.Gameplay.Investigation
{
    public sealed class EvidenceInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private EvidenceData _evidenceData;
        [SerializeField] private InvestigationState _investigationState;
        [SerializeField] private string _prompt = "Inspect Evidence";

        public event Action<EvidenceData> EvidenceCollected;

        public string Prompt => string.IsNullOrWhiteSpace(_prompt) ? "Inspect Evidence" : _prompt;
        public bool CanInteract => _evidenceData != null
            && _investigationState != null
            && !_investigationState.HasEvidence(_evidenceData.EvidenceId);

        public void Interact(GameObject interactor)
        {
            if (!CanInteract || !_investigationState.TryCollectEvidence(_evidenceData.EvidenceId))
            {
                return;
            }

            EvidenceCollected?.Invoke(_evidenceData);
        }

        private void OnValidate()
        {
            _prompt = _prompt?.Trim();
        }
    }
}
