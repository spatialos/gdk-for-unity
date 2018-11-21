using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEnumDetails
    {
        public readonly string PascalCaseName;
        public readonly string CamelCaseName;
        public readonly string FullyQualifiedName;

        public readonly List<(string, uint)> Values;

        public UnityEnumDetails(EnumDefinitionRaw enumDefinitionRaw, SchemaBundle.Bundle bundle)
        {
            (PascalCaseName, CamelCaseName, FullyQualifiedName) = enumDefinitionRaw.Identifier.GetNameSet();

            Values = new List<(string, uint)>();
            foreach (var value in enumDefinitionRaw.Values)
            {
                var (valueName, _, _) = enumDefinitionRaw.Identifier.GetNameSet();
                Values.Add((valueName, value.EnumValue));
            }
        }
    }
}
