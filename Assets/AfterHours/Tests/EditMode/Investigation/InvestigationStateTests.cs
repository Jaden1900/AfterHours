using System.Reflection;
using AfterHours.Gameplay.Investigation;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.Investigation
{
    public sealed class InvestigationStateTests
    {
        [Test]
        public void TryCollectEvidence_CollectsOnceAndRaisesOneEvent()
        {
            GameObject stateObject = new GameObject();
            InvestigationState state = stateObject.AddComponent<InvestigationState>();
            int eventCount = 0;
            state.EvidenceCollected += _ => eventCount++;

            Assert.That(state.TryCollectEvidence("office_key"), Is.True);
            Assert.That(state.TryCollectEvidence("office_key"), Is.False);
            Assert.That(state.HasEvidence("office_key"), Is.True);
            Assert.That(eventCount, Is.EqualTo(1));

            Object.DestroyImmediate(stateObject);
        }

        [Test]
        public void SetFlag_AddsOnceAndRaisesOneEvent()
        {
            GameObject stateObject = new GameObject();
            InvestigationState state = stateObject.AddComponent<InvestigationState>();
            int eventCount = 0;
            state.FlagChanged += _ => eventCount++;

            state.SetFlag("spoke_to_bartender");
            state.SetFlag("spoke_to_bartender");

            Assert.That(state.HasFlag("spoke_to_bartender"), Is.True);
            Assert.That(eventCount, Is.EqualTo(1));

            Object.DestroyImmediate(stateObject);
        }

        [Test]
        public void EvidenceData_ExposesAuthoredReadOnlyValues()
        {
            EvidenceData evidenceData = ScriptableObject.CreateInstance<EvidenceData>();
            SetPrivateField(evidenceData, "_evidenceId", "receipt");
            SetPrivateField(evidenceData, "_displayName", "Bar Receipt");
            SetPrivateField(evidenceData, "_description", "A receipt from last night.");

            Assert.That(evidenceData.EvidenceId, Is.EqualTo("receipt"));
            Assert.That(evidenceData.DisplayName, Is.EqualTo("Bar Receipt"));
            Assert.That(evidenceData.Description, Is.EqualTo("A receipt from last night."));

            Object.DestroyImmediate(evidenceData);
        }

        [Test]
        public void EvidenceInteractable_CollectsOnlyOnce()
        {
            GameObject stateObject = new GameObject();
            InvestigationState state = stateObject.AddComponent<InvestigationState>();
            GameObject evidenceObject = new GameObject();
            EvidenceInteractable interactable = evidenceObject.AddComponent<EvidenceInteractable>();
            EvidenceData evidenceData = ScriptableObject.CreateInstance<EvidenceData>();
            SetPrivateField(evidenceData, "_evidenceId", "receipt");
            SetPrivateField(interactable, "_evidenceData", evidenceData);
            SetPrivateField(interactable, "_investigationState", state);
            int eventCount = 0;
            interactable.EvidenceCollected += _ => eventCount++;

            interactable.Interact(null);
            interactable.Interact(null);

            Assert.That(state.HasEvidence("receipt"), Is.True);
            Assert.That(interactable.CanInteract, Is.False);
            Assert.That(eventCount, Is.EqualTo(1));

            Object.DestroyImmediate(evidenceObject);
            Object.DestroyImmediate(stateObject);
            Object.DestroyImmediate(evidenceData);
        }

        [Test]
        public void DialogueConditionEvaluator_EvaluatesEvidenceAndFlags()
        {
            GameObject stateObject = new GameObject();
            InvestigationState state = stateObject.AddComponent<InvestigationState>();
            state.CollectEvidence("receipt");
            state.SetFlag("met_bartender");
            DialogueConditionEvaluator evaluator = new DialogueConditionEvaluator(state);

            Assert.That(evaluator.HasEvidence("receipt"), Is.True);
            Assert.That(evaluator.HasEvidence("missing"), Is.False);
            Assert.That(evaluator.HasFlag("met_bartender"), Is.True);
            Assert.That(evaluator.HasFlag("missing"), Is.False);

            Object.DestroyImmediate(stateObject);
        }

        private static void SetPrivateField(object instance, string fieldName, object value)
        {
            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(instance, value);
        }
    }
}
