using System;
using System.Collections.Generic;
using UnityEngine;

namespace AfterHours.Gameplay.Investigation
{
    /// <summary>
    /// Owns the runtime-only progress for one investigation session.
    /// </summary>
    public sealed class InvestigationState : MonoBehaviour
    {
        private readonly HashSet<string> _evidenceIds = new(StringComparer.Ordinal);
        private readonly HashSet<string> _flags = new(StringComparer.Ordinal);

        public event Action<string> EvidenceCollected;
        public event Action<string> FlagChanged;

        public bool HasEvidence(string evidenceId)
        {
            return TryNormalizeId(evidenceId, out string normalizedEvidenceId)
                && _evidenceIds.Contains(normalizedEvidenceId);
        }

        public void CollectEvidence(string evidenceId)
        {
            TryCollectEvidence(evidenceId);
        }

        public bool TryCollectEvidence(string evidenceId)
        {
            if (!TryNormalizeId(evidenceId, out string normalizedEvidenceId) || !_evidenceIds.Add(normalizedEvidenceId))
            {
                return false;
            }

            EvidenceCollected?.Invoke(normalizedEvidenceId);
            return true;
        }

        public bool HasFlag(string flag)
        {
            return TryNormalizeId(flag, out string normalizedFlag) && _flags.Contains(normalizedFlag);
        }

        public void SetFlag(string flag)
        {
            if (!TryNormalizeId(flag, out string normalizedFlag) || !_flags.Add(normalizedFlag))
            {
                return;
            }

            FlagChanged?.Invoke(normalizedFlag);
        }

        private static bool TryNormalizeId(string value, out string normalizedValue)
        {
            normalizedValue = value?.Trim();
            return !string.IsNullOrWhiteSpace(normalizedValue);
        }
    }
}
