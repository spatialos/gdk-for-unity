using System;
using Generated.Playground;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker.Core;
using UnityEngine;

#pragma warning disable 649

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
        worldCommandHelper.Dispose();
        worldCommandHelper = null;
    }

    private void OnOnDeleteSpawnedCubeRequest(CubeSpawner.DeleteSpawnedCube.RequestResponder requestResponder)
    {
        var cubeSpawner = writer.Data;

        var spawnedCubes = CubeSpawnerInputBehaviour.GetSpawnedCubes(cubeSpawner);

        var entityId = requestResponder.Request.Payload.CubeEntityId;

        if (!spawnedCubes.Contains(entityId))
        {
            requestResponder.SendResponseFailure("Entity not found in list.");

            return;
        }

        worldCommandHelper.DeleteEntity(entityId, op =>
        {
            if (op.StatusCode != StatusCode.Success)
            {
                requestResponder.SendResponseFailure($"Could not delete entity: {op.Message}.");
                return;
            }

            // Refresh the list here in case it may have changed between the responses
            spawnedCubes = CubeSpawnerInputBehaviour.GetSpawnedCubes(writer.Data);

            spawnedCubes.Remove(entityId);

            writer.Send(new CubeSpawner.Update
            {
                SpawnedCubes = spawnedCubes,
                NumSpawnedCubes = (uint) spawnedCubes.Count
            });

            requestResponder.SendResponse(new DeleteCubeResponse());
        });
    }
}
