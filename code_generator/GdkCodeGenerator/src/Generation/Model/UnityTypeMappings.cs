using System.Collections.Generic;
using Improbable.CodeGeneration.Model;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityTypeMappings
    {
        public const string PackagePrefix = "Generated.";

        public static readonly Dictionary<string, string> BuiltInSchemaTypeToUnityNativeType =
            new Dictionary<string, string>
            {
                { BuiltInTypeConstants.builtInDouble, "double" },
                { BuiltInTypeConstants.builtInFloat, "float" },
                { BuiltInTypeConstants.builtInInt32, "int" },
                { BuiltInTypeConstants.builtInInt64, "long" },
                { BuiltInTypeConstants.builtInUint32, "uint" },
                { BuiltInTypeConstants.builtInUint64, "ulong" },
                { BuiltInTypeConstants.builtInSint32, "int" },
                { BuiltInTypeConstants.builtInSint64, "long" },
                { BuiltInTypeConstants.builtInFixed32, "uint" },
                { BuiltInTypeConstants.builtInFixed64, "ulong" },
                { BuiltInTypeConstants.builtInSfixed32, "int" },
                { BuiltInTypeConstants.builtInSfixed64, "long" },
                { BuiltInTypeConstants.builtInBool, "BlittableBool" },
                { BuiltInTypeConstants.builtInString, "string" },
                { BuiltInTypeConstants.builtInBytes, "global::Improbable.Worker.Bytes" },
                { BuiltInTypeConstants.builtInEntityId, "long" },
                { BuiltInTypeConstants.builtInCoordinates, "global::Generated.Improbable.Coordinates" },
                { BuiltInTypeConstants.builtInVector3d, "global::Generated.Improbable.Vector3d" },
                { BuiltInTypeConstants.builtInVector3f, "global::Generated.Improbable.Vector3f" }
            };

        public static readonly HashSet<string> SchemaTypesThatRequireNoConversion = new HashSet<string>
        {
            BuiltInTypeConstants.builtInDouble,
            BuiltInTypeConstants.builtInFloat,
            BuiltInTypeConstants.builtInInt32,
            BuiltInTypeConstants.builtInInt64,
            BuiltInTypeConstants.builtInUint32,
            BuiltInTypeConstants.builtInUint64,
            BuiltInTypeConstants.builtInSint32,
            BuiltInTypeConstants.builtInSint64,
            BuiltInTypeConstants.builtInFixed32,
            BuiltInTypeConstants.builtInFixed64,
            BuiltInTypeConstants.builtInSfixed32,
            BuiltInTypeConstants.builtInSfixed64,
            BuiltInTypeConstants.builtInString,
            BuiltInTypeConstants.builtInBool
        };

        public static string GetNativeTypeMethod(FieldDefinitionRaw rawFieldType, string spatialOSTypeObject,
            HashSet<string> enumSet)
        {
            if (rawFieldType.IsList())
            {
                var containedType = rawFieldType.listType.valueType;

                var containedTypeAndConversion =
                    GetNativeConversionAndNativeType(containedType.TypeName, "internalObject", enumSet);
                var containedTypeConversion = containedTypeAndConversion.Conversion;

                if (SchemaTypesThatRequireNoConversion.Contains(containedType.TypeName))
                {
                    return string.Format("{0}", spatialOSTypeObject);
                }

                return string.Format("{0}.Select(internalObject => {1}).ToList()", spatialOSTypeObject,
                    containedTypeConversion);
            }

            if (rawFieldType.IsOption())
            {
                var containedType = rawFieldType.optionType.valueType;

                var containedTypeAndConversion =
                    GetNativeConversionAndNativeType(containedType.TypeName, spatialOSTypeObject + ".Value", enumSet);
                var containedTypeQualifiedName = containedTypeAndConversion.TypeName;
                var containedTypeConversion = containedTypeAndConversion.Conversion;

                return string.Format(
                    "{0}.HasValue ? new global::System.Nullable<{1}>({2}) : new global::System.Nullable<{1}>()",
                    spatialOSTypeObject,
                    containedTypeQualifiedName, containedTypeConversion);
            }

            if (rawFieldType.IsMap())
            {
                var keyTypeAndConversion =
                    GetNativeConversionAndNativeType(rawFieldType.mapType.keyType.TypeName, "entry.Key", enumSet);
                var keyFullQualifiedName = keyTypeAndConversion.TypeName;
                var keyTypeConversion = keyTypeAndConversion.Conversion;

                var valueTypeAndConversion =
                    GetNativeConversionAndNativeType(rawFieldType.mapType.valueType.TypeName, "entry.Value", enumSet);
                var valueFullQualifiedName = valueTypeAndConversion.TypeName;
                var valueTypeConversion = valueTypeAndConversion.Conversion;

                return string.Format("{0}.ToDictionary(entry => {1}, entry => {2})", spatialOSTypeObject,
                    string.Format(keyTypeConversion, keyFullQualifiedName),
                    string.Format(valueTypeConversion, valueFullQualifiedName));
            }

            if (rawFieldType.singularType.TypeName == BuiltInTypeConstants.builtInEntityId)
            {
                return string.Format("{0}.Id", spatialOSTypeObject);
            }

            if (enumSet.Contains(rawFieldType.singularType.TypeName))
            {
                return GetEnumConversion(GetFullyQualifiedGeneratedTypeName(rawFieldType.singularType.TypeName),
                    spatialOSTypeObject);
            }

            if (SchemaTypesThatRequireNoConversion.Contains(rawFieldType.singularType.TypeName))
            {
                return string.Format("{0}", spatialOSTypeObject);
            }

            return string.Format("{0}.ToNative({1})",
                GetFullyQualifiedGeneratedTypeName(rawFieldType.singularType.TypeName), spatialOSTypeObject);
        }

        public static string GetSpatialTypeMethod(FieldDefinitionRaw rawFieldType, string nativeTypeObject,
            HashSet<string> enumSet)
        {
            if (rawFieldType.IsList())
            {
                var containedType = rawFieldType.listType.valueType;

                var containedTypeAndConversion =
                    GetSpatialConversionAndSpatialType(containedType.TypeName, "nativeInternalObject", enumSet);
                var containedTypeName = containedTypeAndConversion.TypeName;
                var containedTypeConversion = containedTypeAndConversion.Conversion;

                if (SchemaTypesThatRequireNoConversion.Contains(containedType.TypeName))
                {
                    return string.Format("new global::Improbable.Collections.List<{0}>({1})", containedTypeName,
                        nativeTypeObject);
                }

                return string.Format(
                    "new global::Improbable.Collections.List<{0}>({1}.Select(nativeInternalObject => {2}))",
                    containedTypeName, nativeTypeObject, containedTypeConversion);
            }

            if (rawFieldType.IsOption())
            {
                var containedTypeAndConversion =
                    GetSpatialConversionAndSpatialType(rawFieldType.optionType.valueType.TypeName,
                        nativeTypeObject + ".Value",
                        enumSet);
                var containedTypeName = containedTypeAndConversion.TypeName;
                var containedTypeConversion = containedTypeAndConversion.Conversion;

                return string.Format(
                    "{0}.HasValue ? new global::Improbable.Collections.Option<{1}>({2}) : new global::Improbable.Collections.Option<{1}>()",
                    nativeTypeObject, containedTypeName,
                    containedTypeConversion);
            }

            if (rawFieldType.IsMap())
            {
                var keyTypeAndConversion =
                    GetSpatialConversionAndSpatialType(rawFieldType.mapType.keyType.TypeName, "entry.Key", enumSet);
                var keyFullQualifiedName = keyTypeAndConversion.TypeName;
                var keyTypeConversion = keyTypeAndConversion.Conversion;

                var valueTypeAndConversion =
                    GetSpatialConversionAndSpatialType(rawFieldType.mapType.valueType.TypeName, "entry.Value", enumSet);
                var valueFullQualifiedName = valueTypeAndConversion.TypeName;
                var valueTypeConversion = valueTypeAndConversion.Conversion;

                return string.Format(
                    "new global::Improbable.Collections.Map<{0},{1}>({2}.ToDictionary(entry => {3}, entry => {4}))",
                    keyFullQualifiedName, valueFullQualifiedName, nativeTypeObject,
                    keyTypeConversion,
                    valueTypeConversion);
            }

            if (enumSet.Contains(rawFieldType.singularType.TypeName))
            {
                return GetEnumConversion(GetFullyQualifiedSpatialTypeName(rawFieldType.singularType.TypeName),
                    nativeTypeObject);
            }

            if (rawFieldType.singularType.TypeName == BuiltInTypeConstants.builtInEntityId)
            {
                return string.Format("new global::Improbable.EntityId({0})", nativeTypeObject);
            }

            if (SchemaTypesThatRequireNoConversion.Contains(rawFieldType.singularType.TypeName))
            {
                return string.Format("{0}", nativeTypeObject);
            }

            return string.Format("{0}.ToSpatial({1})",
                GetFullyQualifiedGeneratedTypeName(rawFieldType.singularType.TypeName), nativeTypeObject);
        }

        private static string GetFullyQualifiedGeneratedTypeName(string typeName)
        {
            return "global::Generated." + Formatting.CapitaliseQualifiedNameParts(typeName);
        }

        private static string GetFullyQualifiedSpatialTypeName(string typeName)
        {
            return "global::" + Formatting.CapitaliseQualifiedNameParts(typeName);
        }

        private static TypeAndConversion GetNativeConversionAndNativeType(string typeName, string valueName,
            HashSet<string> enumSet)
        {
            if (SchemaTypesThatRequireNoConversion.Contains(typeName))
            {
                return new TypeAndConversion
                {
                    TypeName = BuiltInSchemaTypeToUnityNativeType[typeName],
                    Conversion = valueName
                };
            }

            if (typeName == BuiltInTypeConstants.builtInEntityId)
            {
                return new TypeAndConversion
                {
                    TypeName = BuiltInSchemaTypeToUnityNativeType[typeName],
                    Conversion = valueName + ".Id"
                };
            }

            if (enumSet.Contains(typeName))
            {
                var qualifiedName = GetFullyQualifiedGeneratedTypeName(typeName);
                return new TypeAndConversion
                {
                    TypeName = qualifiedName,
                    Conversion = GetEnumConversion(qualifiedName, valueName)
                };
            }

            {
                var qualifiedName = GetFullyQualifiedGeneratedTypeName(typeName);
                return new TypeAndConversion
                {
                    TypeName = qualifiedName,
                    Conversion = string.Format("{0}.ToNative({1})", qualifiedName, valueName)
                };
            }
        }

        private static TypeAndConversion GetSpatialConversionAndSpatialType(string typeName, string valueName,
            HashSet<string> enumSet)
        {
            if (SchemaTypesThatRequireNoConversion.Contains(typeName))
            {
                return new TypeAndConversion
                {
                    TypeName = BuiltInSchemaTypeToUnityNativeType[typeName],
                    Conversion = valueName
                };
            }

            if (typeName == BuiltInTypeConstants.builtInEntityId)
            {
                var qualifiedName = "global::Improbable.EntityId";
                return new TypeAndConversion
                {
                    TypeName = qualifiedName,
                    Conversion = string.Format("new {0}({1})", qualifiedName, valueName)
                };
            }

            if (enumSet.Contains(typeName))
            {
                var qualifiedName = GetFullyQualifiedSpatialTypeName(typeName);
                return new TypeAndConversion
                {
                    TypeName = qualifiedName,
                    Conversion = GetEnumConversion(qualifiedName, valueName)
                };
            }

            {
                return new TypeAndConversion
                {
                    TypeName = GetFullyQualifiedSpatialTypeName(typeName),
                    Conversion = string.Format("{0}.ToSpatial({1})", GetFullyQualifiedGeneratedTypeName(typeName),
                        valueName)
                };
            }
        }

        internal struct TypeAndConversion
        {
            public string TypeName;
            public string Conversion;
        }

        private static string GetEnumConversion(string enumToCastTo, string enumName)
        {
            return string.Format("({0}) {1}", enumToCastTo, enumName);
        }
    }
}
