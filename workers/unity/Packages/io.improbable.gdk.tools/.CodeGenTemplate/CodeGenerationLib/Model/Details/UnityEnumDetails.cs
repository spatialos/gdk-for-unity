using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEnumDetails : GeneratorDetails
    {
        public readonly string QualifiedName;
        public readonly IReadOnlyList<(uint, string)> Values;

        public UnityEnumDetails(string package, EnumDefinition rawEnumDefinition) : base(rawEnumDefinition.Name, package)
        {
            QualifiedName = rawEnumDefinition.QualifiedName;
            Values = rawEnumDefinition.Values.Select(value => (value.Value, value.Name)).ToList();
        }
    }
}
