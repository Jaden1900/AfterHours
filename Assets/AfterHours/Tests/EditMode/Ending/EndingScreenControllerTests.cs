using System.Reflection;
using AfterHours.Gameplay.Ending;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.Ending
{
    public sealed class EndingScreenControllerTests
    {
        [SetUp]
        public void SetUp()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        [Test]
        public void Awake_HidesConfiguredEndingScreen()
        {
            GameObject root = new GameObject("Ending Screen");
            EndingScreenController controller = CreateController(root);

            Assert.That(root.activeSelf, Is.False);
            Assert.That(controller.IsEndingShown, Is.False);

            Object.DestroyImmediate(controller.gameObject);
            Object.DestroyImmediate(root);
        }

        [Test]
        public void ShowEnding_ActivatesRoot()
        {
            GameObject root = new GameObject("Ending Screen");
            EndingScreenController controller = CreateController(root);

            controller.ShowEnding();

            Assert.That(root.activeSelf, Is.True);
            Assert.That(controller.IsEndingShown, Is.True);

            Object.DestroyImmediate(controller.gameObject);
            Object.DestroyImmediate(root);
        }

        [Test]
        public void ShowEnding_DuplicateCallsPublishEndingShownOnlyOnce()
        {
            GameObject root = new GameObject("Ending Screen");
            EndingScreenController controller = CreateController(root);
            int shownCount = 0;
            controller.EndingShown += () => shownCount++;

            controller.ShowEnding();
            controller.ShowEnding();

            Assert.That(shownCount, Is.EqualTo(1));

            Object.DestroyImmediate(controller.gameObject);
            Object.DestroyImmediate(root);
        }

        [Test]
        public void HideEnding_HidesRootAndRestoresTimeAndCursorState()
        {
            GameObject root = new GameObject("Ending Screen");
            EndingScreenController controller = CreateController(root);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

            controller.ShowEnding();
            Time.timeScale = 0f;
            controller.HideEnding();

            Assert.That(root.activeSelf, Is.False);
            Assert.That(controller.IsEndingShown, Is.False);
            Assert.That(Time.timeScale, Is.EqualTo(1f));
            Assert.That(Cursor.lockState, Is.EqualTo(CursorLockMode.Confined));
            Assert.That(Cursor.visible, Is.False);

            Object.DestroyImmediate(controller.gameObject);
            Object.DestroyImmediate(root);
        }

        private static EndingScreenController CreateController(GameObject root)
        {
            GameObject controllerObject = new GameObject("Ending Screen Controller");
            controllerObject.SetActive(false);
            EndingScreenController controller = controllerObject.AddComponent<EndingScreenController>();
            SetPrivateField(controller, "_endingScreenRoot", root);
            controllerObject.SetActive(true);
            return controller;
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(target, value);
        }
    }
}
