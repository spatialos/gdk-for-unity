using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(HandlePlayerHeartbeatResponseSystem))]
    public class PlayerHeartbeatInitializationSystem : ComponentSystem
    {
        private EntityQuery initializerGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            var initializerQuery = new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<PlayerHeartbeatClient.Component>(),
                },
                Any = Array.Empty<ComponentType>(),
                None = new[]
                {
                    ComponentType.ReadWrite<HeartbeatData>(),
                },
            };

            initializerGroup = GetEntityQuery(initializerQuery);
        }

        protected override void OnUpdate()
        {
            var entityType = GetArchetypeChunkEntityType();
            var chunkArray = initializerGroup.CreateArchetypeChunkArray(Allocator.TempJob);

            foreach (var chunk in chunkArray)
            {
                var entities = chunk.GetNativeArray(entityType);
                for (int i = 0; i < entities.Length; i++)
                {
                    PostUpdateCommands.AddComponent(entities[i], new HeartbeatData());
                }
            }

            chunkArray.Dispose();
        }
    }
}
