using System.Reflection;
using AfterHours.Gameplay.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AfterHours.Tests.EditMode.Player
{
    public sealed class PlayerInputReaderTests
    {
        private static readonly FieldInfo InteractActionField = typeof(PlayerInputReader)
            .GetField("_interactAction", BindingFlags.Instance | BindingFlags.NonPublic);

        [Test]
        public void InteractionBindingDisplayName_KeyboardBinding_ReturnsConfiguredKeyDisplayName()
        {
            InputAction interactAction = new InputAction("Interact");
            interactAction.AddBinding("<Keyboard>/e", groups: "Keyboard&Mouse");
            PlayerInputReader reader = CreateReader(interactAction);

            Assert.That(reader.InteractionBindingDisplayName,
                Is.EqualTo(interactAction.GetBindingDisplayString(0)));
            Assert.That(reader.InteractionBindingDisplayName, Is.EqualTo("E"));

            Object.DestroyImmediate(reader.gameObject);
            interactAction.Dispose();
        }

        [Test]
        public void InteractionBindingDisplayName_MultipleBindings_ReturnsKeyboardBindingOnly()
        {
            InputAction interactAction = new InputAction("Interact");
            interactAction.AddBinding("<Keyboard>/e", groups: "Keyboard&Mouse");
            interactAction.AddBinding("<Gamepad>/buttonSouth", groups: "Gamepad");
            PlayerInputReader reader = CreateReader(interactAction);

            Assert.That(reader.InteractionBindingDisplayName,
                Is.EqualTo(interactAction.GetBindingDisplayString(0)));
            Assert.That(reader.InteractionBindingDisplayName,
                Is.Not.EqualTo(interactAction.GetBindingDisplayString(1)));

            Object.DestroyImmediate(reader.gameObject);
            interactAction.Dispose();
        }

        [Test]
        public void InteractionBindingDisplayName_MissingAction_ReturnsEmptyString()
        {
            PlayerInputReader reader = CreateReader(null);

            Assert.That(reader.InteractionBindingDisplayName, Is.Empty);

            Object.DestroyImmediate(reader.gameObject);
        }

        private static PlayerInputReader CreateReader(InputAction interactAction)
        {
            GameObject gameObject = new GameObject("PlayerInputReaderTests");
            PlayerInputReader reader = gameObject.AddComponent<PlayerInputReader>();
            InteractActionField.SetValue(reader, interactAction);
            return reader;
        }
    }
}
