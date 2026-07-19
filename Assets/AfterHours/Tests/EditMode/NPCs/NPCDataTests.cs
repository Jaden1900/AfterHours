using System.Reflection;
using AfterHours.Gameplay.NPCs;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.NPCs
{
    public sealed class NPCDataTests
    {
        [Test]
        public void InteractionPrompt_UsesDisplayNameFallbackWhenNoPromptIsAuthored()
        {
            NPCData npcData = ScriptableObject.CreateInstance<NPCData>();
            SetPrivateField(npcData, "_displayName", "Alex");
            SetPrivateField(npcData, "_interactionPrompt", string.Empty);

            Assert.That(npcData.InteractionPrompt, Is.EqualTo("Talk to Alex"));

            Object.DestroyImmediate(npcData);
        }

        private static void SetPrivateField(object instance, string fieldName, object value)
        {
            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(instance, value);
        }
    }
}
