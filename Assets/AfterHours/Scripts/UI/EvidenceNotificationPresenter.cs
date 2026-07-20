using System.Collections.Generic;
using AfterHours.Gameplay.Investigation;
using TMPro;
using UnityEngine;

namespace AfterHours.UI
{
    /// <summary>
    /// Presents evidence notifications from investigation state while leaving evidence collection and UI state separate.
    /// </summary>
    public sealed class EvidenceNotificationPresenter : MonoBehaviour
    {
        [SerializeField] private InvestigationState _investigationState;
        [SerializeField] private EvidenceData[] _evidenceCatalog;
        [SerializeField] private GameObject _notificationPanel;
        [SerializeField] private TMP_Text _headingText;
        [SerializeField] private TMP_Text _displayNameText;
        [SerializeField] private TMP_Text _descriptionText;

        private readonly Dictionary<string, EvidenceData> _evidenceById = new();

        private void Awake()
        {
            BuildEvidenceLookup();
            SetPanelActive(false);
        }

        private void OnEnable()
        {
            if (_investigationState != null)
            {
                _investigationState.EvidenceCollected += ShowEvidence;
            }
        }

        private void OnDisable()
        {
            if (_investigationState != null)
            {
                _investigationState.EvidenceCollected -= ShowEvidence;
            }
        }

        private void ShowEvidence(string evidenceId)
        {
            if (!_evidenceById.TryGetValue(evidenceId, out EvidenceData evidenceData))
            {
                return;
            }

            if (_headingText != null)
            {
                _headingText.text = "Evidence Found";
            }

            if (_displayNameText != null)
            {
                _displayNameText.text = evidenceData.DisplayName;
            }

            if (_descriptionText != null)
            {
                _descriptionText.text = evidenceData.Description;
            }

            SetPanelActive(true);
        }

        private void BuildEvidenceLookup()
        {
            _evidenceById.Clear();
            if (_evidenceCatalog == null)
            {
                return;
            }

            foreach (EvidenceData evidenceData in _evidenceCatalog)
            {
                if (evidenceData == null || string.IsNullOrWhiteSpace(evidenceData.EvidenceId))
                {
                    continue;
                }

                _evidenceById[evidenceData.EvidenceId] = evidenceData;
            }
        }

        private void SetPanelActive(bool isActive)
        {
            if (_notificationPanel != null)
            {
                _notificationPanel.SetActive(isActive);
            }
        }
    }
}
