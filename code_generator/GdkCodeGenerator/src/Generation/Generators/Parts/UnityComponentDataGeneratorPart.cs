using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityComponentDataGenerator
    {
        private string qualifiedNamespace;
        private UnityComponentDefinition unityComponentDefinition;
        private HashSet<string> enumSet;

        public string Generate(UnityComponentDefinition unityComponentDefinition, string package, HashSet<string> e)
        {
            qualifiedNamespace = UnityTypeMappings.PackagePrefix + package;
            this.unityComponentDefinition = unityComponentDefinition;
            this.enumSet = e;

            return TransformText();
        }

        private UnityComponentDetails GetComponentDetails()
        {
            return new UnityComponentDetails(unityComponentDefinition);
        }

        private List<UnityFieldDetails> GetFieldDetailsList()
        {
            return unityComponentDefinition.DataDefinition.typeDefinition.FieldDefinitions
                .Select(fieldDefinition => new UnityFieldDetails(fieldDefinition.RawFieldDefinition, enumSet)).ToList();
        }
    }
}
