using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.NoScope
{
    internal class TextList : IBlock
    {
        private readonly string snippetListSeparator;

        private readonly List<TextBlock> textBlocks = new List<TextBlock>();
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
            textBlocks.AddRange(snippets.Select(s => new TextBlock(s)));
        }

        public string Output(int indentLevel = 0)
        {
            return string.Join(snippetListSeparator, textBlocks.Select(snippet => snippet.Output(indentLevel)));
        }
    }
}
