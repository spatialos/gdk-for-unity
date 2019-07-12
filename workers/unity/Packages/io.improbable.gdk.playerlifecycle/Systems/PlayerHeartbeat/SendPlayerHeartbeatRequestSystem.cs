using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(SpatialOSUpdateGroup))]
    public class SendPlayerHeartbeatRequestSystem : ComponentSystem
    {
        private float timeOfNextHeartbeat = Time.time + PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds;
        private EntityQuery group;
        private CommandSystem commandSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            group = GetEntityQuery(
                ComponentType.ReadOnly<PlayerHeartbeatServer.ComponentAuthority>(),
                ComponentType.ReadWrite<HeartbeatData>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            );
            group.SetFilter(PlayerHeartbeatServer.ComponentAuthority.Authoritative);

            commandSystem = World.GetExistingSystem<CommandSystem>();
        }

        protected override void OnUpdate()
        {
            if (Time.time < timeOfNextHeartbeat)
            {
                return;
            }

            timeOfNextHeartbeat = Time.time + PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds;

            Entities.With(group).ForEach((ref SpatialEntityId spatialEntityId) =>
            {
                commandSystem.SendCommand(
                    new PlayerHeartbeatClient.PlayerHeartbeat.Request(spatialEntityId.EntityId, new Empty()));
            });
        }
    }
}
