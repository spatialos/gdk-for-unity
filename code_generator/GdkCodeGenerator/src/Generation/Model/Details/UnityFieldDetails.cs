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
        }

        public string Type;
        public FieldTypeInfo SchemaTypeInfo;
        public string PascalCaseName;
        public string CamelCaseName;
        public FieldDefinitionRaw RawFieldDefinition;
        public int FieldNumber;

        public UnityFieldDetails(FieldDefinitionRaw rawFieldDefinition, HashSet<string> enumSet)
        {
            PascalCaseName = Formatting.SnakeCaseToCapitalisedCamelCase(rawFieldDefinition.name);
            CamelCaseName = Formatting.SnakeCaseToCamelCase(rawFieldDefinition.name);
            var packagePrefix = UnityTypeMappings.PackagePrefix;
            RawFieldDefinition = rawFieldDefinition;
            FieldNumber = rawFieldDefinition.Number;

            if (rawFieldDefinition.IsOption())
            {
                var containedType = GetTypeFromTypeReference(rawFieldDefinition.optionType.valueType, packagePrefix);
                Type = string.Format("global::System.Nullable<{0}>", containedType);
                SchemaTypeInfo = new FieldTypeInfo
                {
                    OptionTypeInfo = new TypeInfo
                    {
                        IsPrimitive =
                            SchemaFunctionMappings.PrimitiveTypeToAddSchemaFunction.ContainsKey(rawFieldDefinition
                                .optionType.valueType.TypeName),
                        IsEnum = enumSet.Contains(rawFieldDefinition.optionType.valueType.TypeName),
                        Type = containedType,
                        SerializationFunctions = new SerializationFunctionStrings
                        {
                            Serialize = GetSerializationFunctionFromType(rawFieldDefinition.optionType.valueType,
                                packagePrefix),
                            Deserialize =
                                GetDeserializationFunctionFromType(rawFieldDefinition.optionType.valueType,
                                    packagePrefix),
                            GetCount = GetSchemaCountFunctionFromType(rawFieldDefinition.optionType.valueType)
                        }
                    }
                };
            }
            else if (rawFieldDefinition.IsList())
            {
                var containedType = GetTypeFromTypeReference(rawFieldDefinition.listType.valueType, packagePrefix);
                Type = string.Format("global::System.Collections.Generic.List<{0}>", containedType);
                SchemaTypeInfo = new FieldTypeInfo
                {
                    ListTypeInfo = new TypeInfo
                    {
                        IsPrimitive =
                            SchemaFunctionMappings.PrimitiveTypeToAddSchemaFunction.ContainsKey(rawFieldDefinition
                                .listType.valueType.TypeName),
                        IsEnum = enumSet.Contains(rawFieldDefinition.listType.valueType.TypeName),
                        Type = containedType,
                        SerializationFunctions = new SerializationFunctionStrings
                        {
                            Serialize = GetSerializationFunctionFromType(rawFieldDefinition.listType.valueType,
                                packagePrefix),
                            Deserialize =
                                GetDeserializationFunctionFromType(rawFieldDefinition.listType.valueType,
                                    packagePrefix),
                            GetCount = GetSchemaCountFunctionFromType(rawFieldDefinition.listType.valueType)
                        }
                    }
                };
            }
            else if (rawFieldDefinition.IsMap())
            {
                var containedKeyType = GetTypeFromTypeReference(rawFieldDefinition.mapType.keyType, packagePrefix);
                var containedValueType = GetTypeFromTypeReference(rawFieldDefinition.mapType.valueType, packagePrefix);
                Type = string.Format("global::System.Collections.Generic.Dictionary<{0}, {1}>", containedKeyType,
                    containedValueType);
                SchemaTypeInfo = new FieldTypeInfo
                {
                    MapTypeInfo = new KeyValuePair<TypeInfo, TypeInfo>(
                        new TypeInfo
                        {
                            IsPrimitive =
                                SchemaFunctionMappings.PrimitiveTypeToAddSchemaFunction.ContainsKey(rawFieldDefinition
                                    .mapType.keyType.TypeName),
                            IsEnum = enumSet.Contains(rawFieldDefinition.mapType.keyType.TypeName),
                            Type = containedKeyType,
                            SerializationFunctions = new SerializationFunctionStrings
                            {
                                Serialize = GetSerializationFunctionFromType(rawFieldDefinition.mapType.keyType,
                                    packagePrefix),
                                Deserialize =
                                    GetDeserializationFunctionFromType(rawFieldDefinition.mapType.keyType,
                                        packagePrefix),
                                GetCount = GetSchemaCountFunctionFromType(rawFieldDefinition.mapType.keyType)
                            }
                        },
                        new TypeInfo
                        {
                            IsPrimitive =
                                SchemaFunctionMappings.PrimitiveTypeToAddSchemaFunction.ContainsKey(rawFieldDefinition
                                    .mapType.valueType.TypeName),
                            IsEnum = enumSet.Contains(rawFieldDefinition.mapType.valueType.TypeName),
                            Type = containedValueType,
                            SerializationFunctions = new SerializationFunctionStrings
                            {
                                Serialize = GetSerializationFunctionFromType(rawFieldDefinition.mapType.valueType,
                                    packagePrefix),
                                Deserialize =
                                    GetDeserializationFunctionFromType(rawFieldDefinition.mapType.valueType,
                                        packagePrefix),
                                GetCount = GetSchemaCountFunctionFromType(rawFieldDefinition.mapType.valueType)
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
                            SchemaFunctionMappings.PrimitiveTypeToAddSchemaFunction.ContainsKey(rawFieldDefinition
                                .singularType.TypeName),
                        IsEnum = enumSet.Contains(rawFieldDefinition.singularType.TypeName),
                        Type = Type,
                        SerializationFunctions = new SerializationFunctionStrings
                        {
                            Serialize =
                                GetSerializationFunctionFromType(rawFieldDefinition.singularType, packagePrefix),
                            Deserialize =
                                GetDeserializationFunctionFromType(rawFieldDefinition.singularType, packagePrefix),
                            GetCount = GetSchemaCountFunctionFromType(rawFieldDefinition.singularType)
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
            return SchemaFunctionMappings.PrimitiveTypeToAddSchemaFunction.ContainsKey(typeReference.TypeName)
                ? SchemaFunctionMappings.PrimitiveTypeToAddSchemaFunction[typeReference.TypeName]
                : string.Format("{0}.Serialization.Serialize", GetTypeFromTypeReference(typeReference, packagePrefix));
        }

        private static string GetDeserializationFunctionFromType(TypeReferenceRaw typeReference, string packagePrefix)
        {
            return SchemaFunctionMappings.PrimitiveTypeToGetSchemaFunction.ContainsKey(typeReference.TypeName)
                ? SchemaFunctionMappings.PrimitiveTypeToGetSchemaFunction[typeReference.TypeName]
                : string.Format("{0}.Serialization.Deserialize",
                    GetTypeFromTypeReference(typeReference, packagePrefix));
        }

        private static string GetSchemaCountFunctionFromType(TypeReferenceRaw typeReference)
        {
            return SchemaFunctionMappings.PrimitiveTypeToGetCountSchemaFunction.ContainsKey(typeReference.TypeName)
                ? SchemaFunctionMappings.PrimitiveTypeToGetCountSchemaFunction[typeReference.TypeName]
                : "GetObjectCount";
        }
    }
}
