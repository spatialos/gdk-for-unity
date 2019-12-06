using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityComponentReaderWriterGenerator
    {
        private string qualifiedNamespace;
        private UnityComponentDetails details;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public UnityComponentReaderWriterGenerator()
        {
            logger.Trace($"Constructing {GetType()}.");
        }

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

        private IReadOnlyList<UnityFieldDetails> GetFieldDetailsList()
        {
            return details.FieldDetails;
        }

        private IReadOnlyList<UnityEventDetails> GetEventDetailsList()
        {
            return details.EventDetails;
        }

        private IReadOnlyList<UnityCommandDetails> GetCommandDetailsList()
        {
            return details.CommandDetails;
        }
    }
}
