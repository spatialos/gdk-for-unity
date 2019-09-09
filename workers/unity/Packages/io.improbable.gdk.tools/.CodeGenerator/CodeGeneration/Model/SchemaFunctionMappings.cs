using System;
using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.Model
{
    public static class SchemaFunctionMappings
    {
        private static readonly HashSet<PrimitiveType> BuiltInTypeWithSchemaFunctions = new HashSet<PrimitiveType>
        {
            PrimitiveType.Bool,
            PrimitiveType.Bytes,
            PrimitiveType.Double,
            PrimitiveType.EntityId,
            PrimitiveType.Fixed32,
            PrimitiveType.Fixed64,
            PrimitiveType.Float,
            PrimitiveType.Int32,
            PrimitiveType.Int64,
            PrimitiveType.Sfixed32,
            PrimitiveType.Sfixed64,
            PrimitiveType.Uint32,
            PrimitiveType.Uint64,
            PrimitiveType.Sint32,
            PrimitiveType.Sint64,
            PrimitiveType.String,
            PrimitiveType.Entity,
        };

        public static string AddSchemaFunctionFromType(PrimitiveType builtInType)
        {
            if (!BuiltInTypeWithSchemaFunctions.Contains(builtInType))
            {
                throw new ArgumentException($"There are no raw schema functions defined for {builtInType}");
            }

            return $"Add{builtInType.ToString()}";
        }

        public static string GetSchemaFunctionFromType(PrimitiveType builtInType)
        {
            if (!BuiltInTypeWithSchemaFunctions.Contains(builtInType))
            {
                throw new ArgumentException($"There are no raw schema functions defined for {builtInType}");
            }

            return builtInType == PrimitiveType.EntityId
                ? $"Get{builtInType.ToString()}Struct"
                : $"Get{builtInType.ToString()}";
        }

        public static string GetCountSchemaFunctionFromType(PrimitiveType builtInType)
        {
            if (!BuiltInTypeWithSchemaFunctions.Contains(builtInType))
            {
                throw new ArgumentException($"There are no raw schema functions defined for {builtInType}");
            }

            return $"Get{builtInType.ToString()}Count";
        }

        public static string IndexSchemaFunctionFromType(PrimitiveType builtInType)
        {
            if (!BuiltInTypeWithSchemaFunctions.Contains(builtInType))
            {
                throw new ArgumentException($"There are no raw schema functions defined for {builtInType}");
            }

            return builtInType == PrimitiveType.EntityId
                ? $"Index{builtInType.ToString()}Struct"
                : $"Index{builtInType.ToString()}";
        }
    }
}
