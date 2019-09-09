using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class CommandDiffStorageGenerator
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

        private IReadOnlyList<UnityCommandDetails> GetCommandDetailsList()
        {
            return details.CommandDetails;
        }
    }
}
