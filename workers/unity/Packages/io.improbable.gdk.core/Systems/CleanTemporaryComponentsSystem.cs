using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Profiling;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Removes components with attribute RemoveAtEndOfTick from all entities
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(InternalSpatialOSCleanGroup))]
    public class CleanTemporaryComponentsSystem : ComponentSystem
    {
        private readonly List<(EntityQuery, ComponentType)> componentGroupsToRemove =
            new List<(EntityQuery, ComponentType)>();

        protected override void OnCreate()
        {
            base.OnCreate();

            // Find all components with the RemoveAtEndOfTick attribute
            foreach (var type in ReflectionUtility.GetNonAbstractTypes(typeof(IComponentData),
                typeof(RemoveAtEndOfTickAttribute)))
            {
                componentGroupsToRemove.Add((GetEntityQuery(ComponentType.ReadOnly(type)), type));
            }

            foreach (var type in ReflectionUtility.GetNonAbstractTypes(typeof(ISharedComponentData),
                typeof(RemoveAtEndOfTickAttribute)))
            {
                componentGroupsToRemove.Add((GetEntityQuery(ComponentType.ReadOnly(type)), type));
            }
        }

        protected override void OnUpdate()
        {
            Profiler.BeginSample("RemoveRemoveAtEndOfTick");
            foreach (var (componentGroup, componentType) in componentGroupsToRemove)
            {
                if (componentGroup.IsEmptyIgnoreFilter)
                {
                    continue;
                }

                using (var entityArray = componentGroup.ToEntityArray(Allocator.TempJob))
                {
                    foreach (var entity in entityArray)
                    {
                        PostUpdateCommands.RemoveComponent(entity, componentType);
                    }
                }
            }

            Profiler.EndSample();
        }
    }
}
