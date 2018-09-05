using System.Collections.Generic;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Core.GameObjectRepresentation;
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
        [Require] private CubeSpawner.Requirables.CommandRequestHandler commandRequestHandler;
        [Require] private CubeSpawner.Requirables.Writer writer;
        [Require] private WorldCommands.Requirables.WorldCommandRequestSender worldCommandRequestSender;
        [Require] private WorldCommands.Requirables.WorldCommandResponseHandler worldCommandResponseHandler;

        private ILogDispatcher logDispatcher;

        private const string CouldNotDeleteEntityWithId = "Could not delete entity with id {0}: {1}.";

        private const string TheEntityHasBeenUnexpectedlyRemovedFromTheList =
            "The entity {0} has been unexpectedly removed from the list.";

        private readonly HashSet<long> sentRequestIds = new HashSet<long>();

        public void OnEnable()
        {
            logDispatcher = GetComponent<SpatialOSComponent>().LogDispatcher;

            commandRequestHandler.OnDeleteSpawnedCubeRequest += OnDeleteSpawnedCubeRequest;

            worldCommandResponseHandler.OnDeleteEntityResponse += OnDeleteEntityResponse;
        }

        private void OnDeleteSpawnedCubeRequest(CubeSpawner.DeleteSpawnedCube.RequestResponder requestResponder)
        {
            var entityId = requestResponder.Request.Payload.CubeEntityId;

            var spawnedCubes = CubeSpawnerInputBehaviour.GetSpawnedCubes(writer.Data);
            if (!spawnedCubes.Contains(entityId))
            {
                requestResponder.SendResponseFailure($"Requested entity id {entityId} not found in list.");
            }
            else
            {
                requestResponder.SendResponse(new Empty());
            }

            sentRequestIds.Add(worldCommandRequestSender.DeleteEntity(entityId));
        }


        private void OnDeleteEntityResponse(WorldCommands.DeleteEntity.ReceivedResponse response)
        {
            if (!sentRequestIds.Remove(response.RequestId))
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

            var spawnedCubes = CubeSpawnerInputBehaviour.GetSpawnedCubes(writer.Data);

            if (!spawnedCubes.Remove(entityId))
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent(string.Format(TheEntityHasBeenUnexpectedlyRemovedFromTheList, entityId)));
                return;
            }

            writer.Send(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubes,
                NumSpawnedCubes = (uint) spawnedCubes.Count
            });
        }
    }
}
