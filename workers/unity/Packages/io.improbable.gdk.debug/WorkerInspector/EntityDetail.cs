using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Debug.WorkerInspector.Codegen;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector
{
    internal class EntityDetail : VisualElement
    {
        private readonly VisualElement componentContainer;
        private readonly Label entityName;
        private readonly Label entityId;

        private World world;
        private EntityData? selected;
        private readonly List<ComponentVisualElement> visualElements = new List<ComponentVisualElement>();

        public EntityDetail()
        {
            const string uxmlPath = "Packages/io.improbable.gdk.debug/WorkerInspector/Templates/EntityDetail.uxml";
            var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            template.CloneTree(this);

            entityName = this.Q<Label>("entity-name");
            entityId = this.Q<Label>("entity-id");
            componentContainer = this.Q<ScrollView>();
            var window = EditorWindow.GetWindow<WorkerInspectorWindow>();
            if (window != null)
            {
                window.OnToggleHideCollections += ComponentVisualElement.ToggleHideIfEmpty;
            }
        }

        public void SetSelectedEntity(EntityData entityData)
        {
            selected = entityData;
            Update();
        }

        public void SetWorld(World world)
        {
            this.world = world;
            selected = null;
            componentContainer.Clear();
            visualElements.Clear();
            Update();
        }

        public void Update()
        {
            if (!selected.HasValue || !world.EntityManager.Exists(selected.Value.Entity))
            {
                selected = null;
                entityName.text = "No entity selected";
                entityId.text = string.Empty;
                return;
            }

            entityName.text = selected.Value.Metadata;
            entityId.text = $"Entity ID: {selected.Value.EntityId}";

            UpdateComponentSet();

            foreach (var element in visualElements)
            {
                element.Update(world.EntityManager, selected.Value.Entity);
            }
        }

        private void UpdateComponentSet()
        {
            using (var components = world.EntityManager.GetComponentTypes(selected.Value.Entity, allocator: Allocator.Temp))
            {
                var spatialComponents = components
                    .Where(type => typeof(ISpatialComponentData).IsAssignableFrom(type.GetManagedType()))
                    .OrderBy(type => ComponentDatabase.GetComponentId(type.GetManagedType()));

                if (AreSameComponents(spatialComponents, visualElements))
                {
                    return;
                }

                componentContainer.Clear();
                visualElements.Clear();

                foreach (var componentType in spatialComponents)
                {
                    var componentElement = Cache.Get(componentType);
                    componentContainer.Add(componentElement);
                    visualElements.Add(componentElement);
                }
            }
        }

        // NOTE: This relies on the visual elements being inserted in a sorted-order, which is the case above.
        private static bool AreSameComponents(IEnumerable<ComponentType> componentTypes,
            IReadOnlyList<ComponentVisualElement> componentVisualElements)
        {
            if (componentTypes.Count() != componentVisualElements.Count)
            {
                return false;
            }

            var i = 0;

            foreach (var componentType in componentTypes)
            {
                if (componentType.TypeIndex != componentVisualElements[i].ComponentType.TypeIndex)
                {
                    return false;
                }

                i++;
            }

            return true;
        }

        private static class Cache
        {
            private static readonly Dictionary<ComponentType, ComponentVisualElement> cache =
                TypeCache.GetTypesDerivedFrom<ComponentVisualElement>()
                    .Select(type => (ComponentVisualElement) Activator.CreateInstance(type))
                    .ToDictionary(cve => cve.ComponentType, cve => cve);

            public static ComponentVisualElement Get(ComponentType componentType)
            {
                if (!cache.TryGetValue(componentType, out var element))
                {
                    throw new ArgumentException($"Unknown component type: '{componentType.GetManagedType().Name}'",
                        nameof(componentType));
                }

                return element;
            }
        }

        public new class UxmlFactory : UxmlFactory<EntityDetail>
        {
        }
    }
}
