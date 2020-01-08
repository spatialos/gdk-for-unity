using System;
using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter.NoScope;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.CodeWriter.Scopes
{
    public abstract class BaseScopeBlock : BaseContentBlock
    {
        protected string Annotation;
        private readonly TextBlock declaration;

        protected BaseScopeBlock(string declaration)
        {
            this.declaration = new TextBlock(declaration);
        }

        // ReSharper disable once OptionalParameterHierarchyMismatch
        public override string Output(int indentLevel = 0)
        {
            return Output(indentLevel, DefaultContentSeparator);
        }

        protected virtual string Output(int indentLevel, string contentSeparator)
        {
            var indent = new string(' ', indentLevel * CommonGeneratorUtils.SpacesPerIndent);

            var scopeAnnotation = string.Empty;
            if (!string.IsNullOrEmpty(Annotation))
            {
                scopeAnnotation = $"{indent}[{Annotation}]{Environment.NewLine}";
            }

            var scopeDeclaration = "";
            if (declaration.HasValue())
            {
                scopeDeclaration = $"{declaration.Output(indentLevel)}{Environment.NewLine}";
            }

            var scopeOutput = string.Empty;
            if (Content.Any())
            {
                var indentedContents = Content.Select(scopeContent => scopeContent.Output(indentLevel + 1));
                scopeOutput = $"{string.Join(contentSeparator, indentedContents)}{Environment.NewLine}";
            }

            return $"{scopeAnnotation}{scopeDeclaration}{indent}{{{Environment.NewLine}{scopeOutput}{indent}}}";
        }
    }
}
