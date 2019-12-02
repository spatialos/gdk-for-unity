using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public partial class UnityTypeContent
    {
        private UnityTypeDetails details;
        private IReadOnlyList<UnityTypeDetails> nestedTypes;
        private IReadOnlyList<UnityEnumDetails> nestedEnums;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public string Generate(UnityTypeDetails details)
        {
            this.details = details;
            nestedTypes = details.ChildTypes;
            nestedEnums = details.ChildEnums;

            return TransformText();
        }

        private UnityTypeDetails GetTypeDetails()
        {
            return details;
        }

        private IReadOnlyList<UnityFieldDetails> GetFieldDetailsList()
        {
            return details.FieldDetails;
        }

        private string GetConstructorArgs()
        {
            var constructorArgsList = GetFieldDetailsList().Select(fieldDetails => $"{fieldDetails.Type} {fieldDetails.CamelCaseName}");
            return string.Join(", ", constructorArgsList);
        }
    }
}
