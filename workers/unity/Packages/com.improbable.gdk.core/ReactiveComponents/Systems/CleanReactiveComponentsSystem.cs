using System;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.ReactiveComponents
{
    /// <summary>
    ///     Removes GDK reactive components from all entities
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class CleanReactiveComponentsSystem : ComponentSystem
    {
        private readonly List<ComponentCleanup> componentCleanups = new List<ComponentCleanup>();

        // Here to prevent adding an action for the same type multiple times
        private readonly HashSet<Type> typesToRemove = new HashSet<Type>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            GenerateComponentGroups();
        }

        private void GenerateComponentGroups()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
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
            foreach (var cleanup in componentCleanups)
            {
                Profiler.BeginSample("CleanReactiveComponents");
                cleanup.Handler.CleanComponents(cleanup.ComponentsToCleanGroup, this, PostUpdateCommands);
                Profiler.EndSample();
            }
        }

        private struct ComponentCleanup
        {
            public ComponentCleanupHandler Handler;
            public ComponentGroup ComponentsToCleanGroup;
        }
    }
}
