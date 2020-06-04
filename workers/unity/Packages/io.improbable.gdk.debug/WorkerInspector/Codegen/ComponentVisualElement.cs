using Unity.Entities;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public abstract class ComponentVisualElement : VisualElement
    {
        private const string UxmlPath =
            "Packages/io.improbable.gdk.debug/WorkerInspector/Templates/ComponentDrawer.uxml";

        protected readonly Foldout ComponentFoldout;
        protected readonly Toggle AuthoritativeToggle;

        protected ComponentVisualElement()
        {
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            template.CloneTree(this);

            ComponentFoldout = this.Q<Foldout>(className: "component-foldout");
            AuthoritativeToggle = this.Q<Toggle>(className: "is-auth-toggle");
        }

        public abstract ComponentType ComponentType { get; }
        public abstract void Update(EntityManager manager, Entity entity);
    }
}
