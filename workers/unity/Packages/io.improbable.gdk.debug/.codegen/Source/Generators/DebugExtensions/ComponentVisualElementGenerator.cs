using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator
{
    public class ComponentVisualElementGenerator
    {
        private readonly FieldTypeHandler typeGenerator;

        public ComponentVisualElementGenerator(DetailsStore detailsStore)
        {
            typeGenerator = new FieldTypeHandler(detailsStore);
        }

        public CodeWriter Generate(UnityComponentDetails details)
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

                        type.TextList(details.FieldDetails.Select(typeGenerator.ToFieldDeclaration));

                        GenerateConstructor(type, details);
                        GenerateUpdateMethod(type, details);
                    });
                });
            });
        }

        private void GenerateConstructor(TypeBlock typeBlock, UnityComponentDetails details)
        {
            typeBlock.Method($"public {details.Name}Renderer() : base()", mb =>
            {
                mb.Line($"ComponentFoldout.text = \"{details.Name}\";");
                mb.Line($"AuthoritativeToggle.SetEnabled(false);");

                foreach (var field in details.FieldDetails)
                {
                    mb.TextList(typeGenerator.ToFieldInitialisation(field, "ComponentFoldout"));
                }
            });
        }

        private void GenerateUpdateMethod(TypeBlock typeBlock, UnityComponentDetails details)
        {
            typeBlock.Method("public override void Update(EntityManager manager, Entity entity)", mb =>
            {
                mb.Line($"AuthoritativeToggle.value = manager.HasComponent<{details.Name}.HasAuthority>(entity);");
                mb.Line($"var component = manager.GetComponentData<{details.Name}.Component>(entity);");

                mb.TextList(details.FieldDetails.Select(fd => typeGenerator.ToUiFieldUpdate(fd, "component")));
            });
        }
    }
}