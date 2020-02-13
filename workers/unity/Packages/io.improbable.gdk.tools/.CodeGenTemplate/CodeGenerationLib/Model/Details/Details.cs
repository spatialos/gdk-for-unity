using System;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public abstract class Details
    {
        /// <summary>
        /// The raw name as defined in the schema bundle.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The raw name converted to PascalCase.
        /// </summary>
        public readonly string PascalCaseName;

        /// <summary>
        /// The PascalCase name converted to camelCase.
        /// </summary>
        public readonly string CamelCaseName;

        protected Details(Definition definition, Case defaultCase = Case.Raw)
        {
            PascalCaseName = Formatting.SnakeCaseToPascalCase(definition.Name);
            CamelCaseName = Formatting.PascalCaseToCamelCase(PascalCaseName);

            switch (defaultCase)
            {
                case Case.Raw:
                    Name = definition.Name;
                    break;
                case Case.PascalCase:
                    Name = PascalCaseName;
                    break;
                case Case.CamelCase:
                    Name = CamelCaseName;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(defaultCase), defaultCase, null);
            }
        }

        protected enum Case
        {
            Raw,
            PascalCase,
            CamelCase
        }
    }
}
