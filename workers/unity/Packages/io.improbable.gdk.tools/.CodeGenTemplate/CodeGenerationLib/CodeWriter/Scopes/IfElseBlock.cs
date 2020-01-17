using System;
using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    /// <summary>
    /// A code unit representing an if/elseif/else construct.
    /// </summary>
    public class IfElseBlock : ScopeBodyList
    {
        internal IfElseBlock(string declaration, Action<ScopeBody> populate) : base($"if ({declaration})", populate)
        {
        }

        internal IfElseBlock(string declaration, Func<IEnumerable<string>> populate) : base($"if ({declaration})", populate)
        {
        }

        public IfElseBlock ElseIf(string declaration, Action<ScopeBody> populate)
        {
            AddScope($"else if ({declaration})", populate);
            return this;
        }

        public IfElseBlock ElseIf(string declaration, Func<IEnumerable<string>> populate)
        {
            AddScope($"else if ({declaration})", populate);
            return this;
        }

        public void Else(Action<ScopeBody> populate)
        {
            AddScope("else", populate);
        }

        public void Else(Func<IEnumerable<string>> populate)
        {
            AddScope("else", populate);
        }
    }
}
