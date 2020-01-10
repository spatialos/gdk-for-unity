using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class SingularFieldType : IFieldType
    {
        public string Type => containedType.FqnType;

        private readonly ContainedType containedType;

        public SingularFieldType(TypeReference innerType, DetailsStore store)
        {
            containedType = new ContainedType(innerType);
        }

        public string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return containedType.GetSerializationStatement(fieldInstance, schemaObject, fieldNumber);
        }

        public string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return $"{fieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};";
        }

        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"{containedType.GetCountExpression(schemaObject, fieldNumber)} == 1", then =>
            {
                then.WriteLine($"{fieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
            }).Format();
        }

        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return new IfElseBlock($"{containedType.GetCountExpression(schemaObject, fieldNumber)} == 1", then =>
            {
                then.WriteLine($"{updateFieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};");
            }).Format();
        }

        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber)
        {
            return $"{updateFieldInstance} = {containedType.GetDeserializationExpression(schemaObject, fieldNumber)};";
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber)
        {
            return "";
        }
    }
}
