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

                    if (uiType == "")
                    {
                        // TODO: Eliminate this case by supporting 'Entity'.
                        return "";
                    }

                    return $"private readonly {uiType} {fieldDetails.CamelCaseName}Field;";
                case OptionFieldType optionFieldType:
                    var innerUiType = GetUiFieldType(optionFieldType.ContainedType);

                    if (innerUiType == "")
                    {
                        // TODO: Eliminate this case by supporting 'Entity'.
                        return "";
                    }

                    var element = optionFieldType.IsNullable ? "NullableVisualElement" : "OptionVisualElement";

                    return $"private readonly {element}<{innerUiType}, {optionFieldType.ContainedType.FqnType}> {fieldDetails.CamelCaseName}Field;";
                case ListFieldType listFieldType:
                    var innerListType = GetUiFieldType(listFieldType.ContainedType);

                    if (innerListType == "")
                    {
                        // TODO: Eliminate this case by supporting 'Entity'.
                        return "";
                    }

                    return $"private readonly PaginatedListView<{innerListType}, {listFieldType.ContainedType.FqnType}> {fieldDetails.CamelCaseName}Field;";
                default:
                    // TODO: Maps.
                    return "";
            }
        }

        public IEnumerable<string> ToFieldInitialisation(UnityFieldDetails fieldDetails, string parentContainer)
        {
            switch (fieldDetails.FieldType)
            {
                case SingularFieldType singularFieldType:

                    var uiType = GetUiFieldType(singularFieldType.ContainedType);

                    if (uiType == "")
                    {
                        // TODO: Eliminate this case by supporting 'Entity'.
                        yield break;
                    }

                    var humanReadableName = Formatting.SnakeCaseToHumanReadable(fieldDetails.Name);
                    yield return $"{fieldDetails.CamelCaseName}Field = new {uiType}(\"{humanReadableName}\");";

                    if (singularFieldType.ContainedType.Category != ValueType.Type)
                    {
                        yield return $"{fieldDetails.CamelCaseName}Field.SetEnabled(false);";
                    }

                    if (singularFieldType.ContainedType.Category == ValueType.Enum)
                    {
                        yield return $"{fieldDetails.CamelCaseName}Field.Init(default({fieldDetails.Type}));";
                    }

                    yield return $"{parentContainer}.Add({fieldDetails.CamelCaseName}Field);";
                    break;
                case OptionFieldType optionFieldType:
                    var innerUiType = GetUiFieldType(optionFieldType.ContainedType);

                    if (innerUiType == "")
                    {
                        // TODO: Eliminate this case by supporting 'Entity'.
                        yield break;
                    }

                    yield return $"var {fieldDetails.CamelCaseName}InnerField = new {innerUiType}(\"Value\");";

                    if (optionFieldType.ContainedType.Category != ValueType.Type)
                    {
                        yield return $"{fieldDetails.CamelCaseName}InnerField.SetEnabled(false);";
                    }

                    if (optionFieldType.ContainedType.Category == ValueType.Enum)
                    {
                        yield return $"{fieldDetails.CamelCaseName}InnerField.Init(default({optionFieldType.ContainedType.FqnType}));";
                    }

                    var element = optionFieldType.IsNullable ? "NullableVisualElement" : "OptionVisualElement";

                    yield return
                        $"{fieldDetails.CamelCaseName}Field = new {element}<{innerUiType}, {optionFieldType.ContainedType.FqnType}>(\"{Formatting.SnakeCaseToHumanReadable(fieldDetails.Name)}\", {fieldDetails.CamelCaseName}InnerField, (element, data) => {{ {ContainedTypeToUiFieldUpdate(optionFieldType.ContainedType, "element", "data")} }});";
                    yield return $"{parentContainer}.Add({fieldDetails.CamelCaseName}Field);";
                    break;
                case ListFieldType listFieldType:
                    var innerListType = GetUiFieldType(listFieldType.ContainedType);

                    if (innerListType == "")
                    {
                        // TODO: Eliminate this case by supporting 'Entity'.
                        yield break;
                    }

                    yield return
                        $"{fieldDetails.CamelCaseName}Field = new PaginatedListView<{innerListType}, {listFieldType.ContainedType.FqnType}>(\"{Formatting.SnakeCaseToHumanReadable(fieldDetails.Name)}\", () => {{ var inner = new {innerListType}(\"\");";

                    // These lines are part of the func to create an inner list item.
                    if (listFieldType.ContainedType.Category != ValueType.Type)
                    {
                        yield return "inner.SetEnabled(false);";
                    }

                    if (listFieldType.ContainedType.Category == ValueType.Enum)
                    {
                        yield return $"inner.Init(default({listFieldType.ContainedType.FqnType}));";
                    }

                    yield return "return inner; }, (index, data, element) => {";

                    // These lines are part of the binding.
                    var labelBinding = listFieldType.ContainedType.Category == ValueType.Type ? "Label" : "label";
                    yield return $"element.{labelBinding} = $\"Item {{index + 1}}\";";
                    yield return ContainedTypeToUiFieldUpdate(listFieldType.ContainedType, "element", "data");
                    yield return "});";
                    yield return $"{parentContainer}.Add({fieldDetails.CamelCaseName}Field);";
                    break;
                default:
                    // TODO: Maps.
                    yield break;
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
                    if (GetUiFieldType(optionFieldType.ContainedType) == "")
                    {
                        // TODO: Eliminate this case by supporting 'Entity'.
                        return "";
                    }

                    return $"{uiElementName}.Update({fieldParent}.{fieldDetails.PascalCaseName});";
                case ListFieldType listFieldType:
                    if (GetUiFieldType(listFieldType.ContainedType) == "")
                    {
                        // TODO: Eliminate this case by supporting 'Entity'.
                        return "";
                    }

                    return $"{uiElementName}.Update({fieldParent}.{fieldDetails.PascalCaseName});";
                default:
                    // TODO: Maps.
                    return "";
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
                            return $"{uiElementName}.value = {fieldAccessor}.ToString();";
                        case PrimitiveType.Bytes:
                            return $"{uiElementName}.value = global::System.Text.Encoding.Default.GetString({fieldAccessor});";
                        case PrimitiveType.Bool:
                            return $"{uiElementName}.value = {fieldAccessor};";
                            break;
                        case PrimitiveType.Entity:
                            // TODO: Entity type.
                            return "";
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
                            return "TextField";
                        case PrimitiveType.Bool:
                            return "Toggle";
                        case PrimitiveType.Entity:
                            return "";
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

