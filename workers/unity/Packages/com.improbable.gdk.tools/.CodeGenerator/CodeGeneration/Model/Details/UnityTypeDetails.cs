using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityTypeDetails
    {
        public readonly string PascalCaseName;
        public readonly string CamelCaseName;
        public readonly string FullyQualifiedName;

        public readonly List<UnityFieldDetails> Fields;

        public UnityTypeDetails(TypeDefinitionRaw typeDefinitionRaw, SchemaBundle.Bundle bundle)
        {
            (PascalCaseName, CamelCaseName, FullyQualifiedName) = typeDefinitionRaw.Identifier.GetNameSet();
            Fields = typeDefinitionRaw.Fields.Select(field => new UnityFieldDetails(field, bundle)).ToList();
        }
    }
}
