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
