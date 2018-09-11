using System.Collections.Generic;
using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityFieldDetails
    {
        public string Type => fieldType.Type;
        public string PascalCaseName => Formatting.SnakeCaseToCapitalisedCamelCase(rawFieldDefinition.name);
        public string CamelCaseName => Formatting.SnakeCaseToCamelCase(rawFieldDefinition.name);
        public int FieldNumber => rawFieldDefinition.Number;
        public bool IsBlittable;
        public bool CanBeEmpty;

        private IFieldType fieldType;
        private FieldDefinitionRaw rawFieldDefinition;

        public UnityFieldDetails(FieldDefinitionRaw rawFieldDefinition, bool isBlittable, HashSet<string> enumSet)
        {
            this.rawFieldDefinition = rawFieldDefinition;
            IsBlittable = isBlittable;

            if (rawFieldDefinition.IsOption())
            {
                fieldType = new OptionFieldType(rawFieldDefinition.optionType, enumSet);
                CanBeEmpty = true;
            }
            else if (rawFieldDefinition.IsList())
            {
                fieldType = new ListFieldType(rawFieldDefinition.listType, enumSet);
                CanBeEmpty = true;
            }
            else if (rawFieldDefinition.IsMap())
            {
                fieldType = new MapFieldType(rawFieldDefinition.mapType, enumSet);
                CanBeEmpty = true;
            }
            else
            {
                fieldType = new SingularFieldType(rawFieldDefinition.singularType, enumSet);
            }
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
            return fieldType.GetSerializationString(fieldInstance, schemaObject, FieldNumber, indents);
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
            return fieldType.GetDeserializationString(fieldInstance, schemaObject, FieldNumber, indents);
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
            return fieldType.GetDeserializeUpdateString(fieldInstance, schemaObject, FieldNumber,
                indents);
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
            return fieldType.GetDeserializeUpdateIntoUpdateString(updateFieldInstance, schemaObject, FieldNumber,
                indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObj, int indents)
        {
            return fieldType.GetTrySetClearedFieldString(fieldInstance, componentUpdateSchemaObj, FieldNumber, indents);
        }
    }
}
