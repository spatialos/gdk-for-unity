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
        private UnityComponentDetails details;

        public string Generate(UnityComponentDetails details, string package)
        {
            qualifiedNamespace = package;
            this.details = details;

            return TransformText();
        }

        private UnityComponentDetails GetComponentDetails()
        {
            return details;
        }
    }
}
