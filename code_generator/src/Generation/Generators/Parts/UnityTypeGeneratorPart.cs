using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityTypeGenerator
    {
        private string qualifiedNamespace;
        private UnityTypeDefinition typeDefinition;
        private HashSet<string> enumSet;

        public string Generate(UnityTypeDefinition typeDefinition, string package, HashSet<string> enumSet)
        {
            qualifiedNamespace = UnityTypeMappings.PackagePrefix + package;
            this.typeDefinition = typeDefinition;
            this.enumSet = enumSet;

            return TransformText();
        }

        private UnityTypeDetails GetTypeDetails()
        {
            return new UnityTypeDetails(typeDefinition);
        }

        private List<UnityFieldDetails> GetFieldDetailsList()
        {
            return typeDefinition.FieldDefinitions
                .Select(fieldDefinition => new UnityFieldDetails(fieldDefinition.RawFieldDefinition)).ToList();
        }
    }
}
