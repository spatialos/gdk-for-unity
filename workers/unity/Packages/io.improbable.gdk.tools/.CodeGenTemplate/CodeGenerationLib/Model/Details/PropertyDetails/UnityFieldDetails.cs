using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGeneration.Model.Details
{
    public class UnityFieldDetails : Details
    {
        public string Type => FieldType.Type;

        private readonly uint fieldNumber;

        public readonly bool IsBlittable;
        public readonly bool CanBeEmpty;

        public readonly IFieldType FieldType;

        internal readonly FieldDefinition RawFieldDefinition;

        public UnityFieldDetails(FieldDefinition rawFieldDefinition, DetailsStore store) : base(rawFieldDefinition)
        {
            fieldNumber = rawFieldDefinition.FieldId;

            IsBlittable = store.CheckBlittable(rawFieldDefinition);

            if (rawFieldDefinition.OptionType != null)
            {
                CanBeEmpty = true;
                FieldType = new OptionFieldType(rawFieldDefinition.OptionType.InnerType);
            }
            else if (rawFieldDefinition.ListType != null)
            {
                CanBeEmpty = true;
                FieldType = new ListFieldType(rawFieldDefinition.ListType.InnerType);
            }
            else if (rawFieldDefinition.MapType != null)
            {
                CanBeEmpty = true;
                FieldType = new MapFieldType(rawFieldDefinition.MapType.KeyType, rawFieldDefinition.MapType.ValueType);
            }
            else
            {
                FieldType = new SingularFieldType(rawFieldDefinition.SingularType.Type);
            }

            RawFieldDefinition = rawFieldDefinition;
        }

        /// <summary>
        ///     Checks whether the field is a custom schema type.
        /// </summary>
        public bool IsCustomType()
        {
            return RawFieldDefinition.SingularType != null &&
                RawFieldDefinition.SingularType.Type.ValueTypeSelector == ValueType.Type;
        }

        /// <summary>
        ///     Helper function that returns a (multi-line) string that represents the C# code required to serialize
        ///     this field.
        /// </summary>
        /// <param name="fieldInstance">The name of the instance of this field that is to be serialized.</param>
        /// <param name="schemaObject">The name of the SchemaObject is to be used in serialization.</param>
        /// <param name="indents">The indent level that the block of code should be at.</param>
        public string GetSerializationString(string fieldInstance, string schemaObject, int indents)
        {
            var serializationString = FieldType.GetSerializationString(fieldInstance, schemaObject, fieldNumber);
            return CommonGeneratorUtils.IndentEveryNewline(serializationString, indents);
        }

        /// <summary>
        ///     Helper function that returns a (multi-line) string that represents the C# code required to deserialize
        ///     this field on a ComponentData object.
        /// </summary>
        /// <param name="fieldInstance">The name of the instance of this field that is being deserialized into.</param>
        /// <param name="schemaObject">The name of the SchemaObject is to be used in deserialization.</param>
        /// <param name="indents">The indent level that the block of code should be at.</param>
        public string GetDeserializeString(string fieldInstance, string schemaObject, int indents)
        {
            var deserializationString = FieldType.GetDeserializationString(fieldInstance, schemaObject, fieldNumber);
            return CommonGeneratorUtils.IndentEveryNewline(deserializationString, indents);
        }

        /// <summary>
        ///     Helper function that returns a (multi-line) string that represents the C# code required to deserialize
        ///     this field on a ComponentUpdate object.
        /// </summary>
        /// <param name="fieldInstance">The name of the instance of this field that is being deserialized into.</param>
        /// <param name="schemaObject">The name of the SchemaObject is to be used in deserialization.</param>
        /// <param name="indents">The indent level that the block of code should be at.</param>
        /// <returns></returns>
        public string GetDeserializeUpdateString(string fieldInstance, string schemaObject, int indents)
        {
            var deserializationString = FieldType.GetDeserializeUpdateString(fieldInstance, schemaObject, fieldNumber);
            return CommonGeneratorUtils.IndentEveryNewline(deserializationString, indents);
        }

        /// <summary>
        ///     Helper function that returns a (multi-line) string that represents the C# code required to deserialize
        ///     this field on a ComponentUpdate object.
        /// </summary>
        /// <param name="updateFieldInstance">
        ///     The name of the instance of this field on the update object that is being
        ///     deserialized into.
        /// </param>
        /// <param name="schemaObject">The name of the SchemaObject is to be used in deserialization.</param>
        /// <param name="indents">The indent level that the block of code should be at.</param>
        /// <returns></returns>
        public string GetDeserializeUpdateIntoUpdateString(string updateFieldInstance, string schemaObject, int indents)
        {
            var deserializationString =
                FieldType.GetDeserializeUpdateIntoUpdateString(updateFieldInstance, schemaObject, fieldNumber);
            return CommonGeneratorUtils.IndentEveryNewline(deserializationString, indents);
        }

        /// <summary>
        ///     Helper function that returns a (multi-line) string that represents the C# code required to deserialize
        ///     this field on a ComponentData object into an update.
        /// </summary>
        /// <param name="updateFieldInstance">
        ///     The name of the instance of this field on the update object that is being
        ///     deserialized into.
        /// </param>
        /// <param name="schemaObject">The name of the SchemaObject is to be used in deserialization.</param>
        /// <param name="indents">The indent level that the block of code should be at.</param>
        /// <returns></returns>
        public string GetDeserializeDataIntoUpdateString(string updateFieldInstance, string schemaObject, int indents)
        {
            var deserializationString =
                FieldType.GetDeserializeDataIntoUpdateString(updateFieldInstance, schemaObject, fieldNumber);
            return CommonGeneratorUtils.IndentEveryNewline(deserializationString, indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObj, int indents)
        {
            var trySetClearedFieldString = FieldType.GetTrySetClearedFieldString(fieldInstance, componentUpdateSchemaObj, fieldNumber);
            return CommonGeneratorUtils.IndentEveryNewline(trySetClearedFieldString, indents);
        }
    }
}
