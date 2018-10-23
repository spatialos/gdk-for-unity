using System;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Gdk.Core.CodegenAdapters;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Removes GDK reactive components and components with attribute RemoveAtEndOfTick from all entities
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class CleanReactiveComponentsSystem : ComponentSystem
    {
        private readonly List<ComponentCleanup> componentCleanups = new List<ComponentCleanup>();

        // Here to prevent adding an action for the same type multiple times
        private readonly HashSet<Type> typesToRemove = new HashSet<Type>();

        private readonly List<(ComponentGroup, ComponentType)> componentGroupsToRemove = new List<(ComponentGroup, ComponentType)>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
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
                    if (!typeof(ComponentCleanupHandler).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    var componentCleanupHandler = (ComponentCleanupHandler) Activator.CreateInstance(type);

                    componentCleanups.Add(new ComponentCleanup
                    {
                        Handler = componentCleanupHandler,
                        ComponentsToCleanGroup = GetComponentGroup(componentCleanupHandler.CleanupArchetypeQuery)
                    });
                }
            }
        }

        protected override void OnUpdate()
        {
            var buffer = PostUpdateCommands;

            foreach (var cleanup in componentCleanups)
            {
                Profiler.BeginSample("CleanReactiveComponents");
                cleanup.Handler.CleanComponents(cleanup.ComponentsToCleanGroup, this, PostUpdateCommands);
                Profiler.EndSample();
            }

            Profiler.BeginSample("RemoveRemoveAtEndOfTick");
            foreach ((var componentGroup, var componentType) in componentGroupsToRemove)
            {
                if (componentGroup.IsEmptyIgnoreFilter)
                {
                    continue;
                }

                var entityArray = componentGroup.GetEntityArray();
                for (var i = 0; i < entityArray.Length; i++)
                {
                    PostUpdateCommands.RemoveComponent(entityArray[i], componentType);
                }
            }

            Profiler.EndSample();
        }

        private struct ComponentCleanup
        {
            public ComponentCleanupHandler Handler;
            public ComponentGroup ComponentsToCleanGroup;
        }
    }
}
