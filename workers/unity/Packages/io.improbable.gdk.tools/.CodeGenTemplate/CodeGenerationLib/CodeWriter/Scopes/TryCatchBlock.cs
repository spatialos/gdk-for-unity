using System;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class TryCatchBlock : ScopeBodyList
    {
        internal TryCatchBlock(Action<BaseScopeBody> populate) : base("try", populate)
        {
        }

        public TryCatchBlock Catch(string declaration, Action<BaseScopeBody> populate)
        {
            AddScope($"catch ({declaration})", populate);
            return this;
        }

        public void Finally(Action<BaseScopeBody> populate)
        {
            AddScope("finally", populate);
        }
    }
}
