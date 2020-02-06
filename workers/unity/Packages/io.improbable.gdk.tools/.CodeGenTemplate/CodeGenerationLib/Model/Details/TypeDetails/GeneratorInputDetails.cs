using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public abstract class GeneratorInputDetails : Details
    {
        public readonly string Namespace;
        public readonly string NamespacePath;

        public readonly string QualifiedName;
        public readonly string FullyQualifiedName;

        protected GeneratorInputDetails(string package, QualifiedDefinition qualifiedDefinition) : base(qualifiedDefinition)
        {
            Namespace = Formatting.CapitaliseQualifiedNameParts(package);
            NamespacePath = Formatting.GetNamespacePath(package);

            QualifiedName = qualifiedDefinition.QualifiedName;
            FullyQualifiedName = DetailsUtils.GetCapitalisedFqnTypename(qualifiedDefinition.QualifiedName);
        }
    }
}
