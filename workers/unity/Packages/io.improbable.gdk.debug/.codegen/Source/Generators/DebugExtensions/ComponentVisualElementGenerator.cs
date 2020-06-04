using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;

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
                    "Improbable.Gdk.Debug.WorkerInspector.Codegen"
                );

                cgw.Namespace(details.Namespace, ns =>
                {
                    ns.Type($"public class {details.Name}Renderer : ComponentVisualElement", type =>
                    {
                        type.Line($"public override ComponentType ComponentType {{ get; }} = ComponentType.ReadOnly<{details.Name}.Component>();");

                        GenerateConstructor(type, details);
                        GenerateUpdateMethod(type, details);
                    });
                });
            });
        }

        private static void GenerateConstructor(TypeBlock typeBlock, UnityComponentDetails details)
        {
            typeBlock.Method($"public {details.Name}Renderer() : base()", mb =>
            {
                mb.Line($"ComponentFoldout.text = \"{details.Name}\";");
                mb.Line($"AuthoritativeToggle.SetEnabled(false);");
            });
        }

        private static void GenerateUpdateMethod(TypeBlock typeBlock, UnityComponentDetails details)
        {
            typeBlock.Method("public override void Update(EntityManager manager, Entity entity)", mb =>
            {
                mb.Line($"AuthoritativeToggle.value = manager.HasComponent<{details.Name}.HasAuthority>(entity);");
            });
        }
    }
}
