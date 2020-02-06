using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEnumDetails : GeneratorInputDetails
    {
        public readonly IReadOnlyList<(uint, string)> Values;

        public UnityEnumDetails(string package, EnumDefinition rawEnumDefinition)
            : base(package, rawEnumDefinition)
        {
            Values = rawEnumDefinition.Values.Select(value => (value.Value, value.Name)).ToList();
        }
    }
}
