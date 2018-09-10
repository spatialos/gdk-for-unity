using System.Collections.Generic;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

#endregion

namespace Playground.MonoBehaviours
{
    public class DeleteCubeCommandReceiver : MonoBehaviour
    {
        [Require] private CubeSpawner.Requirables.Writer cubeSpawnerWriter;
        [Require] private CubeSpawner.Requirables.CommandRequestHandler cubeSpawnerCommandRequestHandler;
        [Require] private WorldCommands.Requirables.WorldCommandRequestSender worldCommandRequestSender;
        [Require] private WorldCommands.Requirables.WorldCommandResponseHandler worldCommandResponseHandler;

        private ILogDispatcher logDispatcher;
        private const string CouldNotDeleteEntityWithId = "Could not delete entity with id {0}: {1}.";

        public void OnEnable()
        {
            logDispatcher = GetComponent<SpatialOSComponent>().Worker.LogDispatcher;
            cubeSpawnerCommandRequestHandler.OnDeleteSpawnedCubeRequest += OnDeleteSpawnedCubeRequest;
            worldCommandResponseHandler.OnDeleteEntityResponse += OnDeleteEntityResponse;
        }

        private void OnDeleteSpawnedCubeRequest(CubeSpawner.DeleteSpawnedCube.RequestResponder requestResponder)
        {
            var entityId = requestResponder.Request.Payload.CubeEntityId;
            var spawnedCubes = CubeSpawnerInputBehaviour.GetSpawnedCubes(cubeSpawnerWriter.Data);

            if (!spawnedCubes.Contains(entityId))
            {
                requestResponder.SendResponseFailure($"Requested entity id {entityId} not found in list.");
            }
            else
            {
                requestResponder.SendResponse(new Empty());
            }

            worldCommandRequestSender.DeleteEntity(entityId, context: this);
        }


        private void OnDeleteEntityResponse(WorldCommands.DeleteEntity.ReceivedResponse response)
        {
            if (!ReferenceEquals(this, response.Context))
            {
                // This response was not for a command from this behaviour.
                return;
            }

            var op = response.Op;
            var entityId = response.RequestPayload.EntityId;

            if (op.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent(string.Format(CouldNotDeleteEntityWithId,
                        entityId, op.Message)));
                return;
            }

            var spawnedCubesCopy =
                new List<EntityId>(CubeSpawnerInputBehaviour.GetSpawnedCubes(cubeSpawnerWriter.Data));

            if (!spawnedCubesCopy.Remove(entityId))
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent(string.Format("The entity {0} has been unexpectedly removed from the list.",
                        entityId)));
                return;
            }

            cubeSpawnerWriter.Send(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubesCopy,
                NumSpawnedCubes = (uint) spawnedCubesCopy.Count
            });
        }
    }
}
