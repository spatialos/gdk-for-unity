using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class DeleteCubeCommandReceiver : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private CubeSpawnerWriter cubeSpawnerWriter;
        [Require] private CubeSpawnerCommandReceiver cubeSpawnerReceiver;
        [Require] private WorldCommandSender worldCommandSender;

        [Require] private ILogDispatcher logDispatcher;
#pragma warning restore 649

        public void OnEnable()
        {
            cubeSpawnerReceiver.OnDeleteSpawnedCubeRequestReceived += OnDeleteSpawnedCubeRequest;
        }

        private void OnDeleteSpawnedCubeRequest(CubeSpawner.DeleteSpawnedCube.ReceivedRequest receivedRequest)
        {
            var entityId = receivedRequest.Payload.CubeEntityId;
            var spawnedCubes = cubeSpawnerWriter.Data.SpawnedCubes;

            if (!spawnedCubes.Contains(entityId))
            {
                cubeSpawnerReceiver.SendDeleteSpawnedCubeResponse(
                    new CubeSpawner.DeleteSpawnedCube.Response(receivedRequest.RequestId,
                        $"Requested entity id {entityId} not found in list."));
            }
            else
            {
                cubeSpawnerReceiver.SendDeleteSpawnedCubeResponse(
                    new CubeSpawner.DeleteSpawnedCube.Response(receivedRequest.RequestId, new Empty()));
            }

            worldCommandSender.SendDeleteEntityCommand(new WorldCommands.DeleteEntity.Request(entityId),
                OnDeleteEntityResponse);
        }


        private void OnDeleteEntityResponse(WorldCommands.DeleteEntity.ReceivedResponse response)
        {
            var entityId = response.RequestPayload.EntityId;

            // Log not error as the most common failure cause is that the entity has already been deleted.
            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Log,
                    new LogEvent("Could not delete entity.")
                        .WithField(LoggingUtils.EntityId, entityId)
                        .WithField("Reason", response.Message));
                return;
            }

            var spawnedCubesCopy =
                new List<EntityId>(cubeSpawnerWriter.Data.SpawnedCubes);

            if (!spawnedCubesCopy.Remove(entityId))
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent("The entity has been unexpectedly removed from the list.")
                        .WithField(LoggingUtils.EntityId, entityId));
                return;
            }

            cubeSpawnerWriter.SendUpdate(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubesCopy
            });
        }
    }
}
