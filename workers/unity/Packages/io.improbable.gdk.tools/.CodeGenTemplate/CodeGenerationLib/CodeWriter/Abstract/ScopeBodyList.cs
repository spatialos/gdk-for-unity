using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public abstract class ScopeBodyList : IBlock
    {
        private readonly List<BaseScopeBody> scopeBodies = new List<BaseScopeBody>();

        protected ScopeBodyList(string declaration, Action<BaseScopeBody> populate)
        {
            AddScope(declaration, populate);
        }

        protected void AddScope(string declaration, Action<BaseScopeBody> populate)
        {
            scopeBodies.Add(new CustomScopeBlock(declaration, populate));
        }

        public string Output(int indentLevel = 0)
        {
            return string.Join(Environment.NewLine, scopeBodies.Select(cs => cs.Output(indentLevel)));
        }
    }
}
