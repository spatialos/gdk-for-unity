using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Core.Representation.Editor
{
    [CustomEditor(typeof(EntityRepresentationMapping))]
    public class EntityRepresentationMappingEditor : UnityEditor.Editor
    {
        private static TypeCache.TypeCollection entityRepresentationTypes;

        private EntityRepresentationMapping TargetDatabase => (EntityRepresentationMapping) target;
        private SerializedProperty listProperty;

        private VisualElement listContainer;

        static EntityRepresentationMappingEditor()
        {
            entityRepresentationTypes = TypeCache.GetTypesDerivedFrom<IEntityRepresentationResolver>();
        }

        private void OnEnable()
        {
            listProperty = serializedObject.FindProperty(nameof(EntityRepresentationMapping.EntityRepresentationResolvers));
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement { name = "container" };
            container.style.paddingTop = 5;

            listContainer = new VisualElement { name = "list-container" };
            container.Add(listContainer);
            GenerateListElements();

            var addButton = CreateNewEntityButton();
            container.Add(addButton);

            return container;
        }

        private void GenerateListElements()
        {
            listContainer.Clear();
            for (var i = 0; i < listProperty.arraySize; i++)
            {
                var elementIndex = i;
                var property = listProperty.GetArrayElementAtIndex(i);

                var superElementContainer = new VisualElement { name = "element-root-container" };
                var typeLabel = new Label(TargetDatabase.EntityRepresentationResolvers[i].GetType().Name)
                {
                    name = "element-type-name"
                };

                typeLabel.style.unityTextAlign = TextAnchor.UpperRight;
                typeLabel.style.marginRight = 22;
                typeLabel.SetEnabled(false);
                superElementContainer.Add(typeLabel);

                var elementContainer = new VisualElement { name = "element-data-container" };
                elementContainer.style.flexDirection = FlexDirection.Row;
                elementContainer.style.marginTop = -17;

                // Display Element itself
                var elementPropertyField = new PropertyField(property);
                elementPropertyField.BindProperty(property);
                elementPropertyField.style.flexGrow = 1;
                elementPropertyField.style.marginRight = -22;
                elementContainer.Add(elementPropertyField);

                // Removal button
                var button = new Button { name = "remove-element-button", text = $"-" };
                button.clicked += () =>
                {
                    listProperty.DeleteArrayElementAtIndex(elementIndex);
                    serializedObject.ApplyModifiedProperties();
                    GenerateListElements();
                };
                button.style.maxWidth = button.style.maxHeight = 16;
                elementContainer.Add(button);

                superElementContainer.Add(elementContainer);
                listContainer.Add(superElementContainer);
            }
        }

        private Button CreateNewEntityButton()
        {
            var addButton = new Button { name = "new-element-button", text = "New Entity Type" };
            addButton.clicked += () =>
            {
                var menu = new GenericMenu();

                foreach (var type in entityRepresentationTypes)
                {
                    menu.AddItem(new GUIContent(type.Name), false, AddNewElement, type);
                }

                menu.ShowAsContext();
            };

            return addButton;
        }

        private void AddNewElement(object elementType)
        {
            var instance = (IEntityRepresentationResolver) Activator.CreateInstance((Type) elementType);
            TargetDatabase.EntityRepresentationResolvers.Add(instance);
            serializedObject.Update();
            GenerateListElements();
        }
    }
}
