using System.Collections.Generic;
using System.Threading.Tasks;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;

namespace Playground.MonoBehaviours
{
    public class SpawnCubeCommandReceiver : MonoBehaviour
    {
#pragma warning disable 649
        [Require] private TransformInternalReader transformReader;
        [Require] private CubeSpawnerCommandReceiver cubeSpawnerCommandRequestHandler;
        [Require] private CubeSpawnerWriter cubeSpawnerWriter;
        [Require] private WorldCommandSender worldCommandRequestSender;
        [Require] private World world;

        [Require] private ILogDispatcher logDispatcher;
#pragma warning restore 649

        private Vector3 offset;

        public void OnEnable()
        {
            cubeSpawnerCommandRequestHandler.OnSpawnCubeRequestReceived += OnSpawnCubeRequest;
            offset = world.GetExistingSystem<WorkerSystem>().Origin;
        }

        private async void OnSpawnCubeRequest(CubeSpawner.SpawnCube.ReceivedRequest requestReceived)
        {
            cubeSpawnerCommandRequestHandler.SendSpawnCubeResponse(
                new CubeSpawner.SpawnCube.Response(requestReceived.RequestId, new Empty()));

            var entityReservationSystem = world.GetExistingSystem<EntityReservationSystem>();
            var entityId = await entityReservationSystem.GetAsync();
            SpawnCube(entityId);
        }

        private void SpawnCube(EntityId entityId)
        {
            var location = gameObject.transform.position - offset;
            location.y += 2;

            var cubeEntityTemplate = CubeTemplate.CreateCubeEntityTemplate();

            cubeEntityTemplate.SetComponent(new Position.Snapshot(location.ToCoordinates()));
            cubeEntityTemplate.SetComponent(TransformUtils.CreateTransformSnapshot(location, Quaternion.identity));

            worldCommandRequestSender.SendCreateEntityCommand(
                new WorldCommands.CreateEntity.Request(cubeEntityTemplate, entityId), OnEntityCreated);
        }

        private void OnEntityCreated(WorldCommands.CreateEntity.ReceivedResponse response)
        {
            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent("CreateEntity failed.")
                        .WithField(LoggingUtils.EntityId, response.RequestPayload.EntityId)
                        .WithField("Reason", response.Message));

                return;
            }

            var spawnedCubesCopy =
                new List<EntityId>(cubeSpawnerWriter.Data.SpawnedCubes);
            var newEntityId = response.EntityId.Value;

            spawnedCubesCopy.Add(newEntityId);

            cubeSpawnerWriter.SendUpdate(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubesCopy
            });
        }
    }
}
