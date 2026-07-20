namespace AfterHours.Gameplay.Investigation
{
    /// <summary>
    /// Evaluates investigation-based requirements without coupling dialogue to a specific NPC or dialogue UI.
    /// </summary>
    public sealed class DialogueConditionEvaluator
    {
        private readonly InvestigationState _investigationState;

        public DialogueConditionEvaluator(InvestigationState investigationState)
        {
            _investigationState = investigationState;
        }

        public bool HasEvidence(string evidenceId)
        {
            return _investigationState != null && _investigationState.HasEvidence(evidenceId);
        }

        public bool HasFlag(string flag)
        {
            return _investigationState != null && _investigationState.HasFlag(flag);
        }
    }
}
