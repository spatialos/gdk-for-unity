using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeGenerationLib.CodeWriter.Scopes;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public class NamespaceBlock : ScopeBlock
    {
        internal NamespaceBlock(string declaration, Action<NamespaceBlock> populate) : base($"namespace {declaration}")
        {
            populate(this);
        }

        public AnnotationOutsideType Annotate(string annotation)
        {
            return new AnnotationOutsideType(this, annotation);
        }

        public void Line(string snippet)
        {
            Add(new Text(snippet));
        }

        public void Line(Action<StringBuilder> populate)
        {
            var sb = new StringBuilder();
            populate(sb);

            Add(new Text(sb.ToString()));
        }

        public void Line(ICollection<string> snippets)
        {
            if (snippets.Any())
            {
                Add(new TextList(snippets));
            }
        }

        public void CustomScope(Action<CustomScopeBlock> populate)
        {
            Add(new CustomScopeBlock(populate));
        }

        public void CustomScope(string declaration, Action<CustomScopeBlock> populate)
        {
            Add(new CustomScopeBlock(declaration, populate));
        }

        public void Enum(EnumBlock enumBlock)
        {
            Add(enumBlock);
        }

        public void Enum(string declaration, Action<EnumBlock> populate)
        {
            Add(new EnumBlock(declaration, populate));
        }

        public void Type(TypeBlock typeBlock)
        {
            Add(typeBlock);
        }

        public void Type(string declaration, Action<TypeBlock> populate)
        {
            Add(new TypeBlock(declaration, populate));
        }
    }
}
