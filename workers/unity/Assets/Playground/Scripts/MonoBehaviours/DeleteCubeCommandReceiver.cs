using System.Collections.Generic;
using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.GameObjectRepresentation;
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
        [Require] private CubeSpawner.Requirable.Writer cubeSpawnerWriter;
        [Require] private CubeSpawner.Requirable.CommandRequestHandler cubeSpawnerCommandRequestHandler;
        [Require] private WorldCommands.Requirable.WorldCommandRequestSender worldCommandRequestSender;
        [Require] private WorldCommands.Requirable.WorldCommandResponseHandler worldCommandResponseHandler;

        private ILogDispatcher logDispatcher;

        public void OnEnable()
        {
            logDispatcher = GetComponent<SpatialOSComponent>().Worker.LogDispatcher;
            cubeSpawnerCommandRequestHandler.OnDeleteSpawnedCubeRequest += OnDeleteSpawnedCubeRequest;
            worldCommandResponseHandler.OnDeleteEntityResponse += OnDeleteEntityResponse;
        }

        private void OnDeleteSpawnedCubeRequest(CubeSpawner.DeleteSpawnedCube.RequestResponder requestResponder)
        {
            var entityId = requestResponder.Request.Payload.CubeEntityId;
            var spawnedCubes = cubeSpawnerWriter.Data.SpawnedCubes;

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

            var entityId = response.RequestPayload.EntityId;

            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
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

            cubeSpawnerWriter.Send(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubesCopy
            });
        }
    }
}
