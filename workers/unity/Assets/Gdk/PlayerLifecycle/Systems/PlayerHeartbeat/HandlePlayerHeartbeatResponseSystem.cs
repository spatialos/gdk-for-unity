using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatResponseSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;

            public ComponentArray<CommandResponses<PlayerHeartbeatClient.PlayerHeartbeat.Response>>
                PlayerHeartbeatResponses;

            [ReadOnly] public ComponentDataArray<WorldCommandSender> WorldCommandSenders;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
            [ReadOnly] public ComponentDataArray<AwaitingHeartbeatResponseTag> AwaitingHeartbeatResponses;
            public EntityArray Entities;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var entityId = data.SpatialEntityIds[i].EntityId;
                var responses = data.PlayerHeartbeatResponses[i].Buffer;
                var entity = data.Entities[i];

                foreach (var response in responses)
                {
                    if (response.StatusCode != CommandStatusCode.Success)
                    {
                        var failedHeartbeatsComponent = EntityManager.HasComponent<HeartbeatData>(entity)
                            ? EntityManager.GetComponentData<HeartbeatData>(entity)
                            : new HeartbeatData();
                        failedHeartbeatsComponent.NumFailedHeartbeats++;
                        if (EntityManager.HasComponent<HeartbeatData>(entity))
                        {
                            PostUpdateCommands.SetComponent(entity, failedHeartbeatsComponent);
                        }
                        else
                        {
                            PostUpdateCommands.AddComponent(entity, failedHeartbeatsComponent);
                        }

                        Debug.LogFormat(Messages.FailedHeartbeat, entityId,
                            failedHeartbeatsComponent.NumFailedHeartbeats,
                            PlayerLifecycleConfig.MaxNumFailedPlayerHeartbeats);

                        if (failedHeartbeatsComponent.NumFailedHeartbeats >=
                            PlayerLifecycleConfig.MaxNumFailedPlayerHeartbeats)
                        {
                            Debug.LogFormat(Messages.DeletingPlayer, entityId);
                            data.WorldCommandSenders[i].SendDeleteEntityRequest(entityId);
                        }
                    }
                    else
                    {
                        if (EntityManager.HasComponent<HeartbeatData>(entity))
                        {
                            PostUpdateCommands.RemoveComponent<HeartbeatData>(entity);
                        }
                    }
                }

                PostUpdateCommands.RemoveComponent<AwaitingHeartbeatResponseTag>(entity);
            }
        }

        internal static class Messages
        {
            public const string FailedHeartbeat =
                "Client of player entity {0} failed to respond to heartbeat. Number of consecutive failures: {1}/{2}.";

            public const string DeletingPlayer =
                "Client of player entity {0} failed to respond to heartbeat and exhausted all retries. Deleting player.";
        }
    }
}
