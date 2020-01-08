using System;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.NoScope
{
    internal class TextBlock : IBlock
    {
        private readonly string snippet;

        internal TextBlock(string snippet)
        {
            this.snippet = snippet.Trim();
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
