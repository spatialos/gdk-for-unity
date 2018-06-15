using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     This class contains the data required to fill out the "UnityComponentDataConversionGenerator.tt" templates.
    ///     This template generates the ComponentTranslation implementation for IComponentData.
    /// </summary>
    public partial class UnityComponentDataConversionGenerator
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
                .Select(fieldDefinition => new UnityFieldDetails(fieldDefinition.RawFieldDefinition)).ToList();
        }

        private List<UnityCommandDetails> GetCommandDetailsList()
        {
            return unityComponentDefinition.CommandDefinitions
                .Select(commandDefinition => new UnityCommandDetails(commandDefinition)).ToList();
        }

        private List<UnityEventDetails> GetEventDetailsList()
        {
            return unityComponentDefinition.EventDefinitions
                .Select(eventDefinition => new UnityEventDetails(eventDefinition)).ToList();
        }
    }
}
