using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class EnumBlock : ScopeBlock
    {
        private static readonly string EnumMemberSeparator = $",{Environment.NewLine}";

        internal EnumBlock(string declaration, Action<EnumBlock> populate, IEnumerable<string> annotations = null) : base(declaration)
        {
            Annotations = annotations;
            populate(this);
        }

        internal EnumBlock(string declaration, Func<IEnumerable<string>> populate, IEnumerable<string> annotations = null) : base(declaration)
        {
            Annotations = annotations;
            foreach (var member in populate())
            {
                Member(member);
            }
        }

        public void Member(string snippet)
        {
            Add(new Text(snippet));
        }

        public void Members(ICollection<string> snippets)
        {
            Add(new TextList(EnumMemberSeparator, snippets));
        }

        public override string Format(int indentLevel = 0)
        {
            return base.Format(indentLevel, EnumMemberSeparator);
        }
    }
}
