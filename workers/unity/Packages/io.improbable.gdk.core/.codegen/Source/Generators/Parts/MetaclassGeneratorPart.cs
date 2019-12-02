using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class MetaclassGenerator
    {
        private UnityComponentDetails details;
        private string qualifiedNamespace;

        private Logger logger = LogManager.GetCurrentClassLogger();

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
