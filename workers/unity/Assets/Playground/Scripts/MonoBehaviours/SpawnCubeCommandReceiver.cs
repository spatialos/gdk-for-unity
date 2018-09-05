using System.Collections.Generic;
using Generated.Improbable;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.GameObjectRepresentation;
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
        [Require] private WorldCommands.Requirables.WorldCommandRequestSender worldCommandRequestSender;
        [Require] private WorldCommands.Requirables.WorldCommandResponseHandler worldCommandResponseHandler;

        private ILogDispatcher logDispatcher;

        private const string EntityForIdFailedWithMessage = "Create entity (for id {0}) failed with message: \"{1}\"";
        private const string FailedToReserveEntityIdMessage = "Failed to reserve entity id: {0}";

        private readonly HashSet<long> sentRequestIds = new HashSet<long>();

        public void OnEnable()
        {
            commandRequestHandler.OnSpawnCubeRequest += OnSpawnCubeRequest;

            worldCommandResponseHandler.OnReserveEntityIdsResponse += OnEntityIdsReserved;
            worldCommandResponseHandler.OnCreateEntityResponse += OnEntityCreated;

            logDispatcher = GetComponent<SpatialOSComponent>().LogDispatcher;
        }

        private void OnSpawnCubeRequest(CubeSpawner.SpawnCube.RequestResponder requestResponder)
        {
            requestResponder.SendResponse(new Empty());

            sentRequestIds.Add(worldCommandRequestSender.ReserveEntityIds(1));
        }

        private void OnEntityIdsReserved(WorldCommands.ReserveEntityIds.ReceivedResponse response)
        {
            if (!sentRequestIds.Remove(response.RequestId))
            {
                // This response was not for a command from this behaviour.
                return;
            }

            var responseOp = response.Op;
            // TODO make a ticket to make world command responses match with normal command responses

            if (responseOp.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent(string.Format(FailedToReserveEntityIdMessage, responseOp.Message)));

                return;
            }

            var location = transformReader.Data.Location;
            var cubeEntityTemplate =
                CubeTemplate.CreateCubeEntityTemplate(new Coordinates(location.X, location.Y + 2, location.Z));
            var expectedEntityId = responseOp.FirstEntityId.Value;

            sentRequestIds.Add(worldCommandRequestSender.CreateEntity(cubeEntityTemplate, expectedEntityId));
        }

        private void OnEntityCreated(WorldCommands.CreateEntity.ReceivedResponse response)
        {
            if (!sentRequestIds.Remove(response.RequestId))
            {
                // This response was not for a command from this behaviour.
                return;
            }

            var createEntityResponseOp = response.Op;

            if (createEntityResponseOp.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent(string.Format(EntityForIdFailedWithMessage,
                        response.RequestPayload.EntityId,
                        createEntityResponseOp.Message)));

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
        }
    }
}
