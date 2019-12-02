using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class CommandDiffDeserializerGenerator
    {
        private string qualifiedNamespace;
        private string spatialNamespace;
        private UnityComponentDetails details;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public string Generate(UnityComponentDetails details, string package)
        {
            qualifiedNamespace = package;
            spatialNamespace = package;
            this.details = details;

            return TransformText();
        }

        private UnityComponentDetails GetComponentDetails()
        {
            return details;
        }

        private IReadOnlyList<UnityCommandDetails> GetCommandDetailsList()
        {
            return details.CommandDetails;
        }
    }
}
