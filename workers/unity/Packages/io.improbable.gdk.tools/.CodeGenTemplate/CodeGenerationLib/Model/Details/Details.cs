using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public abstract class Details
    {
        public readonly string Name;
        public readonly string CamelCaseName;

        protected Details(string name)
        {
            Name = name;
            CamelCaseName = Formatting.PascalCaseToCamelCase(CamelCaseName);
        }
    }
}
