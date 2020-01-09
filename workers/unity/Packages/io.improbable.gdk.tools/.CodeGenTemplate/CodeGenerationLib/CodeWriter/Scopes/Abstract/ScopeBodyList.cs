using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    /// <summary>
    /// A set of scoped units of code.
    /// </summary>
    public abstract class ScopeBodyList : ICodeBlock
    {
        private readonly List<ScopeBody> scopeBodies = new List<ScopeBody>();

        protected ScopeBodyList(string declaration, Action<ScopeBody> populate)
        {
            AddScope(declaration, populate);
        }

        protected void AddScope(string declaration, Action<ScopeBody> populate)
        {
            scopeBodies.Add(new CustomScopeBlock(declaration, populate));
        }

        public string Output(int indentLevel = 0)
        {
            return string.Join(Environment.NewLine, scopeBodies.Select(cs => cs.Output(indentLevel)));
        }
    }
}
