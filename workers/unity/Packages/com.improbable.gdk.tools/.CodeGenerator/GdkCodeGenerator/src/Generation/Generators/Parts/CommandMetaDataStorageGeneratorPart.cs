using System.Collections.Generic;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class CommandMetaDataStorageGenerator
    {
        private string qualifiedNamespace;
        private string spatialNamespace;
        private UnityComponentDetails componentDetails;

        public string Generate(UnityComponentDetails componentDetails, string package)
        {
            qualifiedNamespace = package;
            spatialNamespace = package;
            this.componentDetails = componentDetails;

            return TransformText();
        }

        private UnityComponentDetails GetComponentDetails()
        {
            return componentDetails;
        }

        private IReadOnlyList<UnityCommandDetails> GetCommandDetailsList()
        {
            return componentDetails.CommandDetails;
        }
    }
}
