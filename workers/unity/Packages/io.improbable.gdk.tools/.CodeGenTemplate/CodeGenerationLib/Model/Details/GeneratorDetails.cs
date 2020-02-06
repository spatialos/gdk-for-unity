using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public abstract class GeneratorDetails
    {
        public readonly string Name;
        public readonly string Namespace;
        public readonly string NamespacePath;

        protected GeneratorDetails(string rawName, string package)
        {
            Name = rawName;
            Namespace = Formatting.CapitaliseQualifiedNameParts(package);
            NamespacePath = Formatting.GetNamespacePath(package);
        }
    }
}
