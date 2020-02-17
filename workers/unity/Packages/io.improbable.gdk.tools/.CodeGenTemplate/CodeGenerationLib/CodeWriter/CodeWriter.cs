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

        private readonly string rawContent;

        private CodeWriter(Action<CodeWriter> populate)
        {
            populate(this);
        }

        private CodeWriter(string rawContent)
        {
            this.rawContent = rawContent;
        }

        public static CodeWriter Populate(Action<CodeWriter> populate)
        {
            return new CodeWriter(populate);
        }

        public static CodeWriter Raw(string raw)
        {
            return new CodeWriter(raw);
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
            if (rawContent != null)
            {
                return rawContent;
            }

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
            return $"{Header}{Environment.NewLine}{fmtUsingDirectives}{Environment.NewLine}{formattedContent}{Environment.NewLine}";
        }
    }
}
