using System;
using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    /// <summary>
    /// A code unit to write initializers.
    /// </summary>
    internal class InitializerBlock : ICodeBlock
    {
        private readonly string declaration;
        private readonly TextList initialValues;

        internal InitializerBlock(string declaration, Func<IEnumerable<string>> populate)
        {
            this.declaration = declaration.Trim();
            initialValues = new TextList($@",{Environment.NewLine}", populate());
        }

        public string Format(int indentLevel = 0)
        {
            var indent = CommonGeneratorUtils.GetIndentSpaces(indentLevel);

            var values = string.Empty;
            if (initialValues.Count > 0)
            {
                values = $"{initialValues.Format(indentLevel + 1)}{Environment.NewLine}";
            }

            return $"{indent}{declaration}{Environment.NewLine}{indent}{{{Environment.NewLine}{values}{indent}}};";
        }
    }
}
