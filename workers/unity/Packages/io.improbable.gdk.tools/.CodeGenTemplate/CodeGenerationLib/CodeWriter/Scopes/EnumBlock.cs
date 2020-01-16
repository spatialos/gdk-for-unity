using System;
using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class EnumBlock : ScopeBlock, IAnnotatable
    {
        private static readonly string EnumMemberSeparator = $",{Environment.NewLine}";

        internal EnumBlock(string declaration, Action<EnumBlock> populate) : base(declaration)
        {
            populate(this);
        }

        public void Member(string snippet)
        {
            Add(new Text(snippet));
        }

        public void Members(ICollection<string> snippets)
        {
            Add(new TextList(EnumMemberSeparator, snippets));
        }

        public void Annotate(string annotation)
        {
            Annotation = annotation;
        }

        public override string Format(int indentLevel = 0)
        {
            return base.Format(indentLevel, EnumMemberSeparator);
        }
    }
}
