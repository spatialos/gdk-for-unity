using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Removes components with attribute RemoveAtEndOfTick from all entities
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateAfter(typeof(SpatialOSSendSystem))]
    public class CleanTemporaryComponentsSystem : ComponentSystem
    {
        private readonly List<(EntityQuery, ComponentType)> componentGroupsToRemove =
            new List<(EntityQuery, ComponentType)>();

        protected override void OnCreate()
        {
            var types = new List<Type>
            {
                typeof(OnConnected),
                typeof(OnDisconnected),
                typeof(NewlyAddedSpatialOSEntity),
            };

            // Cache queries for temporary components
            foreach (var type in types)
            {
                componentGroupsToRemove.Add((GetEntityQuery(ComponentType.ReadOnly(type)), type));
            }
        }

        protected override void OnUpdate()
        {
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
        }
    }
}
