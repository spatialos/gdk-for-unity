using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEnumDetails
    {
        public readonly string Name;
        public readonly string Namespace;

        public readonly string QualifiedName;
        public readonly IReadOnlyList<(uint, string)> Values;

        public UnityEnumDetails(string package, EnumDefinition rawEnumDefinition)
        {
            Name = rawEnumDefinition.Name;
            Namespace = Formatting.CapitaliseQualifiedNameParts(package);

            QualifiedName = rawEnumDefinition.QualifiedName;
            Values = rawEnumDefinition.Values.Select(value => (value.Value, value.Name)).ToList();
        }
    }
}
