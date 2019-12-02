using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class CommandMetaDataStorageGenerator
    {
        private string qualifiedNamespace;
        private string spatialNamespace;
        private UnityComponentDetails componentDetails;

        private Logger logger = LogManager.GetCurrentClassLogger();

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
