using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     This static class marks user defined schema types and components as blittable/non-blittable.
    ///     It operates on a corpus of processed schema files and recursively collects all types and marks them as blittable.
    ///     It then iterates over all components and marks them.
    /// </summary>
    public static class BlittableFlagger
    {
        private static readonly HashSet<string> nonBlittableSchemaBuiltInTypes =
            new HashSet<string> { "string", "bytes" };

        public static void SetBlittableFlags(ICollection<UnitySchemaFile> processedSchemaFiles)
        {
            var unvisitedTypeDefinitions = new Dictionary<string, UnityTypeDefinition>();
            var enumTypes = new HashSet<string>();
            PopulateTypeDefinitions(unvisitedTypeDefinitions, enumTypes, processedSchemaFiles);
            var blittableTypeMap = CalculateBlittableTypes(unvisitedTypeDefinitions, enumTypes);
            ApplyBlittableMarkerToComponents(blittableTypeMap, processedSchemaFiles);
        }

        private static void PopulateTypeDefinitions(Dictionary<string, UnityTypeDefinition> typeMap,
            HashSet<string> enumSet, IEnumerable<UnitySchemaFile> schemaFiles)
        {
            foreach (var schemaFile in schemaFiles)
            {
                foreach (var typeDefinition in schemaFile.TypeDefinitions)
                {
                    typeMap.Add(typeDefinition.QualifiedName, typeDefinition);
                    PopulateTypeDefinitionsRecursively(typeMap, enumSet, typeDefinition);
                }

                foreach (var enumDefinition in schemaFile.EnumDefinitions)
                {
                    enumSet.Add(enumDefinition.qualifiedName);
                }
            }
        }

        private static void PopulateTypeDefinitionsRecursively(Dictionary<string, UnityTypeDefinition> typeMap,
            HashSet<string> enumSet, UnityTypeDefinition typeDefinition)
        {
            foreach (var nestedTypeDefinition in typeDefinition.TypeDefinitions)
            {
                typeMap.Add(nestedTypeDefinition.QualifiedName, nestedTypeDefinition);
                PopulateTypeDefinitionsRecursively(typeMap, enumSet, nestedTypeDefinition);
            }

            foreach (var nestedEnumDefinition in typeDefinition.EnumDefinitions)
            {
                enumSet.Add(nestedEnumDefinition.qualifiedName);
            }
        }

        private static Dictionary<string, bool> CalculateBlittableTypes(
            Dictionary<string, UnityTypeDefinition> unvisited, HashSet<string> enumSet)
        {
            var blittableTypeMap = new Dictionary<string, bool>();

            // All enums are blittable.
            foreach (var enumName in enumSet)
            {
                blittableTypeMap.Add(enumName, true);
            }

            while (unvisited.Count > 0)
            {
                ApplyBlittableMarkerToTypeRecursively(unvisited.First().Value, blittableTypeMap, unvisited);
            }

            return blittableTypeMap;
        }

        private static bool ApplyBlittableMarkerToTypeRecursively(UnityTypeDefinition typeDefinition,
            Dictionary<string, bool> blittableTypeMap, Dictionary<string, UnityTypeDefinition> unvisited)
        {
            var isBlittable = typeDefinition.FieldDefinitions.All(fieldDefinition =>
                CanBlitFieldOfTypeRecursively(fieldDefinition, blittableTypeMap, unvisited));

            typeDefinition.IsBlittable = isBlittable;
            blittableTypeMap[typeDefinition.QualifiedName] = isBlittable;
            unvisited.Remove(typeDefinition.QualifiedName);

            return isBlittable;
        }

        private static bool CanBlitFieldOfTypeRecursively(UnityFieldDefinition fieldDefinition,
            Dictionary<string, bool> blittableTypeMap, Dictionary<string, UnityTypeDefinition> unvisited)
        {
            if (!IsUserType(fieldDefinition))
            {
                return IsBuiltInBlittableType(fieldDefinition);
            }

            var fieldTypeString = fieldDefinition.RawFieldDefinition.singularType.userType;
            if (!blittableTypeMap.ContainsKey(fieldTypeString))
            {
                return ApplyBlittableMarkerToTypeRecursively(unvisited[fieldTypeString], blittableTypeMap, unvisited);
            }

            return blittableTypeMap[fieldTypeString];
        }

        private static void ApplyBlittableMarkerToComponents(Dictionary<string, bool> blittableTypeMap,
            IEnumerable<UnitySchemaFile> schemaFiles)
        {
            foreach (var schemaFile in schemaFiles)
            {
                foreach (var component in schemaFile.ComponentDefinitions)
                {
                    foreach (var field in component.DataDefinition.typeDefinition.FieldDefinitions)
                    {
                        field.IsBlittable = CanBlitFieldOfComponent(field, blittableTypeMap);
                    }

                    component.IsBlittable =
                        component.DataDefinition.typeDefinition.FieldDefinitions.All((field) => field.IsBlittable);
                }
            }
        }

        private static bool CanBlitFieldOfComponent(UnityFieldDefinition fieldDefinition,
            Dictionary<string, bool> blittableTypeMap)
        {
            if (!IsUserType(fieldDefinition))
            {
                return IsBuiltInBlittableType(fieldDefinition);
            }

            return blittableTypeMap[fieldDefinition.RawFieldDefinition.singularType.userType];
        }

        private static bool IsUserType(UnityFieldDefinition fieldDefinition)
        {
            if (fieldDefinition.IsList || fieldDefinition.IsMap || fieldDefinition.IsOption)
            {
                return false;
            }

            return !fieldDefinition.RawFieldDefinition.singularType.IsBuiltInType;
        }

        private static bool IsBuiltInBlittableType(UnityFieldDefinition fieldDefinition)
        {
            if (fieldDefinition.IsList || fieldDefinition.IsMap || fieldDefinition.IsOption)
            {
                return false;
            }

            return !nonBlittableSchemaBuiltInTypes.Contains(fieldDefinition.RawFieldDefinition.singularType
                .builtInType);
        }
    }
}
