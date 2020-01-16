using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class MethodBlock : ScopeBody
    {
        internal MethodBlock(string declaration, Action<MethodBlock> populate, string annotation = "") : base(declaration)
        {
            Annotation = annotation;
            populate(this);
        }

        internal MethodBlock(string declaration, Func<IEnumerable<string>> populate, string annotation = "") : base(
            declaration)
        {
            var methodBody = populate().ToList();
            if (methodBody.Count > 0)
            {
                Add(new TextList(methodBody));
            }
        }
    }
}
