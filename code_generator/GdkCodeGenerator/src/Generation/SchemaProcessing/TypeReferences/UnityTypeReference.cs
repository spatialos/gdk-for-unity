using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     Represents a type in the schema, and contains a reference to it.
    /// </summary>
    public class UnityTypeReference
    {
        public readonly string builtInType;
        public readonly EnumDefinitionRaw enumDefinition;
        public readonly UnityTypeDefinition typeDefinition;

        internal UnityTypeReference(string builtInType, EnumDefinitionRaw enumDefinition,
            UnityTypeDefinition typeDefinition)
        {
            this.builtInType = builtInType;
            this.enumDefinition = enumDefinition;
            this.typeDefinition = typeDefinition;
        }
    }
}
