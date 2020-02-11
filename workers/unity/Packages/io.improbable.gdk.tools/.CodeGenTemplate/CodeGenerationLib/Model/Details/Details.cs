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

        protected Details(Definition definition)
        {
            Name = definition.Name;
            PascalCaseName = Formatting.SnakeCaseToPascalCase(definition.Name);
            CamelCaseName = Formatting.PascalCaseToCamelCase(PascalCaseName);
        }
    }
}
