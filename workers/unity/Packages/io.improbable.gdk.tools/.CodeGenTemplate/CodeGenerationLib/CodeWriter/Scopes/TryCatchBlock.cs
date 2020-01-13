using System;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    /// <summary>
    /// A code unit representing a try/catch/finally construct.
    /// </summary>
    public class TryCatchBlock : ScopeBodyList
    {
        internal TryCatchBlock(Action<ScopeBody> populate) : base("try", populate)
        {
        }

        public TryCatchBlock Catch(string declaration, Action<ScopeBody> populate)
        {
            AddScope($"catch ({declaration})", populate);
            return this;
        }

        public void Finally(Action<ScopeBody> populate)
        {
            AddScope("finally", populate);
        }

        public new string Format(int indentLevel = 0)
        {
            // Ensure that there is at least one Catch/Finally scope after the initial Try scope.
            if (ScopeCount < 2)
            {
                throw new InvalidOperationException("Cannot format TryCatchBlock without Catch or Finally scopes.");
            }

            return base.Format();
        }
    }
}
