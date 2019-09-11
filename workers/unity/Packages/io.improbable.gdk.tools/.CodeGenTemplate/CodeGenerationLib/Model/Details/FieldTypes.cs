using System;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public interface IFieldType
    {
        string Type { get; }
        string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents);
        string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents);

        string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber, int indents);

        string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber,
            int indents);

        string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber,
            int indents);

        string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber,
            int indents);
    }

    public static class CommonCodeWriterBlocks
    {
        public static void WriteCheckIsCleared(CodeWriter codeWriter, uint fieldNumber)
        {
            codeWriter.WriteLine($"var isCleared = updateObj.IsFieldCleared({fieldNumber});");
        }
    }

    public class SingularFieldType : IFieldType
    {
        public string Type => containedType.FqnType;

        private readonly ContainedType containedType;

        public SingularFieldType(TypeReference innerType, DetailsStore store)
        {
            containedType = new ContainedType(innerType);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            return containedType.GetSerializationStatement(fieldInstance, schemaObject, fieldNumber);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            return $"{fieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};";
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)"))
            {
                codeWriter.WriteLine($"{fieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)"))
            {
                codeWriter.WriteLine($"{updateFieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"{updateFieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber,
            int indents)
        {
            return "";
        }
    }

    public class OptionFieldType : IFieldType
    {
        public string Type
        {
            get
            {
                if (containedType.FqnType ==
                    UnityTypeMappings.SchemaTypeToUnityType[PrimitiveType.String]
                    ||
                    containedType.FqnType ==
                    UnityTypeMappings.SchemaTypeToUnityType[PrimitiveType.Bytes])
                {
                    return $"global::Improbable.Gdk.Core.Option<{containedType.FqnType}>";
                }

                return $"{containedType.FqnType}?";
            }
        }

        private readonly ContainedType containedType;

        public OptionFieldType(TypeReference innerType, DetailsStore store)
        {
            containedType = new ContainedType(innerType);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope($"if ({fieldInstance}.HasValue)"))
            {
                codeWriter.WriteLine(containedType.GetSerializationStatement($"{fieldInstance}.Value", schemaObject, fieldNumber));
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)"))
            {
                codeWriter.WriteLine($"{fieldInstance} = new {Type}({containedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();

            CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

            using (codeWriter.Scope("if (isCleared)"))
            {
                codeWriter.WriteLine($"{fieldInstance} = new {Type}();");
            }

            using (codeWriter.Scope($"else if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)"))
            {
                codeWriter.WriteLine($"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
                codeWriter.WriteLine($"{fieldInstance} = new {Type}(value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();

            CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

            using (codeWriter.Scope("if (isCleared)"))
            {
                codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
            }

            using (codeWriter.Scope($"else if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)"))
            {
                codeWriter.WriteLine($"{updateFieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();

            using (codeWriter.Scope($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)"))
            {
                codeWriter.WriteLine($"{updateFieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber,
            int indents)
        {
            var codeWriter = new CodeWriter();

            using (codeWriter.Scope($"if (!{fieldInstance}.HasValue)"))
            {
                codeWriter.WriteLine($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }
    }

    public class ListFieldType : IFieldType
    {
        public string Type => $"global::System.Collections.Generic.List<{containedType.FqnType}>";

        private readonly ContainedType containedType;

        public ListFieldType(TypeReference innerType, DetailsStore store)
        {
            containedType = new ContainedType(innerType);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope($"foreach (var value in {fieldInstance})"))
            {
                codeWriter.WriteLine(containedType.GetSerializationStatement("value", schemaObject, fieldNumber));
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{fieldInstance} = new {Type}();");
                codeWriter.WriteLine($"var list = {fieldInstance};");
                codeWriter.WriteLine(
                    $"var listLength = {containedType.GetCountExpression(schemaObject, fieldNumber)};");

                using (codeWriter.Scope("for (var i = 0; i < listLength; i++)"))
                {
                    codeWriter.WriteLine(
                        $"list.Add({containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")});");
                }
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var listSize = {containedType.GetCountExpression(schemaObject, fieldNumber)};");

                CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

                using (codeWriter.Scope("if (listSize > 0 || isCleared)"))
                {
                    codeWriter.WriteLine($"{fieldInstance}.Clear();");
                }

                using (codeWriter.Scope("for (var i = 0; i < listSize; i++)"))
                {
                    codeWriter.WriteLine(
                        $"var value = {containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};");
                    codeWriter.WriteLine($"{fieldInstance}.Add(value);");
                }
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var listSize = {containedType.GetCountExpression(schemaObject, fieldNumber)};");

                CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

                using (codeWriter.Scope("if (listSize > 0 || isCleared)"))
                {
                    codeWriter.WriteLine(
                        $"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
                }

                using (codeWriter.Scope("for (var i = 0; i < listSize; i++)"))
                {
                    codeWriter.WriteLine(
                        $"var value = {containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};");
                    codeWriter.WriteLine($"{updateFieldInstance}.Value.Add(value);");
                }
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var listSize = {containedType.GetCountExpression(schemaObject, fieldNumber)};");

                codeWriter.WriteLine(
                    $"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");

                using (codeWriter.Scope("for (var i = 0; i < listSize; i++)"))
                {
                    codeWriter.WriteLine(
                        $"var value = {containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};");
                    codeWriter.WriteLine($"{updateFieldInstance}.Value.Add(value);");
                }
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber,
            int indents)
        {
            var codeWriter = new CodeWriter();

            using (codeWriter.Scope($"if ({fieldInstance}.Count == 0)"))
            {
                codeWriter.WriteLine($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }
    }

    public class MapFieldType : IFieldType
    {
        public string Type => $"global::System.Collections.Generic.Dictionary<{keyType.FqnType}, {valueType.FqnType}>";

        private readonly ContainedType keyType;
        private readonly ContainedType valueType;

        public MapFieldType(TypeReference keyType, TypeReference valueType, DetailsStore store)
        {
            this.keyType = new ContainedType(keyType);
            this.valueType = new ContainedType(valueType);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope($"foreach (var keyValuePair in {fieldInstance})"))
            {
                codeWriter.WriteLine($"var mapObj = {schemaObject}.AddObject({fieldNumber});");
                codeWriter.WriteLine(keyType.GetSerializationStatement("keyValuePair.Key", "mapObj", 1));
                codeWriter.WriteLine(valueType.GetSerializationStatement("keyValuePair.Value", "mapObj", 2));
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var map = new {Type}();");
                codeWriter.WriteLine($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");
                codeWriter.WriteLine($"{fieldInstance} = map;");
                using (codeWriter.Scope("for (var i = 0; i < mapSize; i++)"))
                {
                    codeWriter.WriteLine($"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);");
                    codeWriter.WriteLine($"var key = {keyType.GetDeserializationExpression("mapObj", 1)};");
                    codeWriter.WriteLine($"var value = {valueType.GetDeserializationExpression("mapObj", 2)};");
                    codeWriter.WriteLine("map.Add(key, value);");
                }
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");

                CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

                using (codeWriter.Scope("if (mapSize > 0 || isCleared)"))
                {
                    codeWriter.WriteLine($"{fieldInstance}.Clear();");
                }

                using (codeWriter.Scope("for (var i = 0; i < mapSize; i++)"))
                {
                    codeWriter.WriteLine($"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);");
                    codeWriter.WriteLine($"var key = {keyType.GetDeserializationExpression("mapObj", 1)};");
                    codeWriter.WriteLine($"var value = {valueType.GetDeserializationExpression("mapObj", 2)};");
                    codeWriter.WriteLine($"{fieldInstance}.Add(key, value);");
                }
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");

                CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

                using (codeWriter.Scope("if (mapSize > 0 || isCleared)"))
                {
                    codeWriter.WriteLine(
                        $"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
                }

                using (codeWriter.Scope("for (var i = 0; i < mapSize; i++)"))
                {
                    codeWriter.WriteLine($"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);");
                    codeWriter.WriteLine($"var key = {keyType.GetDeserializationExpression("mapObj", 1)};");
                    codeWriter.WriteLine($"var value = {valueType.GetDeserializationExpression("mapObj", 2)};");
                    codeWriter.WriteLine($"{updateFieldInstance}.Value.Add(key, value);");
                }
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var map = new {Type}();");
                codeWriter.WriteLine($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");
                codeWriter.WriteLine($"{updateFieldInstance} = map;");

                using (codeWriter.Scope("for (var i = 0; i < mapSize; i++)"))
                {
                    codeWriter.WriteLine($"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);");
                    codeWriter.WriteLine($"var key = {keyType.GetDeserializationExpression("mapObj", 1)};");
                    codeWriter.WriteLine($"var value = {valueType.GetDeserializationExpression("mapObj", 2)};");
                    codeWriter.WriteLine($"map.Add(key, value);");
                }
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber,
            int indents)
        {
            var codeWriter = new CodeWriter();

            using (codeWriter.Scope($"if ({fieldInstance}.Count == 0)"))
            {
                codeWriter.WriteLine($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }
    }

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
                    FqnType = CommonDetailsUtils.GetCapitalisedFqnTypename(innerType.Enum);
                    primitiveType = null;
                    break;
                case ValueType.Primitive:
                    category = ValueType.Primitive;
                    FqnType = UnityTypeMappings.SchemaTypeToUnityType[innerType.Primitive];
                    primitiveType = innerType.Primitive;
                    break;
                case ValueType.Type:
                    category = ValueType.Type;
                    FqnType = CommonDetailsUtils.GetCapitalisedFqnTypename(innerType.Type);
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
