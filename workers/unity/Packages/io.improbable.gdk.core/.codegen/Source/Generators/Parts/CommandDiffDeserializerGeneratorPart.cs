using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class CommandDiffDeserializerGenerator
    {
        private string qualifiedNamespace;
        private string spatialNamespace;
        private UnityComponentDetails details;

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
