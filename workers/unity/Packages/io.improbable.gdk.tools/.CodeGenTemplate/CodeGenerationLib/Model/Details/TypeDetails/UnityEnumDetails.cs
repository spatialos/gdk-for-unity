using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityEnumDetails : GeneratorInputDetails
    {
        public readonly IReadOnlyList<(uint, string)> Values;

        private static readonly Dictionary<string, uint> EnumMinimums = new Dictionary<string, uint>();

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UnityEnumDetails(string package, EnumDefinition rawEnumDefinition)
            : base(package, rawEnumDefinition)
        {
            var min = rawEnumDefinition.Values.Select(list => list.Value).Min();
            EnumMinimums[FullyQualifiedName] = min;
            if (min != 0)
            {
                Logger.Warn($"The enum, {Name}, is defined with a minimum value of {min}, which is greater than 0. Shifting the enum values to start from 0. " +
                    "This will lead to inconsistencies in the values used in Unity and the values captured in snapshots. " +
                    $"This inconsistency is handled in the serialization/deserialization process but please consider redefining the values in {Name} to start from 0");
            }

            Values = rawEnumDefinition.Values.Select(value => (value.Value - min, value.Name)).ToList();
        }

        public static uint GetEnumMinimum(string fullQualifiedName)
        {
            EnumMinimums.TryGetValue(fullQualifiedName, out var output);
            return output;
        }
    }
}
