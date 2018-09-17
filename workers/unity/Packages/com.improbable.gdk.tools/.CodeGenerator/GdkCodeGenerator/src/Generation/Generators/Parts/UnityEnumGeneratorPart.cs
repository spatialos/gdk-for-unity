using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumGenerator
    {
        private string qualifiedNamespace;
        private EnumDefinitionRaw enumDefinition;

        public string Generate(EnumDefinitionRaw enumDefinition, string package)
        {
            qualifiedNamespace = package;
            this.enumDefinition = enumDefinition;

            return TransformText();
        }

        public UnityEnumDetails GetEnumDetails()
        {
            return new UnityEnumDetails(enumDefinition);
        }
    }
}
