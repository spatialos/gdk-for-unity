using System;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class ContainedType
    {
        public readonly string FqnType;
        private readonly ValueType category;
        private PrimitiveType? primitiveType;

        public ContainedType(TypeReference innerType)
        {
            switch (innerType.ValueTypeSelector)
            {
                case ValueType.Enum:
                    category = ValueType.Enum;
                    FqnType = Formatting.CapitaliseQualifiedNameParts(innerType.Enum);
                    primitiveType = null;
                    break;
                case ValueType.Primitive:
                    category = ValueType.Primitive;
                    FqnType = UnityTypeMappings.SchemaTypeToUnityType[innerType.Primitive];
                    primitiveType = innerType.Primitive;
                    break;
                case ValueType.Type:
                    category = ValueType.Type;
                    FqnType = Formatting.CapitaliseQualifiedNameParts(innerType.Type);
                    primitiveType = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Malformed inner type.");
            }
        }

        public string GetSerializationStatement(string instance, string schemaObject, uint fieldNumber)
        {
            switch (category)
            {
                case ValueType.Primitive:
                    return $"{schemaObject}.{SchemaFunctionMappings.AddSchemaFunctionFromType(primitiveType.Value)}({fieldNumber}, {instance});";
                case ValueType.Enum:
                    return $"{schemaObject}.AddEnum({fieldNumber}, (uint) {instance});";
                case ValueType.Type:
                    return $"{FqnType}.Serialization.Serialize({instance}, {schemaObject}.AddObject({fieldNumber}));";
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), "Unknown type category encountered.");
            }
        }

        public string GetDeserializationExpression(string schemaObject, uint fieldNumber)
        {
            switch (category)
            {
                case ValueType.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.GetSchemaFunctionFromType(primitiveType.Value)}({fieldNumber})";
                case ValueType.Enum:
                    return $"({FqnType}) {schemaObject}.GetEnum({fieldNumber})";
                case ValueType.Type:
                    return $"{FqnType}.Serialization.Deserialize({schemaObject}.GetObject({fieldNumber}))";
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), "Unknown type category encountered.");
            }
        }

        public string GetCountExpression(string schemaObject, uint fieldNumber)
        {
            switch (category)
            {
                case ValueType.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.GetCountSchemaFunctionFromType(primitiveType.Value)}({fieldNumber})";
                case ValueType.Enum:
                    return $"{schemaObject}.GetEnumCount({fieldNumber})";
                case ValueType.Type:
                    return $"{schemaObject}.GetObjectCount({fieldNumber})";
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), "Unknown type category encountered.");
            }
        }

        public string GetFieldIndexExpression(string schemaObject, uint fieldNumber, string index)
        {
            switch (category)
            {
                case ValueType.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.IndexSchemaFunctionFromType(primitiveType.Value)}({fieldNumber}, (uint) {index})";
                case ValueType.Enum:
                    return $"({FqnType}) {schemaObject}.IndexEnum({fieldNumber}, (uint) {index})";
                case ValueType.Type:
                    return $"{FqnType}.Serialization.Deserialize({schemaObject}.IndexObject({fieldNumber}, (uint) {index}))";
                default:
                    throw new ArgumentOutOfRangeException(nameof(category), "Unknown type category encountered.");
            }
        }
    }
}
