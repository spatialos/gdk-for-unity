using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration;

namespace Improbable.Gdk.CodeGenerator
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
                { PrimitiveType.Entity, "TODO" },
            };

        public static bool IsEntity(FieldDefinition field)
        {
            if (field.OptionType != null)
            {
                return field.OptionType.InnerType.ValueTypeSelector == ValueType.Primitive &&
                    field.OptionType.InnerType.Primitive == PrimitiveType.Entity;
            }

            if (field.ListType != null)
            {
                return field.ListType.InnerType.ValueTypeSelector == ValueType.Primitive &&
                    field.ListType.InnerType.Primitive == PrimitiveType.Entity;
            }

            if (field.MapType != null)
            {
                return (field.MapType.KeyType.ValueTypeSelector == ValueType.Primitive &&
                        field.MapType.KeyType.Primitive == PrimitiveType.Entity) ||
                    (field.MapType.ValueType.ValueTypeSelector == ValueType.Primitive &&
                        field.MapType.ValueType.Primitive == PrimitiveType.Entity);
            }

            return field.SingularType.Type.ValueTypeSelector == ValueType.Primitive &&
                field.SingularType.Type.Primitive == PrimitiveType.Entity;
        }
    }
}
