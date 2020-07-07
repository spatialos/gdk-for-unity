using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.Model;
using Improbable.Gdk.CodeGeneration.Model.Details;
using Improbable.Gdk.CodeGeneration.Utils;
using ValueType = Improbable.Gdk.CodeGeneration.Model.ValueType;

namespace Improbable.Gdk.CodeGenerator
{
    public class FieldTypeHandler
    {
        private readonly DetailsStore detailsStore;

        public FieldTypeHandler(DetailsStore detailsStore)
        {
            this.detailsStore = detailsStore;
        }

        public string ToFieldDeclaration(UnityFieldDetails fieldDetails)
        {
            switch (fieldDetails.FieldType)
            {
                case SingularFieldType singularFieldType:
                    var uiType = GetUiFieldType(singularFieldType.ContainedType);
                    return $"private readonly {uiType} {fieldDetails.CamelCaseName}Field;";
                case OptionFieldType optionFieldType:
                    var innerUiType = GetUiFieldType(optionFieldType.ContainedType);
                    var element = optionFieldType.IsNullable ? "NullableVisualElement" : "OptionVisualElement";
                    return $"private readonly {element}<{innerUiType}, {optionFieldType.ContainedType.FqnType}> {fieldDetails.CamelCaseName}Field;";
                case ListFieldType listFieldType:
                    var innerListType = GetUiFieldType(listFieldType.ContainedType);
                    return $"private readonly PaginatedListView<{innerListType}, {listFieldType.ContainedType.FqnType}> {fieldDetails.CamelCaseName}Field;";
                case MapFieldType mapFieldType:
                    var innerKeyType = GetUiFieldType(mapFieldType.KeyType);
                    var innerValueType = GetUiFieldType(mapFieldType.ValueType);

                    return
                        $"private readonly PaginatedMapView<{innerKeyType}, {mapFieldType.KeyType.FqnType}, {innerValueType}, {mapFieldType.ValueType.FqnType}> {fieldDetails.CamelCaseName}Field;";
                default:
                    throw new ArgumentException($"Unexpected field type: {fieldDetails.FieldType.GetType()}");
            }
        }

        public IEnumerable<string> ToFieldInitialisation(UnityFieldDetails fieldDetails, string parentContainer)
        {
            switch (fieldDetails.FieldType)
            {
                case SingularFieldType singularFieldType:
                    var humanReadableName = Formatting.SnakeCaseToHumanReadable(fieldDetails.Name);

                    foreach (var initializer in GetFieldInitializer(singularFieldType.ContainedType, $"{fieldDetails.CamelCaseName}Field", humanReadableName, false))
                    {
                        yield return initializer;
                    }

                    yield return $"{parentContainer}.Add({fieldDetails.CamelCaseName}Field);";
                    break;
                case OptionFieldType optionFieldType:
                    var innerUiType = GetUiFieldType(optionFieldType.ContainedType);

                    foreach (var initializer in GetFieldInitializer(optionFieldType.ContainedType, $"{fieldDetails.CamelCaseName}InnerField", "Value"))
                    {
                        yield return initializer;
                    }

                    var element = optionFieldType.IsNullable ? "NullableVisualElement" : "OptionVisualElement";

                    yield return
                        $"{fieldDetails.CamelCaseName}Field = new {element}<{innerUiType}, {optionFieldType.ContainedType.FqnType}>(\"{Formatting.SnakeCaseToHumanReadable(fieldDetails.Name)}\", {fieldDetails.CamelCaseName}InnerField, (element, data) => {{ {ContainedTypeToUiFieldUpdate(optionFieldType.ContainedType, "element", "data")} }});";
                    yield return $"{parentContainer}.Add({fieldDetails.CamelCaseName}Field);";
                    break;
                case ListFieldType listFieldType:
                    var innerListType = GetUiFieldType(listFieldType.ContainedType);

                    yield return
                        $"{fieldDetails.CamelCaseName}Field = new PaginatedListView<{innerListType}, {listFieldType.ContainedType.FqnType}>(\"{Formatting.SnakeCaseToHumanReadable(fieldDetails.Name)}\", () => {{";

                    foreach (var initializer in GetFieldInitializer(listFieldType.ContainedType, "inner", ""))
                    {
                        yield return initializer;
                    }

                    yield return "return inner; }, (index, data, element) => {";

                    // These lines are part of the binding.
                    var labelBinding = listFieldType.ContainedType.Category == ValueType.Type ? "Label" : "label";
                    yield return $"element.{labelBinding} = $\"Item {{index + 1}}\";";
                    yield return ContainedTypeToUiFieldUpdate(listFieldType.ContainedType, "element", "data");
                    yield return "});";
                    yield return $"{parentContainer}.Add({fieldDetails.CamelCaseName}Field);";
                    break;
                case MapFieldType mapFieldType:
                    var innerKeyType = GetUiFieldType(mapFieldType.KeyType);
                    var innerValueType = GetUiFieldType(mapFieldType.ValueType);

                    yield return
                        $"{fieldDetails.CamelCaseName}Field = new PaginatedMapView<{innerKeyType}, {mapFieldType.KeyType.FqnType}, {innerValueType}, {mapFieldType.ValueType.FqnType}>(\"{Formatting.SnakeCaseToHumanReadable(fieldDetails.Name)}\",";
                    yield return "() => {";

                    foreach (var initializer in GetFieldInitializer(mapFieldType.KeyType, "inner", "Key"))
                    {
                        yield return initializer;
                    }

                    yield return $"return inner; }}, (data, element) => {{ {ContainedTypeToUiFieldUpdate(mapFieldType.KeyType, "element", "data")} }},";

                    yield return "() => {";

                    foreach (var initializer in GetFieldInitializer(mapFieldType.ValueType, "inner", "Value"))
                    {
                        yield return initializer;
                    }

                    yield return $"return inner; }}, (data, element) => {{ {ContainedTypeToUiFieldUpdate(mapFieldType.ValueType, "element", "data")} }});";
                    yield return $"{parentContainer}.Add({fieldDetails.CamelCaseName}Field);";
                    break;
                default:
                    throw new ArgumentException($"Unexpected field type: {fieldDetails.FieldType.GetType()}");
            }
        }

        public string ToUiFieldUpdate(UnityFieldDetails fieldDetails, string fieldParent)
        {
            var uiElementName = $"{fieldDetails.CamelCaseName}Field";
            switch (fieldDetails.FieldType)
            {
                case SingularFieldType singularFieldType:
                    return ContainedTypeToUiFieldUpdate(singularFieldType.ContainedType, uiElementName, $"{fieldParent}.{fieldDetails.PascalCaseName}");
                case OptionFieldType optionFieldType:
                case ListFieldType listFieldType:
                case MapFieldType mapFieldType:
                    return $"{uiElementName}.Update({fieldParent}.{fieldDetails.PascalCaseName});";
                default:
                    throw new ArgumentException($"Unexpected field type: {fieldDetails.FieldType.GetType()}");
            }
        }

        public string ToSetCollectionVisibility(UnityFieldDetails fieldDetails, string fieldParent, string booleanArg)
        {
            var uiElementName = $"{fieldDetails.CamelCaseName}Field";
            switch (fieldDetails.FieldType)
            {
                case SingularFieldType singularFieldType:
                    if (!fieldDetails.IsCustomType())
                    {
                        return string.Empty;
                    }
                    return $"{uiElementName}.SetVisibility({fieldParent}.{fieldDetails.PascalCaseName}, {booleanArg});";
                case OptionFieldType optionFieldType:
                case ListFieldType listFieldType:
                case MapFieldType mapFieldType:
                    return $"{uiElementName}.SetVisibility({fieldParent}.{fieldDetails.PascalCaseName}, {booleanArg});";
                default:
                    throw new ArgumentException($"Unexpected field type: {fieldDetails.FieldType.GetType()}");
            }
        }

        private IEnumerable<string> GetFieldInitializer(ContainedType containedType, string uiElementName, string label, bool newVariable = true)
        {
            var inner = GetUiFieldType(containedType);

            if (newVariable)
            {
                yield return $"var {uiElementName} = new {inner}(\"{label}\");";
            }
            else
            {
                yield return $"{uiElementName} = new {inner}(\"{label}\");";
            }

            // These lines are part of the func to create an inner list item.
            if (containedType.Category != ValueType.Type)
            {
                yield return $"{uiElementName}.SetEnabled(false);";
            }

            if (containedType.Category == ValueType.Enum)
            {
                yield return $"{uiElementName}.Init(default({containedType.FqnType}));";
            }
        }

        private string ContainedTypeToUiFieldUpdate(ContainedType containedType, string uiElementName, string fieldAccessor)
        {
            switch (containedType.Category)
            {
                case ValueType.Enum:
                    return $"{uiElementName}.value = {fieldAccessor};";
                case ValueType.Primitive:
                    var primitiveType = containedType.PrimitiveType.Value;
                    switch (primitiveType)
                    {
                        case PrimitiveType.Int32:
                        case PrimitiveType.Int64:
                        case PrimitiveType.Uint32:
                        case PrimitiveType.Uint64:
                        case PrimitiveType.Sint32:
                        case PrimitiveType.Sint64:
                        case PrimitiveType.Fixed32:
                        case PrimitiveType.Fixed64:
                        case PrimitiveType.Sfixed32:
                        case PrimitiveType.Sfixed64:
                        case PrimitiveType.Float:
                        case PrimitiveType.Double:
                        case PrimitiveType.String:
                        case PrimitiveType.EntityId:
                        case PrimitiveType.Entity:
                            return $"{uiElementName}.value = {fieldAccessor}.ToString();";
                        case PrimitiveType.Bytes:
                            return $"{uiElementName}.value = global::System.Text.Encoding.Default.GetString({fieldAccessor});";
                        case PrimitiveType.Bool:
                            return $"{uiElementName}.value = {fieldAccessor};";
                        case PrimitiveType.Invalid:
                            throw new ArgumentException("Unknown primitive type encountered");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case ValueType.Type:
                    return $"{uiElementName}.Update({fieldAccessor});";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetUiFieldType(ContainedType type)
        {
            switch (type.Category)
            {
                case ValueType.Enum:
                    return "EnumField";
                case ValueType.Primitive:
                    switch (type.PrimitiveType.Value)
                    {
                        case PrimitiveType.Int32:
                        case PrimitiveType.Int64:
                        case PrimitiveType.Uint32:
                        case PrimitiveType.Uint64:
                        case PrimitiveType.Sint32:
                        case PrimitiveType.Sint64:
                        case PrimitiveType.Fixed32:
                        case PrimitiveType.Fixed64:
                        case PrimitiveType.Sfixed32:
                        case PrimitiveType.Sfixed64:
                        case PrimitiveType.Float:
                        case PrimitiveType.Double:
                        case PrimitiveType.String:
                        case PrimitiveType.EntityId:
                        case PrimitiveType.Bytes:
                        case PrimitiveType.Entity:
                            return "TextField";
                        case PrimitiveType.Bool:
                            return "Toggle";
                        case PrimitiveType.Invalid:
                            throw new ArgumentException("Unknown primitive type encountered.");
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case ValueType.Type:
                    return CalculateRendererFullyQualifiedName(type);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // We generate child types inside the parent type (if we don't there will be clashes).
        // However this means to get the renderer type we can't just grab the FQN, since each intermediate type will
        // have 'Renderer' appended to it.
        //
        // We need to crawl back up the type tree until we find the most-parent type and construct the FQN from
        // this information.
        // i.e. - global::{namespace of parent type}.{parent type}Renderer.{child type}Renderer.{child type}Renderer
        private string CalculateRendererFullyQualifiedName(ContainedType type)
        {
            var typeDetails = detailsStore.Types[type.RawType];

            var rendererChain = new List<string>();

            while (true)
            {
                rendererChain.Add($"{typeDetails.Name}Renderer");
                if (typeDetails.Parent == null)
                {
                    break;
                }

                typeDetails = typeDetails.Parent;
            }

            rendererChain.Reverse();

            return $"global::{typeDetails.Namespace}.{string.Join(".", rendererChain)}";
        }
    }
}

