using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     This class contains the data required to fill out the "UnityReaderWriterGenerator.tt" templates.
    ///     This template generates the ReaderWriter implementation for Components.
    /// </summary>
    public partial class UnityReaderWriterGenerator
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
