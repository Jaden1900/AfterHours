using UnityEngine;

namespace AfterHours.Gameplay.Investigation
{
    [CreateAssetMenu(fileName = "EvidenceData", menuName = "AfterHours/Investigation/Evidence Data")]
    public sealed class EvidenceData : ScriptableObject
    {
        [SerializeField] private string _evidenceId;
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Sprite _icon;

        public string EvidenceId => _evidenceId?.Trim() ?? string.Empty;
        public string DisplayName => string.IsNullOrWhiteSpace(_displayName) ? EvidenceId : _displayName.Trim();
        public string Description => _description?.Trim() ?? string.Empty;
        public Sprite Icon => _icon;

        private void OnValidate()
        {
            _evidenceId = _evidenceId?.Trim();
            _displayName = _displayName?.Trim();
            _description = _description?.Trim();
        }
    }
}
