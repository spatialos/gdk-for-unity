using System;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class ContainedType
    {
        public readonly string FqnType;
        public readonly ValueType Category;
        public readonly PrimitiveType? PrimitiveType;

        public ContainedType(TypeReference innerType)
        {
            switch (innerType.ValueTypeSelector)
            {
                case ValueType.Enum:
                    Category = ValueType.Enum;
                    FqnType = DetailsUtils.GetCapitalisedFqnTypename(innerType.Enum);
                    PrimitiveType = null;
                    break;
                case ValueType.Primitive:
                    Category = ValueType.Primitive;
                    FqnType = UnityTypeMappings.SchemaTypeToUnityType[innerType.Primitive];
                    PrimitiveType = innerType.Primitive;
                    break;
                case ValueType.Type:
                    Category = ValueType.Type;
                    FqnType = DetailsUtils.GetCapitalisedFqnTypename(innerType.Type);
                    PrimitiveType = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Malformed inner type.");
            }
        }

        public string GetSerializationStatement(string instance, string schemaObject, uint fieldNumber)
        {
            switch (Category)
            {
                case ValueType.Primitive:
                    return $"{schemaObject}.{SchemaFunctionMappings.AddSchemaFunctionFromType(PrimitiveType.Value)}({fieldNumber}, {instance});";
                case ValueType.Enum:
                    return $"{schemaObject}.AddEnum({fieldNumber}, (uint) {instance});";
                case ValueType.Type:
                    return $"{FqnType}.Serialization.Serialize({instance}, {schemaObject}.AddObject({fieldNumber}));";
                default:
                    throw new ArgumentOutOfRangeException(nameof(Category), "Unknown type category encountered.");
            }
        }

        public string GetDeserializationExpression(string schemaObject, uint fieldNumber)
        {
            switch (Category)
            {
                case ValueType.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.GetSchemaFunctionFromType(PrimitiveType.Value)}({fieldNumber})";
                case ValueType.Enum:
                    return $"({FqnType}) {schemaObject}.GetEnum({fieldNumber})";
                case ValueType.Type:
                    return $"{FqnType}.Serialization.Deserialize({schemaObject}.GetObject({fieldNumber}))";
                default:
                    throw new ArgumentOutOfRangeException(nameof(Category), "Unknown type category encountered.");
            }
        }

        public string GetCountExpression(string schemaObject, uint fieldNumber)
        {
            switch (Category)
            {
                case ValueType.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.GetCountSchemaFunctionFromType(PrimitiveType.Value)}({fieldNumber})";
                case ValueType.Enum:
                    return $"{schemaObject}.GetEnumCount({fieldNumber})";
                case ValueType.Type:
                    return $"{schemaObject}.GetObjectCount({fieldNumber})";
                default:
                    throw new ArgumentOutOfRangeException(nameof(Category), "Unknown type category encountered.");
            }
        }

        public string GetFieldIndexExpression(string schemaObject, uint fieldNumber, string index)
        {
            switch (Category)
            {
                case ValueType.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.IndexSchemaFunctionFromType(PrimitiveType.Value)}({fieldNumber}, (uint) {index})";
                case ValueType.Enum:
                    return $"({FqnType}) {schemaObject}.IndexEnum({fieldNumber}, (uint) {index})";
                case ValueType.Type:
                    return $"{FqnType}.Serialization.Deserialize({schemaObject}.IndexObject({fieldNumber}, (uint) {index}))";
                default:
                    throw new ArgumentOutOfRangeException(nameof(Category), "Unknown type category encountered.");
            }
        }
    }
}
