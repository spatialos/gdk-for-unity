using Generated.Improbable;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;
using Transform = Generated.Improbable.Transform.Transform;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

// ReSharper disable PossibleInvalidOperationException

#endregion

namespace Playground.MonoBehaviours
{
    public class SpawnCubeCommandReceiver : MonoBehaviour
    {
        [Require] private Transform.Requirables.Reader transformReader;
        [Require] private CubeSpawner.Requirables.CommandRequestHandler commandRequestHandler;
        [Require] private CubeSpawner.Requirables.Writer writer;
        [Require] private WorldCommands.WorldCommandRequestSender worldCommandRequestSender;
        [Require] private WorldCommands.WorldCommandResponseHandler worldCommandResponseHandler;

        private ILogDispatcher logDispatcher;
        private WorldCommandHelper worldCommandHelper;

        private const string EntityForIdFailedWithMessage = "Create entity (for id {0}) failed with message: \"{1}\"";
        private const string FailedToReserveEntityIdMessage = "Failed to reserve entity id: {0}";

        public void OnEnable()
        {
            commandRequestHandler.OnSpawnCubeRequest += OnSpawnCubeRequest;

            worldCommandHelper = new WorldCommandHelper(
                worldCommandRequestSender,
                worldCommandResponseHandler
            );

            logDispatcher = GetComponent<SpatialOSComponent>().LogDispatcher;
        }

        public void OnDisable()
        {
            worldCommandHelper?.Dispose();
            worldCommandHelper = null;
        }

        private void OnSpawnCubeRequest(CubeSpawner.SpawnCube.RequestResponder requestResponder)
        {
            worldCommandHelper.ReserveEntityIds(1,
                reserveResponse => OnEntityReserved(requestResponder, reserveResponse));
        }

        private void OnEntityReserved(CubeSpawner.SpawnCube.RequestResponder requestResponder,
            ReserveEntityIdsResponseOp reserveResponse)
        {
            if (reserveResponse.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent(string.Format(FailedToReserveEntityIdMessage, reserveResponse.Message)));
                requestResponder.SendResponseFailure("Could not reserve an entity id.");

                return;
            }

            var location = transformReader.Data.Location;
            var cubeEntityTemplate =
                CubeTemplate.CreateCubeEntityTemplate(new Coordinates(location.X, location.Y + 2, location.Z));
            var expectedEntityId = reserveResponse.FirstEntityId.Value;

            worldCommandHelper.CreateEntity(cubeEntityTemplate, expectedEntityId,
                createEntityResponseOp => OnEntityCreated(expectedEntityId, requestResponder, createEntityResponseOp));
        }

        private void OnEntityCreated(
            EntityId expectedEntityId,
            CubeSpawner.SpawnCube.RequestResponder requestResponder,
            CreateEntityResponseOp createEntityResponseOp)
        {
            if (createEntityResponseOp.StatusCode != StatusCode.Success)
            {
                var errorMessage = createEntityResponseOp.Message;
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent(string.Format(EntityForIdFailedWithMessage, expectedEntityId, errorMessage)));

                requestResponder.SendResponseFailure("Could not create entity.");
                return;
            }

            var spawnedCubes = CubeSpawnerInputBehaviour.GetSpawnedCubes(writer.Data);
            var newEntityId = createEntityResponseOp.EntityId.Value;

            spawnedCubes.Add(newEntityId);

            writer.Send(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubes,
                NumSpawnedCubes = (uint) spawnedCubes.Count
            });

            requestResponder.SendResponse(new Empty());
        }
    }
}
