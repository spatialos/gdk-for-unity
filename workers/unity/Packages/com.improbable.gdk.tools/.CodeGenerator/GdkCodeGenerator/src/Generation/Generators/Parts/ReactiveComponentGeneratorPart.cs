using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     This class contains the data required to fill out the "UnityComponentConversionGenerator.tt" templates.
    ///     This template generates the ComponentTranslation implementation for Components.
    /// </summary>
    public partial class ReactiveComponentGenerator
    {
        private string qualifiedNamespace;
        private string spatialNamespace;
        private UnityComponentDefinition unityComponentDefinition;
        private HashSet<string> enumSet;

        public string Generate(UnityComponentDefinition unityComponentDefinition, string package,
            HashSet<string> enumSet)
        {
            qualifiedNamespace = package;
            spatialNamespace = package;
            this.unityComponentDefinition = unityComponentDefinition;
            this.enumSet = enumSet;

            return TransformText();
        }

        private UnityComponentDetails GetComponentDetails()
        {
            return new UnityComponentDetails(unityComponentDefinition);
        }

        private List<UnityEventDetails> GetEventDetailsList()
        {
            return unityComponentDefinition.EventDefinitions
                .Select(eventDefinition => new UnityEventDetails(eventDefinition)).ToList();
        }
    }
}
