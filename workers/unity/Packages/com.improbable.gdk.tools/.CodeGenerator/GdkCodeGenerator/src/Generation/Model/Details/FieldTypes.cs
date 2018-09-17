using System;
using System.Collections.Generic;
using Improbable.CodeGeneration.Model;
using Improbable.Gdk.CodeGenerator.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public interface IFieldType
    {
        string Type { get; }
        string GetSerializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents);
        string GetDeserializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents);

        string GetDeserializeUpdateString(string fieldInstance, string schemaObject, int fieldNumber, int indents);

        string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, int fieldNumber,
            int indents);

        string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, int fieldNumber,
            int indents);
    }

    public static class CommonCodeWriterBlocks
    {
        public static void WriteCheckIsCleared(CodeWriter codeWriter, int fieldNumber)
        {
            codeWriter.WriteLine("bool isCleared = false;");
            codeWriter.WriteLine("foreach (var fieldIndex in clearedFields)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"isCleared = fieldIndex == {fieldNumber};");
                codeWriter.WriteLine("if (isCleared)");
                using (codeWriter.Scope())
                {
                    codeWriter.WriteLine("break;");
                }
            }
        }
    }

    public class SingularFieldType : IFieldType
    {
        public string Type => containedType.Type;

        private ContainedType containedType;

        public SingularFieldType(TypeReferenceRaw typeReferenceRaw, HashSet<string> enumSet)
        {
            containedType = new ContainedType(typeReferenceRaw, enumSet);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            return containedType.GetSerializationStatement(fieldInstance, schemaObject, fieldNumber);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            return $"{fieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};";
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject,
            int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
                codeWriter.WriteLine($"{fieldInstance} = value;");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
                codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, int fieldNumber,
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
                if (containedType.Type ==
                    UnityTypeMappings.BuiltInSchemaTypeToUnityNativeType[BuiltInTypeConstants.builtInString]
                    ||
                    containedType.Type ==
                    UnityTypeMappings.BuiltInSchemaTypeToUnityNativeType[BuiltInTypeConstants.builtInBytes])
                {
                    return $"global::Improbable.Gdk.Core.Option<{containedType.Type}>";
                }

                return $"{containedType.Type}?";
            }
        }

        private ContainedType containedType;

        public OptionFieldType(FieldDefinitionRaw.OptionTypeRaw optionTypeRaw, HashSet<string> enumSet)
        {
            containedType = new ContainedType(optionTypeRaw.valueType, enumSet);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"if ({fieldInstance}.HasValue)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine(containedType.GetSerializationStatement($"{fieldInstance}.Value", schemaObject, fieldNumber));
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{fieldInstance} = new {Type}({containedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();

            CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

            codeWriter.WriteLine("if (isCleared)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{fieldInstance} = new {Type}();");
            }

            codeWriter.WriteLine($"else if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
                codeWriter.WriteLine($"{fieldInstance} = new {Type}(value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();

            CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

            codeWriter.WriteLine("if (isCleared)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
            }

            codeWriter.WriteLine($"else if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
                codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}(value));");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, int fieldNumber,
            int indents)
        {
            var codeWriter = new CodeWriter();

            codeWriter.WriteLine($"if (!{fieldInstance}.HasValue)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }
    }

    public class ListFieldType : IFieldType
    {
        public string Type => $"global::System.Collections.Generic.List<{containedType.Type}>";

        private ContainedType containedType;

        public ListFieldType(FieldDefinitionRaw.ListTypeRaw listTypeRaw, HashSet<string> enumSet)
        {
            containedType = new ContainedType(listTypeRaw.valueType, enumSet);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"foreach (var value in {fieldInstance})");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine(containedType.GetSerializationStatement("value", schemaObject, fieldNumber));
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"var list = {fieldInstance} = new {Type}();");
            codeWriter.WriteLine($"var listLength = {containedType.GetCountExpression(schemaObject, fieldNumber)};");
            codeWriter.WriteLine("for (var i = 0; i < listLength; i++)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"list.Add({containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject,
            int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"var listSize = {containedType.GetCountExpression(schemaObject, fieldNumber)};");

            CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

            codeWriter.WriteLine("if (listSize > 0 || isCleared)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{fieldInstance}.Clear();");
            }

            codeWriter.WriteLine("for (var i = 0; i < listSize; i++)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};");
                codeWriter.WriteLine($"{fieldInstance}.Add(value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"var listSize = {containedType.GetCountExpression(schemaObject, fieldNumber)};");

            CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

            codeWriter.WriteLine("if (listSize > 0 || isCleared)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
            }

            codeWriter.WriteLine("for (var i = 0; i < listSize; i++)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};");
                codeWriter.WriteLine($"{updateFieldInstance}.Value.Add(value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, int fieldNumber,
            int indents)
        {
            var codeWriter = new CodeWriter();

            codeWriter.WriteLine($"if ({fieldInstance}.Count == 0)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }
    }

    public class MapFieldType : IFieldType
    {
        public string Type => $"global::System.Collections.Generic.Dictionary<{keyType.Type},{valueType.Type}>";

        private ContainedType keyType;
        private ContainedType valueType;

        public MapFieldType(FieldDefinitionRaw.MapTypeRaw mapTypeRaw, HashSet<string> enumSet)
        {
            keyType = new ContainedType(mapTypeRaw.keyType, enumSet);
            valueType = new ContainedType(mapTypeRaw.valueType, enumSet);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"foreach (var keyValuePair in {fieldInstance})");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var mapObj = {schemaObject}.AddObject({fieldNumber});");
                codeWriter.WriteLine(keyType.GetSerializationStatement("keyValuePair.Key", "mapObj", 1));
                codeWriter.WriteLine(valueType.GetSerializationStatement("keyValuePair.Value", "mapObj", 2));
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();

            codeWriter.WriteLine($"var map = {fieldInstance} = new {Type}();");
            codeWriter.WriteLine($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");
            codeWriter.WriteLine("for (var i = 0; i < mapSize; i++)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);");
                codeWriter.WriteLine($"var key = {keyType.GetDeserializationExpression("mapObj", 1)};");
                codeWriter.WriteLine($"var value = {valueType.GetDeserializationExpression("mapObj", 2)};");
                codeWriter.WriteLine("map.Add(key, value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");

            CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

            codeWriter.WriteLine("if (mapSize > 0 || isCleared)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{fieldInstance}.Clear();");
            }

            codeWriter.WriteLine("for (var i = 0; i < mapSize; i++)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);");
                codeWriter.WriteLine($"var key = {keyType.GetDeserializationExpression("mapObj", 1)};");
                codeWriter.WriteLine($"var value = {valueType.GetDeserializationExpression("mapObj", 2)};");
                codeWriter.WriteLine($"{fieldInstance}.Add(key, value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            int fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");

            CommonCodeWriterBlocks.WriteCheckIsCleared(codeWriter, fieldNumber);

            codeWriter.WriteLine("if (mapSize > 0 || isCleared)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
            }

            codeWriter.WriteLine("for (var i = 0; i < mapSize; i++)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);");
                codeWriter.WriteLine($"var key = {keyType.GetDeserializationExpression("mapObj", 1)};");
                codeWriter.WriteLine($"var value = {valueType.GetDeserializationExpression("mapObj", 2)};");
                codeWriter.WriteLine($"{updateFieldInstance}.Value.Add(key, value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, int fieldNumber,
            int indents)
        {
            var codeWriter = new CodeWriter();

            codeWriter.WriteLine($"if ({fieldInstance}.Count == 0)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }
    }

    public class ContainedType
    {
        private enum ContainedTypeCategory
        {
            Primitive = 0,
            Enum = 1,
            UserDefined = 2
        }

        public string Type;
        private ContainedTypeCategory category;
        private string rawType;

        public ContainedType(TypeReferenceRaw typeReferenceRaw, HashSet<string> enumSet)
        {
            category = enumSet.Contains(typeReferenceRaw.TypeName)
                ? ContainedTypeCategory.Enum
                : SchemaFunctionMappings.BuiltInTypeWithSchemaFunctions.Contains(typeReferenceRaw.TypeName)
                    ? ContainedTypeCategory.Primitive
                    : ContainedTypeCategory.UserDefined;
            Type = CommonDetailsUtils.GetFqnTypeFromTypeReference(typeReferenceRaw);
            rawType = typeReferenceRaw.TypeName;
        }

        public string GetSerializationStatement(string instance, string schemaObject, int fieldNumber)
        {
            switch (category)
            {
                case ContainedTypeCategory.Primitive:
                    return $"{schemaObject}.{SchemaFunctionMappings.AddSchemaFunctionFromType(rawType)}({fieldNumber}, {instance});";
                case ContainedTypeCategory.Enum:
                    return $"{schemaObject}.AddEnum({fieldNumber}, (uint) {instance});";
                case ContainedTypeCategory.UserDefined:
                    return $"{Type}.Serialization.Serialize({instance}, {schemaObject}.AddObject({fieldNumber}));";
                default:
                    throw new ArgumentOutOfRangeException("Unknown type category encountered.");
            }
        }

        public string GetDeserializationExpression(string schemaObject, int fieldNumber)
        {
            switch (category)
            {
                case ContainedTypeCategory.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.GetSchemaFunctionFromType(rawType)}({fieldNumber})";
                case ContainedTypeCategory.Enum:
                    return $"({Type}) {schemaObject}.GetEnum({fieldNumber})";
                case ContainedTypeCategory.UserDefined:
                    return $"{Type}.Serialization.Deserialize({schemaObject}.GetObject({fieldNumber}))";
                default:
                    throw new ArgumentOutOfRangeException("Unknown type category encountered.");
            }
        }

        public string GetCountExpression(string schemaObject, int fieldNumber)
        {
            switch (category)
            {
                case ContainedTypeCategory.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.GetCountSchemaFunctionFromType(rawType)}({fieldNumber})";
                case ContainedTypeCategory.Enum:
                    return $"{schemaObject}.GetEnumCount({fieldNumber})";
                case ContainedTypeCategory.UserDefined:
                    return $"{schemaObject}.GetObjectCount({fieldNumber})";
                default:
                    throw new ArgumentOutOfRangeException("Unknown type category encountered.");
            }
        }

        public string GetFieldIndexExpression(string schemaObject, int fieldNumber, string index)
        {
            switch (category)
            {
                case ContainedTypeCategory.Primitive:
                    return
                        $"{schemaObject}.{SchemaFunctionMappings.IndexSchemaFunctionFromType(rawType)}({fieldNumber}, (uint) {index})";
                case ContainedTypeCategory.Enum:
                    return $"({Type}) {schemaObject}.IndexEnum({fieldNumber}, (uint) {index})";
                case ContainedTypeCategory.UserDefined:
                    return $"{Type}.Serialization.Deserialize({schemaObject}.IndexObject({fieldNumber}, (uint) {index}))";
                default:
                    throw new ArgumentOutOfRangeException("Unknown type category encountered.");
            }
        }
    }
}
