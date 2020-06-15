using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class ListFieldType : IFieldType
    {
        public string Type => $"global::System.Collections.Generic.List<{ContainedType.FqnType}>";

        public readonly ContainedType ContainedType;

        public ListFieldType(TypeReference innerType)
        {
            ContainedType = new ContainedType(innerType);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new LoopBlock($"foreach (var value in {fieldInstance})", body =>
            {
                body.Line(ContainedType.GetSerializationStatement("value", schemaObject, fieldNumber));
            }).Format();
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line(new[]
                {
                    $"{fieldInstance} = new {Type}();",
                    $"var list = {fieldInstance};",
                    $"var listLength = {ContainedType.GetCountExpression(schemaObject, fieldNumber)};"
                });

                scope.Loop("for (var i = 0; i < listLength; i++)", body =>
                {
                    body.Line($"list.Add({ContainedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")});");
                });
            }).Format();
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line($"var listSize = {ContainedType.GetCountExpression(schemaObject, fieldNumber)};");

                scope.Line($"var isCleared = updateObj.IsFieldCleared({fieldNumber});");

                scope.If("listSize > 0 || isCleared", then =>
                {
                    then.Line($"{fieldInstance}.Clear();");
                });

                scope.Loop("for (var i = 0; i < listSize; i++)", body =>
                {
                    body.Line(new[]
                    {
                        $"var value = {ContainedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};",
                        $"{fieldInstance}.Add(value);"
                    });
                });
            }).Format();
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line($"var listSize = {ContainedType.GetCountExpression(schemaObject, fieldNumber)};");

                scope.Line($"var isCleared = updateObj.IsFieldCleared({fieldNumber});");

                scope.If("listSize > 0 || isCleared", then =>
                {
                    then.Line(
                        $"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
                });

                scope.Loop("for (var i = 0; i < listSize; i++)", body =>
                {
                    body.Line(new[]
                    {
                        $"var value = {ContainedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};",
                        $"{updateFieldInstance}.Value.Add(value);"
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
                    $"var listSize = {ContainedType.GetCountExpression(schemaObject, fieldNumber)};",
                    $"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());"
                });

                scope.Loop("for (var i = 0; i < listSize; i++)", body =>
                {
                    body.Line(new[]
                    {
                        $"var value = {ContainedType.GetFieldIndexExpression(schemaObject, fieldNumber, "i")};",
                        $"{updateFieldInstance}.Value.Add(value);"
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
