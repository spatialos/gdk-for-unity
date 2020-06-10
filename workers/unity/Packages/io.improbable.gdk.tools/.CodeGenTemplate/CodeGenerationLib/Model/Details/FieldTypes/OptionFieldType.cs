using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class OptionFieldType : IFieldType
    {
        public string Type
        {
            get
            {
                if (!IsNullable)
                {
                    return $"global::Improbable.Gdk.Core.Option<{ContainedType.FqnType}>";
                }

                return $"{ContainedType.FqnType}?";
            }
        }

        public readonly ContainedType ContainedType;
        public readonly bool IsNullable;

        public OptionFieldType(TypeReference innerType)
        {
            ContainedType = new ContainedType(innerType);
            IsNullable = ContainedType.FqnType != UnityTypeMappings.SchemaTypeToUnityType[PrimitiveType.String] && ContainedType.FqnType != UnityTypeMappings.SchemaTypeToUnityType[PrimitiveType.Bytes];
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"{fieldInstance}.HasValue", then =>
            {
                then.Line(ContainedType.GetSerializationStatement($"{fieldInstance}.Value", schemaObject, fieldNumber));
            }).Format();
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"{ContainedType.GetCountExpression(schemaObject, fieldNumber)} == 1", then =>
            {
                then.Line($"{fieldInstance} = new {Type}({ContainedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
            }).Format();
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line($"var isCleared = updateObj.IsFieldCleared({fieldNumber});");

                scope.If("isCleared", then =>
                    {
                        then.Line($"{fieldInstance} = new {Type}();");
                    })
                    .ElseIf($"{ContainedType.GetCountExpression(schemaObject, fieldNumber)} == 1", then =>
                    {
                        then.Line(new[]
                        {
                            $"var value = {ContainedType.GetDeserializationExpression(schemaObject, fieldNumber)};",
                            $"{fieldInstance} = new {Type}(value);"
                        });
                    });
            }).Format();
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.Line($"var isCleared = updateObj.IsFieldCleared({fieldNumber});");

                scope.If("isCleared",
                    then =>
                    {
                        then.Line($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
                    }).ElseIf($"{ContainedType.GetCountExpression(schemaObject, fieldNumber)} == 1",
                    then =>
                    {
                        then.Line($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>({ContainedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
                    });
            }).Format();
        }

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"{ContainedType.GetCountExpression(schemaObject, fieldNumber)} == 1",
                then =>
                {
                    then.Line(
                        $"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>({ContainedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
                }).Format();
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"!{fieldInstance}.HasValue", then =>
            {
                then.Line($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }).Format();
        }
    }
}
