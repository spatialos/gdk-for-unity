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
        private readonly List<Action> removeComponentActions = new List<Action>();

        private readonly List<ComponentCleanup> componentCleanups = new List<ComponentCleanup>();

        // Here to prevent adding an action for the same type multiple times
        private readonly HashSet<Type> typesToRemove = new HashSet<Type>();

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            // TODO: Remove workaround when UTY-936 is fixed
            var addRemoveComponentActionMethod =
                typeof(CleanReactiveComponentsSystem).GetMethod(nameof(AddRemoveComponentAction),
                    BindingFlags.NonPublic | BindingFlags.Instance);

            if (addRemoveComponentActionMethod == null)
            {
                throw new MissingMethodException(nameof(CleanReactiveComponentsSystem),
                    nameof(AddRemoveComponentAction));
            }

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
                        addRemoveComponentActionMethod.MakeGenericMethod(type).Invoke(this, null);
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
                            addRemoveComponentActionMethod.MakeGenericMethod(componentType.GetManagedType())
                                .Invoke(this, null);
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

        private void AddRemoveComponentAction<T>()
        {
            var componentGroup = GetComponentGroup(ComponentType.ReadOnly<T>());
            removeComponentActions.Add(() =>
            {
                if (componentGroup.IsEmptyIgnoreFilter)
                {
                    return;
                }

                var entityArray = componentGroup.GetEntityArray();
                for (var i = 0; i < entityArray.Length; ++i)
                {
                    PostUpdateCommands.RemoveComponent<T>(entityArray[i]);
                }
            });
        }

        protected override void OnUpdate()
        {
            foreach (var removeComponentAction in removeComponentActions)
            {
                removeComponentAction();
            }

            var buffer = PostUpdateCommands;
            foreach (var cleanup in componentCleanups)
            {
                cleanup.Handler.CleanupUpdates(cleanup.UpdateGroup, ref buffer);
                cleanup.Handler.CleanupEvents(cleanup.EventGroups, ref buffer);
                cleanup.Handler.CleanupAuthChanges(cleanup.AuthorityChangesGroup, ref buffer);
                cleanup.Handler.CleanupCommands(cleanup.CommandsGroups, ref buffer);
            }
        }

        private struct ComponentCleanup
        {
            public ComponentCleanupHandler Handler;
            public ComponentGroup UpdateGroup;
            public ComponentGroup AuthorityChangesGroup;
            public ComponentGroup[] EventGroups;
            public ComponentGroup[] CommandsGroups;
        }
    }
}
