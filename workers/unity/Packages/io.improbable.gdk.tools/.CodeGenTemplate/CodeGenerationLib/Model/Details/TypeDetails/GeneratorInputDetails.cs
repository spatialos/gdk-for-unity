using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public abstract class GeneratorInputDetails : Details
    {
        public readonly string Namespace;
        public readonly string NamespacePath;

        public readonly string QualifiedName;
        public readonly string FullyQualifiedName;

        protected GeneratorInputDetails(BaseTypeDetails baseTypeDetails, string package) : base(baseTypeDetails.Name)
        {
            Namespace = Formatting.CapitaliseQualifiedNameParts(package);
            NamespacePath = Formatting.GetNamespacePath(package);

            QualifiedName = baseTypeDetails.QualifiedName;
            FullyQualifiedName = Formatting.CapitaliseQualifiedNameParts(baseTypeDetails.QualifiedName);
        }
    }
}
