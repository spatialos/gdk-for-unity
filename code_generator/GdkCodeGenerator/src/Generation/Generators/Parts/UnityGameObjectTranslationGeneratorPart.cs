using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     This class contains the data required to fill out the "UnityGameObjectTranslationGenerator.tt" templates.
    ///     This template generates the GameObjectTranslation implementation for Components.
    /// </summary>
    public partial class UnityGameObjectTranslationGenerator
    {
        private string qualifiedNamespace;
        private UnityComponentDefinition unityComponentDefinition;

        public string Generate(UnityComponentDefinition unityComponentDefinition, string package)
        {
            qualifiedNamespace = UnityTypeMappings.PackagePrefix + package;
            this.unityComponentDefinition = unityComponentDefinition;

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

        private List<UnityEventDetails> GetEventDetailsList()
        {
            return unityComponentDefinition.EventDefinitions
                .Select(eventDefinition => new UnityEventDetails(eventDefinition)).ToList();
        }

        private List<UnityCommandDetails> GetCommandDetailsList()
        {
            return unityComponentDefinition.CommandDefinitions
                .Select(commandDefinition => new UnityCommandDetails(commandDefinition)).ToList();
        }
    }
}
