using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.PlayerLifecycle;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatResponseSystem : ComponentSystem
    {
        private ComponentGroup group;

        [Inject] private WorkerSystem worker;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            var query = new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<HeartbeatData>(),
                    ComponentType.Create<WorldCommands.DeleteEntity.CommandSender>(),
                    ComponentType.ReadOnly<PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat>(),
                    ComponentType.ReadOnly<SpatialEntityId>(),
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            group = GetComponentGroup(query);
        }

        protected override void OnUpdate()
        {
            var heartbeatType = GetArchetypeChunkComponentType<HeartbeatData>();
            var entityDeleterType = GetArchetypeChunkComponentType<WorldCommands.DeleteEntity.CommandSender>();
            var responsesType =
                GetArchetypeChunkComponentType<PlayerHeartbeatClient.CommandResponses.PlayerHeartbeat>(true);
            var spatialIdType = GetArchetypeChunkComponentType<SpatialEntityId>(true);

            var chunkArray = group.CreateArchetypeChunkArray(Allocator.Temp);

            foreach (var chunk in chunkArray)
            {
                var heartbeats = chunk.GetNativeArray(heartbeatType);
                var responses = chunk.GetNativeArray(responsesType);
                var deleteRequesters = chunk.GetNativeArray(entityDeleterType);
                var spatialIds = chunk.GetNativeArray(spatialIdType);

                for (var i = 0; i < responses.Length; i++)
                {
                    var heartbeatData = heartbeats[i];

                    var responded = false;
                    foreach (var response in responses[i].Responses)
                    {
                        if (response.StatusCode == StatusCode.Success)
                        {
                            responded = true;
                            break;
                        }
                    }

                    if (responded)
                    {
                        heartbeatData.NumFailedHeartbeats = 0;
                    }
                    else
                    {
                        heartbeatData.NumFailedHeartbeats += 1;

                        if (heartbeatData.NumFailedHeartbeats >= PlayerLifecycleConfig.MaxNumFailedPlayerHeartbeats)
                        {
                            var entityId = spatialIds[i].EntityId;
                            deleteRequesters[i].RequestsToSend.Add(WorldCommands.DeleteEntity.CreateRequest
                            (
                                entityId
                            ));

                            worker.LogDispatcher.HandleLog(LogType.Log,
                                new LogEvent(
                                        $"A client failed to respond to too many heartbeats. Deleting their player.")
                                    .WithField("EntityID", entityId));
                        }
                    }

                    heartbeats[i] = heartbeatData;
                }
            }

            chunkArray.Dispose();
        }
    }
}
