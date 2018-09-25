using System.Collections.Generic;
using Improbable.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityTypeMappings
    {
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
                { BuiltInTypeConstants.builtInCoordinates, "global::Improbable.Coordinates" },
                { BuiltInTypeConstants.builtInVector3d, "global::Improbable.Vector3d" },
                { BuiltInTypeConstants.builtInVector3f, "global::Improbable.Vector3f" }
            };
    }
}
