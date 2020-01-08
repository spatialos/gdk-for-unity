using System;
using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    internal class InitializerCodeBlock : ICodeBlock
    {
        private readonly string declaration;
        private readonly TextList initialValues;

        internal InitializerCodeBlock(string declaration, Func<IEnumerable<string>> populate)
        {
            this.declaration = declaration.Trim();
            initialValues = new TextList($@",{Environment.NewLine}", populate());
        }

        public string Output(int indentLevel = 0)
        {
            var indent = new string(' ', indentLevel * CommonGeneratorUtils.SpacesPerIndent);

            var output = "";
            if (initialValues.Count > 0)
            {
                output = $"{initialValues.Output(indentLevel + 1)}{Environment.NewLine}";
            }

            return
                $@"{indent}{declaration}
{indent}{{
{output}{indent}}};";
        }
    }
}
