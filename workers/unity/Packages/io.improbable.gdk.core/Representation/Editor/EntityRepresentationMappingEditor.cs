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

        private EntityRepresentationMapping targetDatabase => (EntityRepresentationMapping) target;
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
            const string uxmlPath = "Packages/io.improbable.gdk.core/Representation/Templates/EntityRepresentationMappingEditor.uxml";
            const string ussPath = "Packages/io.improbable.gdk.core/Representation/Templates/EntityRepresentationMappingEditor.uss";

            var windowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            var rootVisualElement = windowTemplate.CloneTree();

            var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
            rootVisualElement.styleSheets.Add(stylesheet);

            listContainer = rootVisualElement.Q("list-container");
            GenerateListElements();

            var button = rootVisualElement.Q<Button>("add-type-button");
            InitializeNewEntityButton(button);

            return rootVisualElement;
        }

        private void GenerateListElements()
        {
            const string uxmlPath = "Packages/io.improbable.gdk.core/Representation/Templates/EntityResolverElement.uxml";
            const string ussPath = "Packages/io.improbable.gdk.core/Representation/Templates/EntityResolverElement.uss";

            var elementTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            var stylesheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);

            listContainer.Clear();
            for (var i = 0; i < listProperty.arraySize; i++)
            {
                var elementIndex = i;
                var property = listProperty.GetArrayElementAtIndex(i);

                var rootElementContainer = elementTemplate.CloneTree();
                rootElementContainer.styleSheets.Add(stylesheet);

                rootElementContainer.Q<Label>("type-label").text =
                    targetDatabase.EntityRepresentationResolvers[i].GetType().Name;

                var elementPropertyField = new PropertyField(property) { name = "element-property" };
                elementPropertyField.BindProperty(property);
                rootElementContainer.Q("property-container").Insert(0, elementPropertyField);

                rootElementContainer.Q<Button>("delete-button").clicked += () =>
                {
                    listProperty.DeleteArrayElementAtIndex(elementIndex);
                    serializedObject.ApplyModifiedProperties();
                    GenerateListElements();
                };

                listContainer.Add(rootElementContainer);
            }
        }

        private void InitializeNewEntityButton(Button button)
        {
            button.clicked += () =>
            {
                var menu = new GenericMenu();

                foreach (var type in entityRepresentationTypes)
                {
                    menu.AddItem(new GUIContent(type.Name), false, AddNewElement, type);
                }

                menu.ShowAsContext();
            };
        }

        private void AddNewElement(object elementType)
        {
            var instance = (IEntityRepresentationResolver) Activator.CreateInstance((Type) elementType);
            targetDatabase.EntityRepresentationResolvers.Add(instance);
            serializedObject.Update();
            GenerateListElements();
        }
    }
}
