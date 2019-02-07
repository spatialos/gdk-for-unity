using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityEventGenerator
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

        private IReadOnlyList<UnityEventDetails> GetEventDetailsList()
        {
            return details.EventDetails;
        }
    }
}
