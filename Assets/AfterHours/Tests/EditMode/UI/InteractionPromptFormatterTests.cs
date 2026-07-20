using AfterHours.UI;
using NUnit.Framework;

namespace AfterHours.Tests.EditMode.UI
{
    public sealed class InteractionPromptFormatterTests
    {
        [Test]
        public void Format_UsesProvidedBindingAndPrompt()
        {
            Assert.That(InteractionPromptFormatter.Format("[{0}] {1}", "E", "Talk to Alex"),
                Is.EqualTo("[E] Talk to Alex"));
        }
    }
}
