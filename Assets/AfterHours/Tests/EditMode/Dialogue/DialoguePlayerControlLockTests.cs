using System.Reflection;
using AfterHours.Gameplay.Dialogue;
using AfterHours.Gameplay.Player;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.Dialogue
{
    public sealed class DialoguePlayerControlLockTests
    {
        [Test]
        public void DialogueState_DisablesAndRestoresMovementAndCameraControls()
        {
            GameObject dialogueObject = new GameObject();
            DialogueController controller = dialogueObject.AddComponent<DialogueController>();
            GameObject playerObject = new GameObject();
            playerObject.AddComponent<CharacterController>();
            PlayerMovement movement = playerObject.AddComponent<PlayerMovement>();
            GameObject cameraObject = new GameObject();
            ThirdPersonCameraController cameraController = cameraObject.AddComponent<ThirdPersonCameraController>();
            GameObject lockObject = new GameObject();
            lockObject.SetActive(false);
            DialoguePlayerControlLock controlLock = lockObject.AddComponent<DialoguePlayerControlLock>();
            SetPrivateField(controlLock, "_dialogueController", controller);
            SetPrivateField(controlLock, "_playerMovement", movement);
            SetPrivateField(controlLock, "_cameraController", cameraController);
            lockObject.SetActive(true);

            controller.TryOpenDialogue("Alex", "Good evening.");

            Assert.That(movement.enabled, Is.False);
            Assert.That(cameraController.enabled, Is.False);

            controller.CloseDialogue();

            Assert.That(movement.enabled, Is.True);
            Assert.That(cameraController.enabled, Is.True);
            Object.DestroyImmediate(lockObject);
            Object.DestroyImmediate(cameraObject);
            Object.DestroyImmediate(playerObject);
            Object.DestroyImmediate(dialogueObject);
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(target, value);
        }
    }
}
