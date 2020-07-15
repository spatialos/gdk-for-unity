using System;
using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class EnumBlock : ScopeBlock
    {
        private static readonly string EnumMemberSeparator = $",{Environment.NewLine}";

        internal EnumBlock(string declaration, Action<EnumBlock> populate, List<string> annotation = null) : base(declaration)
        {
            Annotations = annotation;
            populate(this);
        }

        internal EnumBlock(string declaration, Func<IEnumerable<string>> populate, List<string> annotation = null) : base(declaration)
        {
            Annotations = annotation;
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
