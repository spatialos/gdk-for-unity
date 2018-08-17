using System.Collections.Generic;
using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityTypeMappings
    {
        public const string PackagePrefix = "Generated.";

        private const int EntityAclComponentId = 50;
        private const int MetadataComponentId = 53;
        private const int PositionComponentId = 54;
        private const int PersistenceComponentId = 55;

        public static readonly HashSet<int> WellKnownComponents = new HashSet<int>
        {
            EntityAclComponentId,
            MetadataComponentId,
            PositionComponentId,
            PersistenceComponentId
        };

        public static readonly Dictionary<string, string> BuiltInSchemaTypeToUnityNativeType =
            new Dictionary<string, string>
            {
                { BuiltInTypeConstants.builtInDouble, "double" },
                { BuiltInTypeConstants.builtInFloat, "float" },
                { BuiltInTypeConstants.builtInInt32, "int" },
                { BuiltInTypeConstants.builtInInt64, "long" },
                { BuiltInTypeConstants.builtInUint32, "uint" },
                { BuiltInTypeConstants.builtInUint64, "ulong" },
                { BuiltInTypeConstants.builtInSint32, "int" },
                { BuiltInTypeConstants.builtInSint64, "long" },
                { BuiltInTypeConstants.builtInFixed32, "uint" },
                { BuiltInTypeConstants.builtInFixed64, "ulong" },
                { BuiltInTypeConstants.builtInSfixed32, "int" },
                { BuiltInTypeConstants.builtInSfixed64, "long" },
                { BuiltInTypeConstants.builtInBool, "BlittableBool" },
                { BuiltInTypeConstants.builtInString, "string" },
                { BuiltInTypeConstants.builtInBytes, "byte[]" },
                { BuiltInTypeConstants.builtInEntityId, "global::Improbable.Worker.EntityId" },
                { BuiltInTypeConstants.builtInCoordinates, "global::Generated.Improbable.Coordinates" },
                { BuiltInTypeConstants.builtInVector3d, "global::Generated.Improbable.Vector3d" },
                { BuiltInTypeConstants.builtInVector3f, "global::Generated.Improbable.Vector3f" }
            };

        public static readonly HashSet<string> SchemaTypesThatRequireNoConversion = new HashSet<string>
        {
            BuiltInTypeConstants.builtInDouble,
            BuiltInTypeConstants.builtInFloat,
            BuiltInTypeConstants.builtInInt32,
            BuiltInTypeConstants.builtInInt64,
            BuiltInTypeConstants.builtInUint32,
            BuiltInTypeConstants.builtInUint64,
            BuiltInTypeConstants.builtInSint32,
            BuiltInTypeConstants.builtInSint64,
            BuiltInTypeConstants.builtInFixed32,
            BuiltInTypeConstants.builtInFixed64,
            BuiltInTypeConstants.builtInSfixed32,
            BuiltInTypeConstants.builtInSfixed64,
            BuiltInTypeConstants.builtInString,
            BuiltInTypeConstants.builtInBool
        };
    }
}
