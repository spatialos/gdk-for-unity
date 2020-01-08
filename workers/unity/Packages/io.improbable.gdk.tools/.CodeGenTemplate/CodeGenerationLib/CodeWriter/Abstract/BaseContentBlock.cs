using System;
using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    public abstract class BaseContentBlock : IBlock
    {
        protected static readonly string DefaultContentSeparator = $"{Environment.NewLine}{Environment.NewLine}";

        private readonly List<IBlock> content = new List<IBlock>();
        protected IEnumerable<IBlock> Content => content;

        public abstract string Output(int indentLevel);

        protected void Add(IBlock block)
        {
            content.Add(block);
        }

        protected void Add(IEnumerable<IBlock> blocks)
        {
            content.AddRange(blocks);
        }
    }
}
