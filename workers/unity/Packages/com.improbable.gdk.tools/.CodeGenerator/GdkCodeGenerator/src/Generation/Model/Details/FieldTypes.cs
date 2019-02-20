using System;
using Improbable.Gdk.CodeGeneration.Model;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGenerator.Utils;

namespace Improbable.Gdk.CodeGenerator
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

        public SingularFieldType(Field.InnerType innerType, DetailsStore store)
        {
            containedType = new ContainedType(innerType, store);
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
            codeWriter.WriteLine($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
                codeWriter.WriteLine($"{fieldInstance} = value;");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
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

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
            codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(value);");

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
                if (containedType.Type ==
                    UnityTypeMappings.BuiltInSchemaTypeToUnityNativeType[BuiltInSchemaTypes.BuiltInString]
                    ||
                    containedType.Type ==
                    UnityTypeMappings.BuiltInSchemaTypeToUnityNativeType[BuiltInSchemaTypes.BuiltInBytes])
                {
                    return $"global::Improbable.Gdk.Core.Option<{containedType.Type}>";
                }

                return $"{containedType.Type}?";
            }
        }

        private ContainedType containedType;

        public OptionFieldType(Field.InnerType innerType, DetailsStore store)
        {
            containedType = new ContainedType(innerType, store);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"if ({fieldInstance}.HasValue)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine(containedType.GetSerializationStatement($"{fieldInstance}.Value", schemaObject, fieldNumber));
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"{fieldInstance} = new {Type}({containedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
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
            uint fieldNumber, int indents)
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

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();

            codeWriter.WriteLine($"if ({containedType.GetCountExpression(schemaObject, fieldNumber)} == 1)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
                codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}(value));");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber,
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

        public ListFieldType(Field.InnerType innerType, DetailsStore store)
        {
            containedType = new ContainedType(innerType, store);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"foreach (var value in {fieldInstance})");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine(containedType.GetSerializationStatement("value", schemaObject, fieldNumber));
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"{fieldInstance} = new {Type}();");
            codeWriter.WriteLine($"var list = {fieldInstance};");
            codeWriter.WriteLine($"var listLength = {containedType.GetCountExpression(schemaObject, fieldNumber)};");
            codeWriter.WriteLine("for (var i = 0; i < listLength; i++)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"list.Add({containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")});");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject,
            uint fieldNumber, int indents)
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
            uint fieldNumber, int indents)
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

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"var listSize = {containedType.GetCountExpression(schemaObject, fieldNumber)};");

            codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");

            codeWriter.WriteLine("for (var i = 0; i < listSize; i++)");
            using (codeWriter.Scope())
            {
                codeWriter.WriteLine($"var value = {containedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};");
                codeWriter.WriteLine($"{updateFieldInstance}.Value.Add(value);");
            }

            return CommonGeneratorUtils.IndentEveryNewline(codeWriter.Build(), indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber,
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

        public MapFieldType(Field.InnerType keyType, Field.InnerType valueType, DetailsStore store)
        {
            this.keyType = new ContainedType(keyType, store);
            this.valueType = new ContainedType(valueType, store);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
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

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();

            codeWriter.WriteLine($"{fieldInstance} = new {Type}();");
            codeWriter.WriteLine($"var map = {fieldInstance};");
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

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber, int indents)
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
            uint fieldNumber, int indents)
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

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject,
            uint fieldNumber, int indents)
        {
            var codeWriter = new CodeWriter();
            codeWriter.WriteLine($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");

            codeWriter.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");

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

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber,
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

        // TODO: This can probably be reworked with the inner type concept.
        public ContainedType(Field.InnerType innerType, DetailsStore store)
        {
            if (innerType.Primitive != null)
            {
                category = ContainedTypeCategory.Primitive;
                Type = CommonDetailsUtils.GetCapitalisedFqnTypename(innerType.Primitive);
                rawType = innerType.Primitive;
            }
            else if (innerType.UserType != null)
            {
                category = ContainedTypeCategory.UserDefined;
                Type = CommonDetailsUtils.GetCapitalisedFqnTypename(innerType.UserType.QualifiedName);
                rawType = innerType.UserType.QualifiedName;
            }
            else if (innerType.EnumType != null)
            {
                category = ContainedTypeCategory.Enum;
                Type = CommonDetailsUtils.GetCapitalisedFqnTypename(innerType.EnumType.QualifiedName);
                rawType = innerType.EnumType.QualifiedName;
            }
            else
            {
                throw new ArgumentException("Malformed inner type.");
            }
        }

        public string GetSerializationStatement(string instance, string schemaObject, uint fieldNumber)
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

        public string GetDeserializationExpression(string schemaObject, uint fieldNumber)
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

        public string GetCountExpression(string schemaObject, uint fieldNumber)
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

        public string GetFieldIndexExpression(string schemaObject, uint fieldNumber, string index)
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
