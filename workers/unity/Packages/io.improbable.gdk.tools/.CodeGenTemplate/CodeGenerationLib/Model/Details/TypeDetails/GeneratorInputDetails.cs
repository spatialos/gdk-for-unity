using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public abstract class GeneratorInputDetails : Details
    {
        /// <summary>
        /// The namespace for the code generated from the given details.
        /// </summary>
        public readonly string Namespace;

        /// <summary>
        /// The target path for code generated from the given details.
        /// </summary>
        /// <remarks>
        /// The path is relative to the target directory defined of the codegen job that calls generators
        /// with these details.
        /// </remarks>
        public readonly string NamespacePath;

        /// <summary>
        /// The qualified name as defined by the the schema bundle.
        /// </summary>
        public readonly string QualifiedName;

        /// <summary>
        /// A properly formatted variant of the QualifiedName.
        /// </summary>
        /// <remarks>
        /// Includes the "global::" prefix.
        /// </remarks>
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
