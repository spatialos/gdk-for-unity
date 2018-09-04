using System;
using System.Collections.Generic;
using Generated.Improbable;
using Generated.Improbable.Transform;
using Generated.Playground;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Improbable.Worker;
using Improbable.Worker.Core;
using Playground;
using UnityEngine;
using Quaternion = Generated.Improbable.Transform.Quaternion;
using Transform = Generated.Improbable.Transform.Transform;

#pragma warning disable 649

public class SpawnCubeCommandReceiver : MonoBehaviour
{
    [Require] private Transform.Requirables.Reader transformReader;

    [Require] private CubeSpawner.Requirables.CommandRequestHandler commandRequestHandler;
    [Require] private CubeSpawner.Requirables.Writer writer;

    [Require] private WorldCommands.WorldCommandRequestSender worldCommandRequestSender;
    [Require] private WorldCommands.WorldCommandResponseHandler worldCommandResponseHandler;

    private ILogDispatcher logDispatcher;
    private WorldCommandHelper worldCommandHelper;

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
        worldCommandHelper.Dispose();
        worldCommandHelper = null;

        commandRequestHandler.OnSpawnCubeRequest -= OnSpawnCubeRequest;
    }

    private void OnSpawnCubeRequest(CubeSpawner.SpawnCube.RequestResponder requestResponder)
    {
        worldCommandHelper.ReserveEntityIds(1, reserveResponse =>
        {
            if (reserveResponse.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error,
                    new LogEvent($"Failed to reserve entity id: {reserveResponse.Message}"));
                requestResponder.SendResponseFailure("Could not reserve entity id");

                return;
            }

            var location = transformReader.Data.Location;

            var cubeEntityTemplate =
                CubeTemplate.CreateCubeEntityTemplate(new Coordinates(location.X, location.Y + 2, location.Z));

            worldCommandHelper.CreateEntity(cubeEntityTemplate, reserveResponse.FirstEntityId,
                createEntityResponseOp =>
                {
                    if (createEntityResponseOp.StatusCode != StatusCode.Success ||
                        createEntityResponseOp.EntityId == null)
                    {
                        logDispatcher.HandleLog(LogType.Error,
                            new LogEvent($"Create entity failed with message: \"{createEntityResponseOp.Message}\""));

                        requestResponder.SendResponseFailure("Could not create entity");

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
                });
        });
    }
}
