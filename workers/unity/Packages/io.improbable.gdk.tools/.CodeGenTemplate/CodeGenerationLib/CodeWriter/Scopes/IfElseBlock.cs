using System;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class IfElseBlock : ScopeBodyList
    {
        internal IfElseBlock(string declaration, Action<ScopeBody> populate) : base($"if ({declaration})", populate)
        {
        }

        public IfElseBlock ElseIf(string declaration, Action<ScopeBody> populate)
        {
            AddScope($"else if ({declaration})", populate);
            return this;
        }

        public void Else(Action<ScopeBody> populate)
        {
            AddScope("else", populate);
        }
    }
}
