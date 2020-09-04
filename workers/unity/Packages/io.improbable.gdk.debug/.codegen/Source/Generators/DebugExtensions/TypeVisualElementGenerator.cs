using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator
{
    public class TypeVisualElementGenerator
    {
        private readonly FieldTypeHandler fieldTypeHandler;

        public TypeVisualElementGenerator(DetailsStore detailsStore)
        {
            fieldTypeHandler = new FieldTypeHandler(detailsStore);
        }

        public CodeWriter Generate(UnityTypeDetails details)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "UnityEngine.UIElements",
                    "UnityEditor.UIElements",
                    "Improbable.Gdk.Debug.WorkerInspector.Codegen"
                );

                cgw.Namespace(details.Namespace, ns =>
                {
                    ns.Type(GenerateType(details));
                });
            });
        }

        private TypeBlock GenerateType(UnityTypeDetails details)
        {
            return Scope.Type($"public class {details.Name}Renderer : SchemaTypeVisualElement<{details.FullyQualifiedName}>",
                type =>
                {
                    type.TextList(details.FieldDetails.Select(fieldTypeHandler.ToFieldDeclaration));

                    GenerateConstructor(type, details);
                    GenerateUpdateMethod(type, details);

                    foreach (var nestedType in details.ChildTypes)
                    {
                        type.Type(GenerateType(nestedType));
                    }
                });
        }

        private void GenerateConstructor(TypeBlock typeBlock, UnityTypeDetails details)
        {
            typeBlock.Method($"public {details.Name}Renderer(string label) : base(label)", mb =>
            {
                foreach (var field in details.FieldDetails)
                {
                    mb.TextList(fieldTypeHandler.ToFieldInitialisation(field, "Container"));
                }
            });
        }

        private void GenerateUpdateMethod(TypeBlock typeBlock, UnityTypeDetails details)
        {
            typeBlock.Method($"protected override void Update({details.FullyQualifiedName} data)", mb =>
            {
                mb.TextList(details.FieldDetails.Select(fd => fieldTypeHandler.ToUiFieldUpdate(fd, "data")));
            });
        }
    }
}
