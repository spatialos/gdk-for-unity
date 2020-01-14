using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    public class CodeWriter : ICodeBlock
    {
        private static readonly string Header = CommonGeneratorUtils.GetGeneratedHeader();
        private static readonly string ContentSeparator = $"{Environment.NewLine}{Environment.NewLine}";

        private readonly List<string> usingDirectives = new List<string>();
        private readonly List<ICodeBlock> content = new List<ICodeBlock>();

        private CodeWriter(Action<CodeWriter> populate)
        {
            populate(this);
        }

        public static CodeWriter Populate(Action<CodeWriter> populate)
        {
            return new CodeWriter(populate);
        }

        public void Line(string snippet)
        {
            content.Add(new Text(snippet));
        }

        public void Line(Action<StringBuilder> populate)
        {
            var sb = new StringBuilder();
            populate(sb);

            content.Add(new Text(sb.ToString()));
        }

        public void Line(ICollection<string> snippets)
        {
            if (snippets.Any())
            {
                content.Add(new TextList(snippets));
            }
        }

        public void CustomScope(Action<CustomScopeBlock> populate)
        {
            content.Add(new CustomScopeBlock(populate));
        }

        public void CustomScope(string declaration, Action<CustomScopeBlock> populate)
        {
            content.Add(new CustomScopeBlock(declaration, populate));
        }

        public void Method(string declaration, Action<MethodBlock> populate)
        {
            content.Add(new MethodBlock(declaration, populate));
        }

        public void Enum(string declaration, Action<EnumBlock> populate)
        {
            content.Add(new EnumBlock(declaration, populate));
        }

        public void Type(string declaration, Action<TypeBlock> populate)
        {
            content.Add(new TypeBlock(declaration, populate));
        }

        public void Namespace(string declaration, Action<NamespaceBlock> populate)
        {
            content.Add(new NamespaceBlock(declaration, populate));
        }

        public void UsingDirectives(params string[] directives)
        {
            usingDirectives.AddRange(directives);
        }

        // ReSharper disable once OptionalParameterHierarchyMismatch
        public string Format(int indentLevel = 0)
        {
            var fmtUsingDirectives = string.Empty;
            if (usingDirectives.Count > 0)
            {
                var sb = new StringBuilder(Environment.NewLine);
                foreach (var directive in usingDirectives)
                {
                    sb.AppendLine($"using {directive};");
                }

                fmtUsingDirectives = sb.ToString();
            }

            var formattedContent = string.Join(ContentSeparator, content.Select(block => block.Format(0)));
            return $@"{Header}{Environment.NewLine}{fmtUsingDirectives}{Environment.NewLine}{formattedContent}";
        }
    }
}
