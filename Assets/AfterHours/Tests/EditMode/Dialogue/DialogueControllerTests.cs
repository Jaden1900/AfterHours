using AfterHours.Gameplay.Dialogue;
using NUnit.Framework;
using UnityEngine;

namespace AfterHours.Tests.EditMode.Dialogue
{
    public sealed class DialogueControllerTests
    {
        [Test]
        public void TryOpenDialogue_OpensValidDialogueAndPublishesEvent()
        {
            GameObject gameObject = new GameObject();
            DialogueController controller = gameObject.AddComponent<DialogueController>();
            string openedSpeaker = null;
            string openedText = null;
            controller.DialogueOpened += (speaker, text) =>
            {
                openedSpeaker = speaker;
                openedText = text;
            };

            bool opened = controller.TryOpenDialogue("Alex", "Good evening.");

            Assert.That(opened, Is.True);
            Assert.That(controller.IsDialogueOpen, Is.True);
            Assert.That(controller.CurrentSpeakerName, Is.EqualTo("Alex"));
            Assert.That(controller.CurrentDialogueText, Is.EqualTo("Good evening."));
            Assert.That(openedSpeaker, Is.EqualTo("Alex"));
            Assert.That(openedText, Is.EqualTo("Good evening."));

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void TryOpenDialogue_RejectsSecondDialogueWhileDialogueIsOpen()
        {
            GameObject gameObject = new GameObject();
            DialogueController controller = gameObject.AddComponent<DialogueController>();

            controller.TryOpenDialogue("Alex", "First line.");

            Assert.That(controller.TryOpenDialogue("Morgan", "Second line."), Is.False);
            Assert.That(controller.CurrentSpeakerName, Is.EqualTo("Alex"));
            Assert.That(controller.CurrentDialogueText, Is.EqualTo("First line."));

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void CloseDialogue_ClearsStateAndPublishesEventOnce()
        {
            GameObject gameObject = new GameObject();
            DialogueController controller = gameObject.AddComponent<DialogueController>();
            int closeCount = 0;
            controller.DialogueClosed += () => closeCount++;
            controller.TryOpenDialogue("Alex", "Good evening.");

            controller.CloseDialogue();
            controller.CloseDialogue();

            Assert.That(controller.IsDialogueOpen, Is.False);
            Assert.That(controller.CurrentSpeakerName, Is.Null);
            Assert.That(controller.CurrentDialogueText, Is.Null);
            Assert.That(closeCount, Is.EqualTo(1));

            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void Next_AdvancesPagesThenClosesDialogueOnLastPage()
        {
            GameObject gameObject = new GameObject();
            DialogueController controller = gameObject.AddComponent<DialogueController>();
            int pageChangeCount = 0;
            controller.DialoguePageChanged += _ => pageChangeCount++;

            Assert.That(controller.TryOpenDialogue("Alex", new[] { "First.", "Second." }), Is.True);
            Assert.That(controller.PageCount, Is.EqualTo(2));
            Assert.That(controller.CurrentPageIndex, Is.EqualTo(0));
            Assert.That(controller.HasNextPage, Is.True);

            controller.Next();

            Assert.That(controller.CurrentPageIndex, Is.EqualTo(1));
            Assert.That(controller.CurrentDialogueText, Is.EqualTo("Second."));
            Assert.That(controller.HasNextPage, Is.False);
            Assert.That(pageChangeCount, Is.EqualTo(1));

            controller.Next();

            Assert.That(controller.IsDialogueOpen, Is.False);
            Assert.That(controller.PageCount, Is.EqualTo(0));
            Object.DestroyImmediate(gameObject);
        }
    }
}
