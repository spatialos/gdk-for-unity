using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatResponseSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat> PlayerHeartbeatResponses;
            public ComponentDataArray<WorldCommands.DeleteEntity.CommandSender> WorldCommandSenders;
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
                var responses = data.PlayerHeartbeatResponses[i].Responses;
                var entity = data.Entities[i];
                var entityDeleteSender = data.WorldCommandSenders[i];

                foreach (var response in responses)
                {
                    if (response.StatusCode != StatusCode.Success)
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
                            entityDeleteSender.RequestsToSend.Add(new WorldCommands.DeleteEntity.Request
                            {
                                EntityId = entityId,
                            });
                            data.WorldCommandSenders[i] = entityDeleteSender;
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
