using System.Linq;
using Unity.Entities;
using UnityEditor;
using UnityEngine;
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

        protected void InjectComponentIcon(string iconName)
        {
            var iconContent = EditorGUIUtility.IconContent(iconName);
            var iconVisualElement = new VisualElement();
            iconVisualElement.style.backgroundImage = new StyleBackground((Texture2D) iconContent.image);
            iconVisualElement.AddToClassList("component-icon");

            // We want to inject the icon after the toggle icon and before the text.
            var foldoutToggle = ComponentFoldout.Q<VisualElement>(className: "unity-toggle__input");
            var children = foldoutToggle.Children().ToList();
            children.Insert(1, iconVisualElement);

            foldoutToggle.Clear();
            foreach (var child in children)
            {
                foldoutToggle.Add(child);
            }
        }

        public abstract ComponentType ComponentType { get; }
        public abstract void Update(EntityManager manager, Entity entity);
    }
}
