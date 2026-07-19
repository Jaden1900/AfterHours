using System.Reflection;
using AfterHours.Gameplay.Dialogue;
using AfterHours.Gameplay.NPCs;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.NPCs
{
    public sealed class NPCInteractableTests
    {
        [Test]
        public void CanInteract_RequiresDataAndDialogueControllerAndNoActiveDialogue()
        {
            GameObject npcObject = new GameObject();
            NPCInteractable interactable = npcObject.AddComponent<NPCInteractable>();
            NPCData npcData = ScriptableObject.CreateInstance<NPCData>();
            GameObject controllerObject = new GameObject();
            DialogueController controller = controllerObject.AddComponent<DialogueController>();

            Assert.That(interactable.CanInteract, Is.False);

            SetPrivateField(interactable, "_npcData", npcData);
            Assert.That(interactable.CanInteract, Is.False);

            SetPrivateField(interactable, "_dialogueController", controller);
            Assert.That(interactable.CanInteract, Is.True);

            controller.TryOpenDialogue("Alex", "Good evening.");
            Assert.That(interactable.CanInteract, Is.False);

            Object.DestroyImmediate(npcObject);
            Object.DestroyImmediate(controllerObject);
            Object.DestroyImmediate(npcData);
        }

        private static void SetPrivateField(object instance, string fieldName, object value)
        {
            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(instance, value);
        }
    }
}
