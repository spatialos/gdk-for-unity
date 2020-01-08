namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public interface IFieldType
    {
        string Type { get; }
        string GetSerializationString(string fieldInstance, string schemaObject, uint fieldNumber);
        string GetDeserializationString(string fieldInstance, string schemaObject, uint fieldNumber);
        string GetDeserializeUpdateString(string fieldInstance, string schemaObject, uint fieldNumber);
        string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber);
        string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject, uint fieldNumber);
        string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObject, uint fieldNumber);
    }
}
