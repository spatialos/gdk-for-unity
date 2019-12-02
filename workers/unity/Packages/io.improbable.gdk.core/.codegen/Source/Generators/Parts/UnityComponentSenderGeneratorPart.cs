using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

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

        private IReadOnlyList<UnityFieldDetails> GetFieldDetailsList()
        {
            return details.FieldDetails;
        }
    }
}
