using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.Core.CodegenAdapters;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Removes GDK reactive components and components with attribute RemoveAtEndOfTick from all entities
    /// </summary>
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class CleanReactiveComponentsSystem : ComponentSystem
    {
        private readonly List<ComponentCleanup> componentCleanups = new List<ComponentCleanup>();

        // Here to prevent adding an action for the same type multiple times
        private readonly HashSet<Type> typesToRemove = new HashSet<Type>();
        private readonly List<(ComponentGroup, Type)> componentGroupsToRemove = new List<(ComponentGroup, Type)>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Find all components with the RemoveAtEndOfTick attribute
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<RemoveAtEndOfTickAttribute>(false) == null)
                    {
                        continue;
                    }

                    if (!typeof(IComponentData).IsAssignableFrom(type)
                        && !typeof(ISharedComponentData).IsAssignableFrom(type))
                    {
                        continue;
                    }

                    if (typesToRemove.Add(type))
                    {
                        componentGroupsToRemove.Add((GetComponentGroup(ComponentType.ReadOnly(type)), type));
                    }
                }

                // Find all ComponentCleanupHandlers
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(ComponentCleanupHandler).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        var componentCleanupHandler = (ComponentCleanupHandler) Activator.CreateInstance(type);
                        foreach (var componentType in componentCleanupHandler.CleanUpComponentTypes)
                        {
                            typesToRemove.Add(componentType.GetManagedType());
                            componentGroupsToRemove.Add((GetComponentGroup(componentType), componentType.GetManagedType()));
                        }

                        // Updates group
                        componentCleanups.Add(new ComponentCleanup
                        {
                            Handler = componentCleanupHandler,
                            UpdateGroup = GetComponentGroup(componentCleanupHandler.ComponentUpdateType),
                            AuthorityChangesGroup = GetComponentGroup(componentCleanupHandler.AuthorityChangesType),
                            EventGroups =
                                componentCleanupHandler.EventComponentTypes
                                    .Select(eventType => GetComponentGroup(eventType)).ToArray(),
                            CommandsGroups = componentCleanupHandler.CommandReactiveTypes
                                .Select(t => GetComponentGroup(t)).ToArray()
                        });
                    }
                }
            }
        }

        protected override void OnUpdate()
        {
            var buffer = PostUpdateCommands;
            foreach (var cleanup in componentCleanups)
            {
                cleanup.Handler.CleanupUpdates(cleanup.UpdateGroup, ref buffer);
                cleanup.Handler.CleanupEvents(cleanup.EventGroups, ref buffer);
                cleanup.Handler.CleanupAuthChanges(cleanup.AuthorityChangesGroup, ref buffer);
                cleanup.Handler.CleanupCommands(cleanup.CommandsGroups, ref buffer);
            }

            // Clean components with RemoveAtEndOfTick attribute
            RemoveComponents();
        }

        private struct ComponentCleanup
        {
            public ComponentCleanupHandler Handler;
            public ComponentGroup UpdateGroup;
            public ComponentGroup AuthorityChangesGroup;
            public ComponentGroup[] EventGroups;
            public ComponentGroup[] CommandsGroups;
        }

        private void RemoveComponents()
        {
            var componentsToRemove = new List<(Entity, Type)>();
            foreach ((ComponentGroup componentGroup, Type type) in componentGroupsToRemove)
            {
                if (componentGroup.IsEmptyIgnoreFilter)
                {
                    continue;
                }

                var entityArray = componentGroup.GetEntityArray();
                for (var i = 0; i < entityArray.Length; ++i)
                {
                    componentsToRemove.Add((entityArray[i], type));
                }
            }

            foreach ((Entity entity, Type type) in componentsToRemove)
            {
                EntityManager.RemoveComponent(entity, type);
            }
        }
    }
}
