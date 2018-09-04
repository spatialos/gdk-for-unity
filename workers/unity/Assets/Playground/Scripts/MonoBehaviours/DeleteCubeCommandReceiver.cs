using Generated.Playground;
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
        [Require] private WorldCommands.WorldCommandRequestSender worldCommandRequestSender;
        [Require] private WorldCommands.WorldCommandResponseHandler worldCommandResponseHandler;
        [Require] private CubeSpawner.Requirables.CommandRequestHandler commandRequestHandler;
        [Require] private CubeSpawner.Requirables.Writer writer;

        private WorldCommandHelper worldCommandHelper;

        public void OnEnable()
        {
            commandRequestHandler.OnDeleteSpawnedCubeRequest += OnOnDeleteSpawnedCubeRequest;

            worldCommandHelper = new WorldCommandHelper(
                worldCommandRequestSender,
                worldCommandResponseHandler);
        }

        public void OnDisable()
        {
            worldCommandHelper?.Dispose();
            worldCommandHelper = null;
        }

        private void OnOnDeleteSpawnedCubeRequest(CubeSpawner.DeleteSpawnedCube.RequestResponder requestResponder)
        {
            var cubeSpawner = writer.Data;
            var spawnedCubes = CubeSpawnerInputBehaviour.GetSpawnedCubes(cubeSpawner);
            var entityId = requestResponder.Request.Payload.CubeEntityId;

            if (!spawnedCubes.Contains(entityId))
            {
                requestResponder.SendResponseFailure($"Entity id {entityId} not found in list.");

                return;
            }

            worldCommandHelper.DeleteEntity(entityId, op => OnDeleteResponse(op, requestResponder, entityId));
        }

        private void OnDeleteResponse(DeleteEntityResponseOp op,
            CubeSpawner.DeleteSpawnedCube.RequestResponder requestResponder,
            EntityId entityId)
        {
            if (op.StatusCode != StatusCode.Success)
            {
                requestResponder.SendResponseFailure($"Could not delete entity: {op.Message}.");
                return;
            }

            // Refresh the list here in case it may have changed between the responses
            var spawnedCubes = CubeSpawnerInputBehaviour.GetSpawnedCubes(writer.Data);

            if (!spawnedCubes.Remove(entityId))
            {
                requestResponder.SendResponseFailure(
                    $"The entity {entityId} has been unexpectedly removed from the list.");
                return;
            }

            writer.Send(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubes,
                NumSpawnedCubes = (uint) spawnedCubes.Count
            });

            requestResponder.SendResponse(new Empty());
        }
    }
}
