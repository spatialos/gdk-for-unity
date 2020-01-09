using System;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    /// <summary>
    /// Free-form text to write.
    /// </summary>
    internal class Text : ICodeBlock
    {
        private readonly string snippet;

        internal Text(string snippet, bool trim = true)
        {
            this.snippet = trim ? snippet.Trim() : snippet;
        }

        public bool HasValue()
        {
            return !string.IsNullOrEmpty(snippet);
        }

        public string Output(int indentLevel)
        {
            var spaces = new string(' ', indentLevel * CommonGeneratorUtils.SpacesPerIndent);
            var indentedCode = snippet
                .Replace($"{Environment.NewLine}", $"{Environment.NewLine}{spaces}")
                .Replace($"{Environment.NewLine}{spaces}{Environment.NewLine}", $"{Environment.NewLine}{Environment.NewLine}");
            return $"{spaces}{indentedCode}";
        }
    }
}
