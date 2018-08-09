using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityReferenceTypeProviderGenerator
    {
        private string qualifiedNamespace;
        private string spatialNamespace;
        private UnityComponentDefinition unityComponentDefinition;
        private HashSet<string> enumSet;

        public string Generate(UnityComponentDefinition unityComponentDefinition, string package,
            HashSet<string> enumSet)
        {
            qualifiedNamespace = UnityTypeMappings.PackagePrefix + package;
            spatialNamespace = package;
            this.unityComponentDefinition = unityComponentDefinition;
            this.enumSet = enumSet;

            return TransformText();
        }

        private UnityComponentDetails GetComponentDetails()
        {
            return new UnityComponentDetails(unityComponentDefinition);
        }

        private List<UnityFieldDetails> GetFieldDetailsList()
        {
            return unityComponentDefinition.DataDefinition.typeDefinition.FieldDefinitions
                .Select(fieldDefinition => new UnityFieldDetails(fieldDefinition.RawFieldDefinition, fieldDefinition.IsBlittable, enumSet)).ToList();
        }
    }
}
