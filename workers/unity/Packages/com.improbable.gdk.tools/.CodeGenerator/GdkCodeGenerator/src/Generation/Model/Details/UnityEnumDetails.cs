using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityEnumDetails
    {
        public string TypeName => Formatting.SnakeCaseToCapitalisedCamelCase(enumDefinition.name);
        public string FqnTypeName => CommonDetailsUtils.GetCapitalisedFqnTypename(enumDefinition.qualifiedName);

        private readonly EnumDefinitionRaw enumDefinition;
        
        public UnityEnumDetails(EnumDefinitionRaw enumDefinition)
        {
            this.enumDefinition = enumDefinition;
        }
    }
}
