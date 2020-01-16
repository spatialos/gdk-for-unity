using System;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class MethodBlock : ScopeBody, IAnnotatable
    {
        internal MethodBlock(string declaration, Action<MethodBlock> populate) : base(declaration)
        {
            populate(this);
        }

        public void Annotate(string annotation)
        {
            Annotation = annotation;
        }
    }
}
