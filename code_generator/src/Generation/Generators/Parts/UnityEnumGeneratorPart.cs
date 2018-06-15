using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumGenerator
    {
        private string qualifiedNamespace;
        private EnumDefinitionRaw enumDefinition;

        public string Generate(EnumDefinitionRaw enumDefinition, string package)
        {
            qualifiedNamespace = UnityTypeMappings.PackagePrefix + package;
            this.enumDefinition = enumDefinition;

            return TransformText();
        }

        public UnityEnumDetails GetEnumDetails()
        {
            return new UnityEnumDetails(enumDefinition);
        }
    }
}
