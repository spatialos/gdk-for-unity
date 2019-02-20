using System;
using System.Collections.ObjectModel;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityFieldDetails
    {
        public string Type => fieldType.Type;

        public string PascalCaseName { get; }
        public string CamelCaseName { get; }
        public uint FieldNumber { get; }

        public bool IsBlittable { get; }
        public bool CanBeEmpty { get; }

        private IFieldType fieldType;

        public UnityFieldDetails(Field field, DetailsStore store)
        {
            PascalCaseName = Formatting.SnakeCaseToPascalCase(field.Identifier.Name);
            CamelCaseName = Formatting.PascalCaseToCamelCase(PascalCaseName);
            FieldNumber = field.FieldId;

            IsBlittable = store.BlittableMap.Contains(field.Identifier);

            if (field.Option != null)
            {
                CanBeEmpty = true;
                fieldType = new OptionFieldType(field.Option.InnerType, store);
            }
            else if (field.List != null)
            {
                CanBeEmpty = true;
                fieldType = new ListFieldType(field.List.InnerType, store);
            }
            else if (field.Map != null)
            {
                CanBeEmpty = true;
                fieldType = new MapFieldType(field.Map.KeyType, field.Map.ValueType, store);
            }
            else
            {
                var singularType = field.Singular.Type;
                fieldType = new SingularFieldType(singularType, store);
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
            return fieldType.GetDeserializeDataIntoUpdateString(updateFieldInstance, schemaObject, FieldNumber,
                indents);
        }

        public string GetTrySetClearedFieldString(string fieldInstance, string componentUpdateSchemaObj, int indents)
        {
            return fieldType.GetTrySetClearedFieldString(fieldInstance, componentUpdateSchemaObj, FieldNumber, indents);
        }
    }
}
