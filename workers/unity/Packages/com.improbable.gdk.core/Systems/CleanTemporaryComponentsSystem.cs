using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Removes components with attribute RemoveAtEndOfTick from all entities
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSCleanGroup))]
    public class CleanTemporaryComponentsSystem : ComponentSystem
    {
        private readonly List<(ComponentGroup, ComponentType)> componentGroupsToRemove =
            new List<(ComponentGroup, ComponentType)>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

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

                    componentGroupsToRemove.Add((GetComponentGroup(ComponentType.ReadOnly(type)), type));
                }
            }
        }

        protected override void OnUpdate()
        {
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
    }
}
