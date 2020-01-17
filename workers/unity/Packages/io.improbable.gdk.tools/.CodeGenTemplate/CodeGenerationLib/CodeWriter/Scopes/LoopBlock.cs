using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    /// <summary>
    /// Scope with mandatory declaration for defining loops.
    /// </summary>
    public class LoopBlock : ScopeBody
    {
        internal LoopBlock(string declaration, Action<LoopBlock> populate) : base(declaration)
        {
            populate(this);
        }

        internal LoopBlock(string declaration, Func<IEnumerable<string>> populate) : base(declaration)
        {
            Line(populate().ToList());
        }

        public void Continue()
        {
            Line("continue;");
        }

        public void Break()
        {
            Line("break;");
        }
    }
}
