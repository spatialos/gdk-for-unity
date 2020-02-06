using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public abstract class Details
    {
        public readonly string Name;
        public readonly string PascalCaseName;
        public readonly string CamelCaseName;

        protected Details(Definition definition)
        {
            Name = definition.Name;
            PascalCaseName = Formatting.SnakeCaseToPascalCase(definition.Name);
            CamelCaseName = Formatting.PascalCaseToCamelCase(PascalCaseName);
        }
    }
}
