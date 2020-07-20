using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class MethodBlock : ScopeBody
    {
        internal MethodBlock(string declaration, Action<MethodBlock> populate, IEnumerable<string> annotations = null) : base(declaration)
        {
            Annotations = annotations;
            populate(this);
        }

        internal MethodBlock(string declaration, Func<IEnumerable<string>> populate, IEnumerable<string> annotations = null) : base(
            declaration)
        {
            Annotations = annotations;

            var methodBody = populate().ToList();
            if (methodBody.Count > 0)
            {
                Add(new TextList(methodBody));
            }
        }
    }
}
