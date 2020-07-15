using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class MethodBlock : ScopeBody
    {
        internal MethodBlock(string declaration, Action<MethodBlock> populate, List<string> annotation = null) : base(declaration)
        {
            Annotations = annotation;
            populate(this);
        }

        internal MethodBlock(string declaration, Func<IEnumerable<string>> populate, List<string> annotation = null) : base(
            declaration)
        {
            Annotations = annotation;

            var methodBody = populate().ToList();
            if (methodBody.Count > 0)
            {
                Add(new TextList(methodBody));
            }
        }
    }
}
