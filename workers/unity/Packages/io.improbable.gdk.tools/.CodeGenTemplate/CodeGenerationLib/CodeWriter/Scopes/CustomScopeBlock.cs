using System;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class CustomScopeBlock : BaseScopeBody
    {
        internal CustomScopeBlock(Action<CustomScopeBlock> populate) : base(string.Empty)
        {
            populate(this);
        }

        internal CustomScopeBlock(string declaration, Action<CustomScopeBlock> populate) : base(declaration)
        {
            populate(this);
        }
    }
}
