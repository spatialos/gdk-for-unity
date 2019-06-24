using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;
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

        [Require] private ILogDispatcher logDispatcher;
#pragma warning restore 649

        public void OnEnable()
        {
            cubeSpawnerCommandRequestHandler.OnSpawnCubeRequestReceived += OnSpawnCubeRequest;
        }

        private void OnSpawnCubeRequest(CubeSpawner.SpawnCube.ReceivedRequest requestReceived)
        {
            cubeSpawnerCommandRequestHandler.SendSpawnCubeResponse(
                new CubeSpawner.SpawnCube.Response(requestReceived.RequestId, new Empty()));

            var request = new WorldCommands.ReserveEntityIds.Request
            {
                NumberOfEntityIds = 1,
                Context = this,
            };
            worldCommandRequestSender.SendReserveEntityIdsCommand(request, OnEntityIdsReserved);
        }

        private void OnEntityIdsReserved(WorldCommands.ReserveEntityIds.ReceivedResponse response)
        {
            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent("ReserveEntityIds failed.").WithField("Reason", response.Message));

                return;
            }

            var location = gameObject.transform.position;
            location.y += 2;

            var cubeEntityTemplate = CubeTemplate.CreateCubeEntityTemplate();

            cubeEntityTemplate.SetComponent(new Position.Snapshot(location.ToCoordinates()));
            cubeEntityTemplate.SetComponent(TransformUtils.CreateTransformSnapshot(location, Quaternion.identity));

            var expectedEntityId = response.FirstEntityId.Value;

            worldCommandRequestSender.SendCreateEntityCommand(
                new WorldCommands.CreateEntity.Request(cubeEntityTemplate, expectedEntityId), OnEntityCreated);
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
