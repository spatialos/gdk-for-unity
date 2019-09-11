using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.Model
{
    public static class UnityTypeMappings
    {
        public static readonly Dictionary<PrimitiveType, string> SchemaTypeToUnityType =
            new Dictionary<PrimitiveType, string>
            {
                { PrimitiveType.Double, "double" },
                { PrimitiveType.Float, "float" },
                { PrimitiveType.Int32, "int" },
                { PrimitiveType.Int64, "long" },
                { PrimitiveType.Uint32, "uint" },
                { PrimitiveType.Uint64, "ulong" },
                { PrimitiveType.Sint32, "int" },
                { PrimitiveType.Sint64, "long" },
                { PrimitiveType.Fixed32, "uint" },
                { PrimitiveType.Fixed64, "ulong" },
                { PrimitiveType.Sfixed32, "int" },
                { PrimitiveType.Sfixed64, "long" },
                { PrimitiveType.Bool, "bool" },
                { PrimitiveType.String, "string" },
                { PrimitiveType.Bytes, "byte[]" },
                { PrimitiveType.EntityId, "global::Improbable.Gdk.Core.EntityId" },
                { PrimitiveType.Entity, "global::Improbable.Gdk.Core.EntitySnapshot" },
            };
    }
}
