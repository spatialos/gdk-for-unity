using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Model;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityTypeMappings
    {
        public static readonly Dictionary<string, string> BuiltInSchemaTypeToUnityNativeType =
            new Dictionary<string, string>
            {
                { BuiltInSchemaTypes.BuiltInDouble, "double" },
                { BuiltInSchemaTypes.BuiltInFloat, "float" },
                { BuiltInSchemaTypes.BuiltInInt32, "int" },
                { BuiltInSchemaTypes.BuiltInInt64, "long" },
                { BuiltInSchemaTypes.BuiltInUint32, "uint" },
                { BuiltInSchemaTypes.BuiltInUint64, "ulong" },
                { BuiltInSchemaTypes.BuiltInSint32, "int" },
                { BuiltInSchemaTypes.BuiltInSint64, "long" },
                { BuiltInSchemaTypes.BuiltInFixed32, "uint" },
                { BuiltInSchemaTypes.BuiltInFixed64, "ulong" },
                { BuiltInSchemaTypes.BuiltInSfixed32, "int" },
                { BuiltInSchemaTypes.BuiltInSfixed64, "long" },
                { BuiltInSchemaTypes.BuiltInBool, "bool" },
                { BuiltInSchemaTypes.BuiltInString, "string" },
                { BuiltInSchemaTypes.BuiltInBytes, "byte[]" },
                { BuiltInSchemaTypes.BuiltInEntityId, "global::Improbable.Gdk.Core.EntityId" }
            };
    }
}
