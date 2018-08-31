using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateBefore(typeof(SpatialOSUpdateGroup))]
    public class SendPlayerHeartbeatRequestSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat> RequestSenders;
            [ReadOnly] public ComponentDataArray<Authoritative<PlayerHeartbeatServer>> AuthorityMarkers;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
            public EntityArray Entities;
            public SubtractiveComponent<AwaitingHeartbeatResponseTag> NotAwaitingHeartbeatResponse;
        }

        [Inject] private Data data;

        private float timeOfNextHeartbeat = Time.time + PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds;

        protected override void OnUpdate()
        {
            if (Time.time < timeOfNextHeartbeat)
            {
                return;
            }

            timeOfNextHeartbeat = Time.time + PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds;

            for (var i = 0; i < data.Length; i++)
            {
                var entityId = data.SpatialEntityIds[i].EntityId;
                data.RequestSenders[i].RequestsToSend
                    .Add(PlayerHeartbeatClient.PlayerHeartbeat.CreateRequest(entityId, new Empty()));

                var entity = data.Entities[i];
                PostUpdateCommands.AddComponent(entity, new AwaitingHeartbeatResponseTag());
            }
        }
    }
}
