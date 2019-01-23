using System.Collections.Generic;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     This class contains the data required to fill out the "UnityComponentSenderGenerator.tt" templates.
    ///     This template generates the component groups for creating component updates from ecs components.
    /// </summary>
    public partial class UnityComponentSenderGenerator
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
    }
}
