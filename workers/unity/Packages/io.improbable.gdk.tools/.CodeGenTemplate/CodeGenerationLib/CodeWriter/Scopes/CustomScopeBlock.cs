using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    /// <summary>
    /// A generic scope with optional declaration.
    /// </summary>
    public class CustomScopeBlock : ScopeBody
    {
        internal CustomScopeBlock(Action<CustomScopeBlock> populate) : base(string.Empty)
        {
            populate(this);
        }

        internal CustomScopeBlock(Func<IEnumerable<string>> populate) : base(string.Empty)
        {
            Line(populate().ToList());
        }

        internal CustomScopeBlock(string declaration, Action<CustomScopeBlock> populate) : base(declaration)
        {
            populate(this);
        }

        internal CustomScopeBlock(string declaration, Func<IEnumerable<string>> populate) : base(declaration)
        {
            Line(populate().ToList());
        }
    }
}
