using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model;
using Improbable.Gdk.CodeGeneration.Model.Details;
using Improbable.Gdk.CodeGeneration.Utils;
using ValueType = Improbable.Gdk.CodeGeneration.Model.ValueType;

namespace Improbable.Gdk.CodeGenerator
{
    public static class ComponentVisualElementGenerator
    {
        public static CodeWriter Generate(UnityComponentDetails details)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Unity.Entities",
                    "UnityEngine.UIElements",
                    "UnityEditor.UIElements",
                    "Improbable.Gdk.Debug.WorkerInspector.Codegen"
                );

                cgw.Namespace(details.Namespace, ns =>
                {
                    ns.Type($"public class {details.Name}Renderer : ComponentVisualElement", type =>
                    {
                        type.Line($"public override ComponentType ComponentType {{ get; }} = ComponentType.ReadOnly<{details.Name}.Component>();");

                        type.TextList(details.FieldDetails.Select(ToFieldDeclaration));

                        GenerateConstructor(type, details);
                        GenerateUpdateMethod(type, details);
                    });
                });
            });
        }

        private static string ToFieldDeclaration(UnityFieldDetails fieldDetails)
        {
            switch (fieldDetails.FieldType)
            {
                case SingularFieldType singularFieldType:
                    var uiType = GetUiFieldType(singularFieldType.ContainedType);

                    if (uiType == "")
                    {
                        // TODO: Eliminate this case.
                        return "";
                    }

                    return $"private readonly {uiType} {fieldDetails.CamelCaseName}Field;";
                default:
                    // TODO: Lists, maps, and options
                    return "";
            }
        }

        private static void GenerateConstructor(TypeBlock typeBlock, UnityComponentDetails details)
        {
            typeBlock.Method($"public {details.Name}Renderer() : base()", mb =>
            {
                mb.Line($"ComponentFoldout.text = \"{details.Name}\";");
                mb.Line($"AuthoritativeToggle.SetEnabled(false);");

                foreach (var field in details.FieldDetails)
                {
                    mb.TextList(ToFieldInitialisation(field));
                }
            });
        }

        private static IEnumerable<string> ToFieldInitialisation(UnityFieldDetails fieldDetails)
        {
            switch (fieldDetails.FieldType)
            {
                case SingularFieldType singularFieldType:

                    var uiType = GetUiFieldType(singularFieldType.ContainedType);

                    if (uiType == "")
                    {
                        // TODO: Eliminate this case.
                        yield break;
                    }

                    var humanReadableName = Formatting.SnakeCaseToHumanReadable(fieldDetails.Name);
                    yield return $"{fieldDetails.CamelCaseName}Field = new {uiType}(\"{humanReadableName}\");";
                    yield return $"{fieldDetails.CamelCaseName}Field.SetEnabled(false);";

                    if (singularFieldType.ContainedType.Category == ValueType.Enum)
                    {
                        yield return $"{fieldDetails.CamelCaseName}Field.Init(default({fieldDetails.Type}));";
                    }

                    yield return $"ComponentFoldout.Add({fieldDetails.CamelCaseName}Field);";
                    break;
                default:
                    // TODO: Lists, maps, and options
                    yield break;
            }
        }

        private static void GenerateUpdateMethod(TypeBlock typeBlock, UnityComponentDetails details)
        {
            typeBlock.Method("public override void Update(EntityManager manager, Entity entity)", mb =>
            {
                mb.Line($"AuthoritativeToggle.value = manager.HasComponent<{details.Name}.HasAuthority>(entity);");
                mb.Line($"var component = manager.GetComponentData<{details.Name}.Component>(entity);");

                mb.TextList(TextList.New(details.FieldDetails.Select(ToUiFieldUpdate)));
            });
        }

        private static string ToUiFieldUpdate(UnityFieldDetails fieldDetails)
        {
            switch (fieldDetails.FieldType)
            {
                case SingularFieldType singularFieldType:
                    switch (singularFieldType.ContainedType.Category)
                    {
                        case ValueType.Enum:
                            return $"{fieldDetails.CamelCaseName}Field.value = component.{fieldDetails.PascalCaseName};";
                        case ValueType.Primitive:
                            var primitiveType = singularFieldType.ContainedType.PrimitiveType.Value;

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
                                    return $"{fieldDetails.CamelCaseName}Field.value = component.{fieldDetails.PascalCaseName}.ToString();";
                                case PrimitiveType.Bytes:
                                    return $"{fieldDetails.CamelCaseName}Field.value = global::System.Text.Encoding.Default.GetString(component.{fieldDetails.PascalCaseName});";
                                case PrimitiveType.Bool:
                                    return $"{fieldDetails.CamelCaseName}Field.value = component.{fieldDetails.PascalCaseName};";
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
                            // TODO: User defined types.
                            return "";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    // TODO: Lists, maps, and options
                    return "";
            }
        }

        private static string GetUiFieldType(ContainedType type)
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
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
