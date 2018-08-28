using System.Collections.Generic;
using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityFieldDetails
    {
        public struct FieldTypeInfo
        {
            public TypeInfo? SingularTypeInfo;
            public TypeInfo? ListTypeInfo;
            public TypeInfo? OptionTypeInfo;
            public KeyValuePair<TypeInfo, TypeInfo> MapTypeInfo;
        }

        public struct TypeInfo
        {
            public bool IsPrimitive;
            public bool IsEnum;
            public string Type;
            public SerializationFunctionStrings SerializationFunctions;
        }

        // NOTE: These will be context dependent! If the type is primitive,
        // these are called like SchemaObject.<function>(field_number). If
        // the type is user defined, it will be called like:
        // global::<type>.<function>(SchemaObject, field_number)
        public struct SerializationFunctionStrings
        {
            public string Serialize;
            public string Deserialize;
            public string GetCount;
            public string DeserializeIndex;
        }

        public string Type;
        public FieldTypeInfo SchemaTypeInfo;
        public string PascalCaseName;
        public string CamelCaseName;
        public FieldDefinitionRaw RawFieldDefinition;
        public int FieldNumber;
        public bool IsBlittable;

        public UnityFieldDetails(FieldDefinitionRaw rawFieldDefinition, bool isBlittable, HashSet<string> enumSet)
        {
            PascalCaseName = Formatting.SnakeCaseToCapitalisedCamelCase(rawFieldDefinition.name);
            CamelCaseName = Formatting.SnakeCaseToCamelCase(rawFieldDefinition.name);
            var packagePrefix = UnityTypeMappings.PackagePrefix;
            RawFieldDefinition = rawFieldDefinition;
            FieldNumber = rawFieldDefinition.Number;
            IsBlittable = isBlittable;

            if (rawFieldDefinition.IsOption())
            {
                var valueType = rawFieldDefinition.optionType.valueType;
                var containedType = GetTypeFromTypeReference(valueType, packagePrefix);
                var isBuiltInRefType = valueType.TypeName == BuiltInTypeConstants.builtInBytes ||
                    valueType.TypeName == BuiltInTypeConstants.builtInString;
                Type = isBuiltInRefType
                    ? string.Format("global::Improbable.Gdk.Core.Option<{0}>", containedType)
                    : string.Format("global::System.Nullable<{0}>", containedType);
                SchemaTypeInfo = new FieldTypeInfo
                {
                    OptionTypeInfo = new TypeInfo
                    {
                        IsPrimitive =
                            SchemaFunctionMappings.BuiltInTypeToAddSchemaFunction.ContainsKey(valueType.TypeName),
                        IsEnum = enumSet.Contains(valueType.TypeName),
                        Type = containedType,
                        SerializationFunctions = new SerializationFunctionStrings
                        {
                            Serialize = GetSerializationFunctionFromType(valueType,
                                packagePrefix),
                            Deserialize =
                                GetDeserializationFunctionFromType(valueType,
                                    packagePrefix),
                            GetCount = GetSchemaCountFunctionFromType(valueType, enumSet),
                            DeserializeIndex = GetDeserializeIndexFunctionFromType(valueType)
                        }
                    }
                };
            }
            else if (rawFieldDefinition.IsList())
            {
                var valueType = rawFieldDefinition.listType.valueType;
                var containedType = GetTypeFromTypeReference(valueType, packagePrefix);
                Type = string.Format("global::System.Collections.Generic.List<{0}>", containedType);
                SchemaTypeInfo = new FieldTypeInfo
                {
                    ListTypeInfo = new TypeInfo
                    {
                        IsPrimitive =
                            SchemaFunctionMappings.BuiltInTypeToAddSchemaFunction.ContainsKey(valueType.TypeName),
                        IsEnum = enumSet.Contains(valueType.TypeName),
                        Type = containedType,
                        SerializationFunctions = new SerializationFunctionStrings
                        {
                            Serialize = GetSerializationFunctionFromType(valueType,
                                packagePrefix),
                            Deserialize =
                                GetDeserializationFunctionFromType(valueType,
                                    packagePrefix),
                            GetCount = GetSchemaCountFunctionFromType(valueType, enumSet),
                            DeserializeIndex = GetDeserializeIndexFunctionFromType(valueType)
                        }
                    }
                };
            }
            else if (rawFieldDefinition.IsMap())
            {
                var keyType = rawFieldDefinition.mapType.keyType;
                var valueType = rawFieldDefinition.mapType.valueType;

                var containedKeyType = GetTypeFromTypeReference(keyType, packagePrefix);
                var containedValueType = GetTypeFromTypeReference(valueType, packagePrefix);
                Type = string.Format("global::System.Collections.Generic.Dictionary<{0}, {1}>", containedKeyType,
                    containedValueType);
                SchemaTypeInfo = new FieldTypeInfo
                {
                    MapTypeInfo = new KeyValuePair<TypeInfo, TypeInfo>(
                        new TypeInfo
                        {
                            IsPrimitive =
                                SchemaFunctionMappings.BuiltInTypeToAddSchemaFunction.ContainsKey(keyType.TypeName),
                            IsEnum = enumSet.Contains(keyType.TypeName),
                            Type = containedKeyType,
                            SerializationFunctions = new SerializationFunctionStrings
                            {
                                Serialize = GetSerializationFunctionFromType(keyType,
                                    packagePrefix),
                                Deserialize =
                                    GetDeserializationFunctionFromType(keyType,
                                        packagePrefix),
                                GetCount = GetSchemaCountFunctionFromType(keyType, enumSet),
                                DeserializeIndex = GetDeserializeIndexFunctionFromType(keyType)
                            }
                        },
                        new TypeInfo
                        {
                            IsPrimitive =
                                SchemaFunctionMappings.BuiltInTypeToAddSchemaFunction.ContainsKey(valueType.TypeName),
                            IsEnum = enumSet.Contains(valueType.TypeName),
                            Type = containedValueType,
                            SerializationFunctions = new SerializationFunctionStrings
                            {
                                Serialize = GetSerializationFunctionFromType(valueType,
                                    packagePrefix),
                                Deserialize =
                                    GetDeserializationFunctionFromType(valueType,
                                        packagePrefix),
                                GetCount = GetSchemaCountFunctionFromType(valueType, enumSet),
                                DeserializeIndex = GetDeserializeIndexFunctionFromType(valueType)
                            }
                        }
                    )
                };
            }
            else
            {
                Type = GetTypeFromTypeReference(rawFieldDefinition.singularType, packagePrefix);
                SchemaTypeInfo = new FieldTypeInfo
                {
                    SingularTypeInfo = new TypeInfo
                    {
                        IsPrimitive =
                            SchemaFunctionMappings.BuiltInTypeToAddSchemaFunction.ContainsKey(rawFieldDefinition
                                .singularType.TypeName),
                        IsEnum = enumSet.Contains(rawFieldDefinition.singularType.TypeName),
                        Type = Type,
                        SerializationFunctions = new SerializationFunctionStrings
                        {
                            Serialize =
                                GetSerializationFunctionFromType(rawFieldDefinition.singularType, packagePrefix),
                            Deserialize =
                                GetDeserializationFunctionFromType(rawFieldDefinition.singularType, packagePrefix),
                            GetCount = GetSchemaCountFunctionFromType(rawFieldDefinition.singularType, enumSet),
                            DeserializeIndex = GetDeserializeIndexFunctionFromType(rawFieldDefinition.singularType)
                        }
                    }
                };
            }
        }

        private static string GetTypeFromTypeReference(TypeReferenceRaw typeReference, string packagePrefix)
        {
            return typeReference.IsBuiltInType
                ? UnityTypeMappings.BuiltInSchemaTypeToUnityNativeType[typeReference.TypeName]
                : string.Format("global::{0}{1}", packagePrefix,
                    Formatting.CapitaliseQualifiedNameParts(typeReference.TypeName));
        }

        private static string GetSerializationFunctionFromType(TypeReferenceRaw typeReference, string packagePrefix)
        {
            return SchemaFunctionMappings.BuiltInTypeToAddSchemaFunction.ContainsKey(typeReference.TypeName)
                ? SchemaFunctionMappings.BuiltInTypeToAddSchemaFunction[typeReference.TypeName]
                : string.Format("{0}.Serialization.Serialize", GetTypeFromTypeReference(typeReference, packagePrefix));
        }

        private static string GetDeserializationFunctionFromType(TypeReferenceRaw typeReference, string packagePrefix)
        {
            return SchemaFunctionMappings.BuiltInTypeToGetSchemaFunction.ContainsKey(typeReference.TypeName)
                ? SchemaFunctionMappings.BuiltInTypeToGetSchemaFunction[typeReference.TypeName]
                : string.Format("{0}.Serialization.Deserialize",
                    GetTypeFromTypeReference(typeReference, packagePrefix));
        }

        private static string GetSchemaCountFunctionFromType(TypeReferenceRaw typeReference, HashSet<string> enumSet)
        {
            return SchemaFunctionMappings.BuiltInTypeToGetCountSchemaFunction.ContainsKey(typeReference.TypeName)
                ? SchemaFunctionMappings.BuiltInTypeToGetCountSchemaFunction[typeReference.TypeName]
                : (enumSet.Contains(typeReference.TypeName) ? "GetEnumCount" : "GetObjectCount");
        }

        private static string GetDeserializeIndexFunctionFromType(TypeReferenceRaw typeReference)
        {
            return SchemaFunctionMappings.BuiltInTypeToIndexSchemaFunction.ContainsKey(typeReference.TypeName)
                ? SchemaFunctionMappings.BuiltInTypeToIndexSchemaFunction[typeReference.TypeName]
                : null;
        }
    }
}
