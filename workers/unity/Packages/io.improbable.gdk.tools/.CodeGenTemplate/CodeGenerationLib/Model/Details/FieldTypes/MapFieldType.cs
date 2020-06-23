using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class MapFieldType : IFieldType
    {
        public string Type => $"global::System.Collections.Generic.Dictionary<{KeyType.FqnType}, {ValueType.FqnType}>";

        public readonly ContainedType KeyType;
        public readonly ContainedType ValueType;

        public MapFieldType(TypeReference keyType, TypeReference valueType)
        {
            KeyType = new ContainedType(keyType);
            ValueType = new ContainedType(valueType);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new LoopBlock($"foreach (var keyValuePair in {fieldInstance})", body =>
            {
                body.Line(new[]
                {
                    $"var mapObj = {schemaObject}.AddObject({fieldNumber});",
                    KeyType.GetSerializationStatement("keyValuePair.Key", "mapObj", 1),
                    ValueType.GetSerializationStatement("keyValuePair.Value", "mapObj", 2)
                });
            }).Format();
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line(new[]
                {
                    $"var map = new {Type}();",
                    $"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});",
                    $"{fieldInstance} = map;"
                });

                scope.Loop("for (var i = 0; i < mapSize; i++)", body =>
                {
                    body.Line(new[]
                    {
                        $"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);",
                        $"var key = {KeyType.GetDeserializationExpression("mapObj", 1)};",
                        $"var value = {ValueType.GetDeserializationExpression("mapObj", 2)};",
                        "map.Add(key, value);"
                    });
                });
            }).Format();
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");

                scope.Line($"var isCleared = updateObj.IsFieldCleared({fieldNumber});");

                scope.If("mapSize > 0 || isCleared", then =>
                {
                    then.Line($"{fieldInstance}.Clear();");
                });

                scope.Loop("for (var i = 0; i < mapSize; i++)", body =>
                {
                    body.Line(new[]
                    {
                        $"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);",
                        $"var key = {KeyType.GetDeserializationExpression("mapObj", 1)};",
                        $"var value = {ValueType.GetDeserializationExpression("mapObj", 2)};",
                        $"{fieldInstance}.Add(key, value);"
                    });
                });
            }).Format();
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line($"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});");

                scope.Line($"var isCleared = updateObj.IsFieldCleared({fieldNumber});");

                scope.If("mapSize > 0 || isCleared",
                    then =>
                    {
                        then.Line(
                            $"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
                    });

                scope.Loop("for (var i = 0; i < mapSize; i++)", body =>
                {
                    body.Line(new[]
                    {
                        $"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);",
                        $"var key = {KeyType.GetDeserializationExpression("mapObj", 1)};",
                        $"var value = {ValueType.GetDeserializationExpression("mapObj", 2)};",
                        $"{updateFieldInstance}.Value.Add(key, value);"
                    });
                });
            }).Format();
        }

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line(new[]
                {
                    $"var map = new {Type}();",
                    $"var mapSize = {schemaObject}.GetObjectCount({fieldNumber});",
                    $"{updateFieldInstance} = map;"
                });

                scope.Loop("for (var i = 0; i < mapSize; i++)", body =>
                {
                    body.Line(new[]
                    {
                        $"var mapObj = {schemaObject}.IndexObject({fieldNumber}, (uint) i);",
                        $"var key = {KeyType.GetDeserializationExpression("mapObj", 1)};",
                        $"var value = {ValueType.GetDeserializationExpression("mapObj", 2)};",
                        "map.Add(key, value);"
                    });
                });
            }).Format();
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"{fieldInstance}.Count == 0", then =>
            {
                then.Line($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }).Format();
        }
    }
}
