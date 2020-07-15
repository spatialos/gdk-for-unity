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

            // This assignment forces the generation of the text element
            // This prevents the text element from generating after the button which aligns the text element to the right.
            ComponentFoldout.text = "Component";
            SetInfoButton();
        }

        protected override void ExecuteDefaultActionAtTarget(EventBase evt)
        {
            base.ExecuteDefaultActionAtTarget(evt);
            if (evt is HideCollectionEvent hideEvent)
            {
                hideEvent.PropagateToChildren(ComponentFoldout);
            }
        }

        protected void InjectComponentIcon(string iconName)
        {
            var iconContent = EditorGUIUtility.IconContent(iconName);
            var iconVisualElement = new VisualElement();
            iconVisualElement.style.backgroundImage = new StyleBackground((Texture2D) iconContent.image);
            iconVisualElement.AddToClassList("component-icon");

            // We want to inject the icon after the toggle icon and before the text.
            var foldoutToggle = ComponentFoldout.Q<VisualElement>(className: "unity-toggle__input");
            foldoutToggle.Insert(1, iconVisualElement);
        }

        private void SetInfoButton()
        {
            var iconContent = EditorGUIUtility.IconContent("console.infoicon.sml");
            var infoButton = new Button();
            infoButton.style.backgroundImage = new StyleBackground((Texture2D) iconContent.image);
            infoButton.AddToClassList("component-info-icon");
            infoButton.clicked += WriteDebugInfo;

            var buttonContainer = new VisualElement();
            buttonContainer.AddToClassList("component-info-container");
            buttonContainer.Add(infoButton);

            var foldoutToggle = ComponentFoldout.Q<VisualElement>(className: "unity-toggle__input");
            foldoutToggle.Add(buttonContainer);
        }

        protected abstract void WriteDebugInfo();

        public abstract ComponentType ComponentType { get; }

        public abstract void Update(EntityManager manager, Entity entity);
    }
}
