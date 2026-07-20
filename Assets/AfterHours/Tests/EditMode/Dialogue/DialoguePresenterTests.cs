using System.Reflection;
using AfterHours.Gameplay.Dialogue;
using AfterHours.UI;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.Dialogue
{
    public sealed class DialoguePresenterTests
    {
        [Test]
        public void DialogueEvents_OpenAndClosePanel()
        {
            GameObject controllerObject = new GameObject();
            DialogueController controller = controllerObject.AddComponent<DialogueController>();
            GameObject panel = new GameObject();
            GameObject presenterObject = new GameObject();
            presenterObject.SetActive(false);
            DialoguePresenter presenter = presenterObject.AddComponent<DialoguePresenter>();
            SetPrivateField(presenter, "_dialogueController", controller);
            SetPrivateField(presenter, "_dialoguePanel", panel);

            presenterObject.SetActive(true);
            Assert.That(panel.activeSelf, Is.False);

            controller.TryOpenDialogue("Alex", "Good evening.");
            Assert.That(panel.activeSelf, Is.True);

            controller.CloseDialogue();
            Assert.That(panel.activeSelf, Is.False);
            Object.DestroyImmediate(presenterObject);
            Object.DestroyImmediate(panel);
            Object.DestroyImmediate(controllerObject);
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(target, value);
        }
    }
}
