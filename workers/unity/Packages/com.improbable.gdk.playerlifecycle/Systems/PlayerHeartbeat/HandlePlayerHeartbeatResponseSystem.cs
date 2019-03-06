using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatResponseSystem : ComponentSystem
    {
        private ILogDispatcher logger;
        private CommandSystem commandSystem;
        private WorkerSystem workerSystem;

        private readonly Dictionary<(EntityId, Entity), bool> respondedCache =
            new Dictionary<(EntityId, Entity), bool>();

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            workerSystem = World.GetExistingManager<WorkerSystem>();
            commandSystem = World.GetExistingManager<CommandSystem>();
            logger = workerSystem.LogDispatcher;
        }

        protected override void OnUpdate()
        {
            var responses = commandSystem.GetResponses<PlayerHeartbeatClient.PlayerHeartbeat.ReceivedResponse>();
            if (responses.Count == 0)
            {
                return;
            }

            var heartbeats = GetComponentDataFromEntity<HeartbeatData>();

            // Concatenate command responses
            for (var i = 0; i < responses.Count; i++)
            {
                ref readonly var response = ref responses[i];
                var spatialId = response.EntityId;
                if (!workerSystem.TryGetEntity(spatialId, out var entity) || !heartbeats.Exists(entity))
                {
                    continue;
                }

                var key = (spatialId, entity);
                respondedCache.TryGetValue(key, out var responded);
                respondedCache[key] = responded | (response.StatusCode == StatusCode.Success);
            }

            foreach (var response in respondedCache)
            {
                var spatialId = response.Key.Item1;
                var entity = response.Key.Item2;
                var heartbeatData = heartbeats[entity];

                if (response.Value == true)
                {
                    heartbeatData.NumFailedHeartbeats = 0;
                }
                else
                {
                    heartbeatData.NumFailedHeartbeats += 1;

                    if (heartbeatData.NumFailedHeartbeats >= PlayerLifecycleConfig.MaxNumFailedPlayerHeartbeats)
                    {
                        commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(spatialId));

                        logger.HandleLog(LogType.Log,
                            new LogEvent(
                                    $"A client failed to respond to too many heartbeats. Deleting their player.")
                                .WithField("EntityID", spatialId));
                    }
                }

                heartbeats[entity] = heartbeatData;
            }

            respondedCache.Clear();
        }
    }
}
