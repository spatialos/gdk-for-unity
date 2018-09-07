using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.PlayerLifecycle;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatResponseSystem : ComponentSystem
    {
        private struct InitializeGroup
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly]
            public ComponentDataArray<PlayerHeartbeatClient.Component> NeedsHeartbeat;
            public SubtractiveComponent<HeartbeatData> NoHeartbeatData;
        }

        private struct HeartbeatGroup
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<HeartbeatData> Heartbeats;
            public ComponentDataArray<WorldCommands.DeleteEntity.CommandSender> WorldCommandSenders;
            [ReadOnly] public ComponentDataArray<PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat> PlayerHeartbeatResponses;
            [ReadOnly] public ComponentDataArray<SpatialEntityId> SpatialEntityIds;
        }

        [Inject] private InitializeGroup initializeGroup;
        [Inject] private HeartbeatGroup heartbeatGroup;

        protected override void OnUpdate()
        {
            for (var i = 0; i < initializeGroup.Length; i++)
            {
                PostUpdateCommands.AddComponent(initializeGroup.Entities[i], new HeartbeatData());
            }

            for (var i = 0; i < heartbeatGroup.Length; i++)
            {
                var heartbeatsComponent = heartbeatGroup.Heartbeats[i];

                var stillAlive = false;
                var responses = heartbeatGroup.PlayerHeartbeatResponses[i].Responses;
                foreach (var response in responses)
                {
                    if (response.StatusCode == StatusCode.Success)
                    {
                        stillAlive = true;
                        break;
                    }
                }

                if (stillAlive)
                {
                    heartbeatsComponent.NumFailedHeartbeats = 0;
                }
                else
                {
                    heartbeatsComponent.NumFailedHeartbeats++;

                    if (heartbeatsComponent.NumFailedHeartbeats >=
                        PlayerLifecycleConfig.MaxNumFailedPlayerHeartbeats)
                    {
                        var entityDeleteSender = heartbeatGroup.WorldCommandSenders[i];
                        entityDeleteSender.RequestsToSend.Add(WorldCommands.DeleteEntity.CreateRequest
                        (
                            heartbeatGroup.SpatialEntityIds[i].EntityId
                        ));
                        heartbeatGroup.WorldCommandSenders[i] = entityDeleteSender;
                    }
                }

                heartbeatGroup.Heartbeats[i] = heartbeatsComponent;
            }
        }
    }
}
