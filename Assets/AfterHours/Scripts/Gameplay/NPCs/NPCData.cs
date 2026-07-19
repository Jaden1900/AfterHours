using UnityEngine;

namespace AfterHours.Gameplay.NPCs
{
    [CreateAssetMenu(fileName = "NPCData", menuName = "AfterHours/NPC Data")]
    public sealed class NPCData : ScriptableObject
    {
        [SerializeField] private string _npcId;
        [SerializeField] private string _displayName;
        [SerializeField] [TextArea] private string _initialDialogueText;
        [SerializeField] private string _interactionPrompt;

        public string NpcId => _npcId;
        public string DisplayName => string.IsNullOrWhiteSpace(_displayName) ? "Unknown" : _displayName;
        public string InteractionPrompt => string.IsNullOrWhiteSpace(_interactionPrompt)
            ? $"Talk to {DisplayName}"
            : _interactionPrompt;
        public string InitialDialogueText => _initialDialogueText ?? string.Empty;

        private void OnValidate()
        {
            _npcId = _npcId?.Trim();
            _displayName = _displayName?.Trim();
            _interactionPrompt = _interactionPrompt?.Trim();
            _initialDialogueText = _initialDialogueText?.Trim();
        }
    }
}
