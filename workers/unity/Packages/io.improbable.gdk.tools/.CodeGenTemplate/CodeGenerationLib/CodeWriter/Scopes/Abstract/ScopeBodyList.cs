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
        public int ScopeCount => scopeBodies.Count;

        protected ScopeBodyList(string declaration, Action<ScopeBody> populate)
        {
            AddScope(declaration, populate);
        }

        protected ScopeBodyList(string declaration, Func<IEnumerable<string>> populate)
        {
            AddScope(declaration, populate);
        }

        protected void AddScope(string declaration, Action<ScopeBody> populate)
        {
            scopeBodies.Add(new CustomScopeBlock(declaration, populate));
        }

        protected void AddScope(string declaration, Func<IEnumerable<string>> populate)
        {
            scopeBodies.Add(new CustomScopeBlock(declaration, populate));
        }

        public string Format(int indentLevel = 0)
        {
            return string.Join(Environment.NewLine, scopeBodies.Select(cs => cs.Format(indentLevel)));
        }
    }
}
