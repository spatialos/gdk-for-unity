using System;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class LoopBlock : ScopeBody
    {
        internal LoopBlock(string declaration, Action<LoopBlock> populate) : base(declaration)
        {
            populate(this);
        }
    }
}
