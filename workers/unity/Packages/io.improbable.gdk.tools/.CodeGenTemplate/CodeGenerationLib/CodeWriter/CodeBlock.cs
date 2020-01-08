using System;
using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    public abstract class CodeBlock : ICodeBlock
    {
        protected static readonly string DefaultContentSeparator = $"{Environment.NewLine}{Environment.NewLine}";

        private readonly List<ICodeBlock> content = new List<ICodeBlock>();
        protected IEnumerable<ICodeBlock> Content => content;

        public abstract string Output(int indentLevel);

        protected void Add(ICodeBlock block)
        {
            content.Add(block);
        }

        protected void Add(IEnumerable<ICodeBlock> blocks)
        {
            content.AddRange(blocks);
        }
    }
}
