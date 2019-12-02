using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityComponentDataGenerator
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

        private bool ShouldGenerateClearedFieldsSet()
        {
            return GetFieldDetailsList().Any(fieldDetails => fieldDetails.CanBeEmpty);
        }

        private string GetConstructorArgs()
        {
            var constructorArgsList = GetFieldDetailsList().Select(fieldDetails => $"{fieldDetails.Type} {fieldDetails.CamelCaseName}");
            return string.Join(", ", constructorArgsList);
        }
    }
}
