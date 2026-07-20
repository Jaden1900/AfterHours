using System.Reflection;
using AfterHours.Gameplay.Interaction;
using AfterHours.Gameplay.Player;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.Interaction
{
    public sealed class InteractionTests
    {
        [Test]
        public void ConsumeInteractPressed_ReturnsTrueOnlyOncePerPress()
        {
            GameObject player = new GameObject();
            PlayerInputReader inputReader = player.AddComponent<PlayerInputReader>();

            InvokePrivateMethod(inputReader, "OnInteract");

            Assert.That(inputReader.ConsumeInteractPressed(), Is.True);
            Assert.That(inputReader.ConsumeInteractPressed(), Is.False);

            Object.DestroyImmediate(player);
        }

        [Test]
        public void TestInteractable_ExposesConfiguredPromptAndAvailability()
        {
            GameObject target = new GameObject();
            TestInteractable interactable = target.AddComponent<TestInteractable>();

            SetPrivateField(interactable, "_prompt", "Read note");
            SetPrivateField(interactable, "_canInteract", false);

            Assert.That(interactable.Prompt, Is.EqualTo("Read note"));
            Assert.That(interactable.CanInteract, Is.False);

            Object.DestroyImmediate(target);
        }

        [Test]
        public void DoorInteractable_StartsClosed()
        {
            GameObject doorObject = new GameObject();
            DoorInteractable door = doorObject.AddComponent<DoorInteractable>();

            Assert.That(door.IsOpen, Is.False);
            Assert.That(door.CanInteract, Is.True);
            Assert.That(door.Prompt, Is.EqualTo("Open Door"));

            Object.DestroyImmediate(doorObject);
        }

        [Test]
        public void DoorInteractable_InteractionOpensDoorAndAppliesAssignedState()
        {
            GameObject doorObject = new GameObject();
            DoorInteractable door = doorObject.AddComponent<DoorInteractable>();
            GameObject closedVisual = new GameObject();
            GameObject openVisual = new GameObject();
            GameObject blockerObject = new GameObject();
            BoxCollider blockingCollider = blockerObject.AddComponent<BoxCollider>();

            SetPrivateField(door, "_closedDoorVisual", closedVisual);
            SetPrivateField(door, "_openDoorVisual", openVisual);
            SetPrivateField(door, "_blockingCollider", blockingCollider);
            InvokePrivateMethod(door, "Awake");

            door.Interact(null);

            Assert.That(door.IsOpen, Is.True);
            Assert.That(door.CanInteract, Is.False);
            Assert.That(door.Prompt, Is.Null);
            Assert.That(closedVisual.activeSelf, Is.False);
            Assert.That(openVisual.activeSelf, Is.True);
            Assert.That(blockingCollider.enabled, Is.False);

            Object.DestroyImmediate(doorObject);
            Object.DestroyImmediate(closedVisual);
            Object.DestroyImmediate(openVisual);
            Object.DestroyImmediate(blockerObject);
        }

        [Test]
        public void DoorInteractable_DuplicateInteractionFiresDoorOpenedOnlyOnce()
        {
            GameObject doorObject = new GameObject();
            DoorInteractable door = doorObject.AddComponent<DoorInteractable>();
            int openedCount = 0;
            door.DoorOpened += () => openedCount++;

            door.Interact(null);
            door.Interact(null);

            Assert.That(openedCount, Is.EqualTo(1));

            Object.DestroyImmediate(doorObject);
        }

        [Test]
        public void DoorInteractable_StartsOpenAppliesInitialState()
        {
            GameObject doorObject = new GameObject();
            DoorInteractable door = doorObject.AddComponent<DoorInteractable>();
            GameObject closedVisual = new GameObject();
            GameObject openVisual = new GameObject();
            openVisual.SetActive(false);
            GameObject blockerObject = new GameObject();
            BoxCollider blockingCollider = blockerObject.AddComponent<BoxCollider>();

            SetPrivateField(door, "_closedDoorVisual", closedVisual);
            SetPrivateField(door, "_openDoorVisual", openVisual);
            SetPrivateField(door, "_blockingCollider", blockingCollider);
            SetPrivateField(door, "_startsOpen", true);
            InvokePrivateMethod(door, "Awake");

            Assert.That(door.IsOpen, Is.True);
            Assert.That(door.CanInteract, Is.False);
            Assert.That(door.Prompt, Is.Null);
            Assert.That(closedVisual.activeSelf, Is.False);
            Assert.That(openVisual.activeSelf, Is.True);
            Assert.That(blockingCollider.enabled, Is.False);

            Object.DestroyImmediate(doorObject);
            Object.DestroyImmediate(closedVisual);
            Object.DestroyImmediate(openVisual);
            Object.DestroyImmediate(blockerObject);
        }

        [Test]
        public void CurrentTarget_ClearsWhenInteractableBecomesUnavailable()
        {
            GameObject controllerObject = new GameObject();
            PlayerInteractionController controller = controllerObject.AddComponent<PlayerInteractionController>();
            GameObject view = new GameObject();
            GameObject target = new GameObject();
            TestInteractable interactable = target.AddComponent<TestInteractable>();
            GameObject colliderObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            colliderObject.transform.SetParent(target.transform);
            colliderObject.transform.position = Vector3.forward * 2f;

            SetPrivateField(controller, "_viewTransform", view.transform);
            Physics.SyncTransforms();
            InvokePrivateMethod(controller, "Update");
            Assert.That(controller.CurrentInteractable, Is.SameAs(interactable));
            Assert.That(controller.HasValidTarget, Is.True);

            SetPrivateField(interactable, "_canInteract", false);
            InvokePrivateMethod(controller, "Update");

            Assert.That(controller.CurrentInteractable, Is.Null);
            Assert.That(controller.CurrentPrompt, Is.Null);
            Assert.That(controller.HasValidTarget, Is.False);

            Object.DestroyImmediate(controllerObject);
            Object.DestroyImmediate(view);
            Object.DestroyImmediate(target);
        }

        [Test]
        public void CurrentTarget_ResolvesInteractableOnColliderParent()
        {
            GameObject controllerObject = new GameObject();
            PlayerInteractionController controller = controllerObject.AddComponent<PlayerInteractionController>();
            GameObject view = new GameObject();
            GameObject target = new GameObject();
            TestInteractable interactable = target.AddComponent<TestInteractable>();
            GameObject colliderObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            colliderObject.transform.SetParent(target.transform);
            colliderObject.transform.position = Vector3.forward * 2f;

            SetPrivateField(controller, "_viewTransform", view.transform);
            Physics.SyncTransforms();
            InvokePrivateMethod(controller, "Update");

            Assert.That(controller.CurrentInteractable, Is.SameAs(interactable));
            Assert.That(controller.CurrentPrompt, Is.EqualTo(interactable.Prompt));

            Object.DestroyImmediate(controllerObject);
            Object.DestroyImmediate(view);
            Object.DestroyImmediate(target);
        }

        private static void InvokePrivateMethod(object instance, string methodName)
        {
            MethodInfo method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            object[] arguments = method.GetParameters().Length == 0
                ? null
                : new object[] { default(UnityEngine.InputSystem.InputAction.CallbackContext) };
            method.Invoke(instance, arguments);
        }

        private static void SetPrivateField(object instance, string fieldName, object value)
        {
            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(instance, value);
        }
    }
}
