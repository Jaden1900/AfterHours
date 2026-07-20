using System;

namespace AfterHours.UI
{
    public static class InteractionPromptFormatter
    {
        public static string Format(string format, string bindingDisplayName, string prompt)
        {
            return string.Format(string.IsNullOrWhiteSpace(format) ? "{1}" : format,
                bindingDisplayName ?? string.Empty, prompt ?? string.Empty);
        }
    }
}
