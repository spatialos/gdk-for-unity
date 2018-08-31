using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEnumContent
    {
        private EnumDefinitionRaw enumDefinition;

        public string Generate(EnumDefinitionRaw enumDefinition)
        {
            this.enumDefinition = enumDefinition;
            return TransformText();
        }

        public UnityEnumDetails GetEnumDetails()
        {
            return new UnityEnumDetails(enumDefinition);
        }
    }
}
