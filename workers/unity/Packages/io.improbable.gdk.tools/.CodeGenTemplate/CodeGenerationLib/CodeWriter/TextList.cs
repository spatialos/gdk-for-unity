using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    internal class TextList : ICodeBlock
    {
        private readonly string snippetListSeparator;

        private readonly List<Text> textBlocks = new List<Text>();
        public int Count => textBlocks.Count;

        internal TextList(IEnumerable<string> snippets)
        {
            snippetListSeparator = Environment.NewLine;
            Add(snippets);
        }

        internal TextList(string separator, IEnumerable<string> snippets)
        {
            snippetListSeparator = separator;
            Add(snippets);
        }

        private void Add(IEnumerable<string> snippets)
        {
            textBlocks.AddRange(snippets.Select(s => new Text(s)));
        }

        public string Output(int indentLevel = 0)
        {
            return string.Join(snippetListSeparator, textBlocks.Select(snippet => snippet.Output(indentLevel)));
        }
    }
}
