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

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"{fieldInstance}.HasValue", then =>
            {
                then.WriteLine(containedType.GetSerializationStatement($"{fieldInstance}.Value", schemaObject, fieldNumber));
            }).Format();
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"{containedType.GetCountExpression(schemaObject, fieldNumber)} == 1", then =>
            {
                then.WriteLine($"{fieldInstance} = new {Type}({containedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
            }).Format();
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.WriteLine(CommonDetailsUtils.WriteCheckIsCleared(fieldNumber));

                scope.If("isCleared", then =>
                    {
                        then.WriteLine($"{fieldInstance} = new {Type}();");
                    })
                    .ElseIf($"{containedType.GetCountExpression(schemaObject, fieldNumber)} == 1", then =>
                    {
                        then.WriteLine(new[]
                        {
                            $"var value = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};",
                            $"{fieldInstance} = new {Type}(value);"
                        });
                    });
            }).Format();
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.WriteLine(CommonDetailsUtils.WriteCheckIsCleared(fieldNumber));

                scope.If("isCleared",
                    then =>
                    {
                        then.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>(new {Type}());");
                    }).ElseIf($"{containedType.GetCountExpression(schemaObject, fieldNumber)} == 1",
                    then =>
                    {
                        then.WriteLine($"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>({containedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
                    });
            }).Format();
        }

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return new CustomScopeBlock(scope =>
            {
                scope.If($"{containedType.GetCountExpression(schemaObject, fieldNumber)} == 1",
                    then =>
                    {
                        then.WriteLine(
                            $"{updateFieldInstance} = new global::Improbable.Gdk.Core.Option<{Type}>({containedType.GetDeserializationExpression(schemaObject, fieldNumber)});");
                    });
            }).Format();
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"!{fieldInstance}.HasValue", then =>
            {
                then.WriteLine($"{componentUpdateSchemaObject}.AddClearedField({fieldNumber});");
            }).Format();
        }
    }
}
