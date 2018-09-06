using System.Collections.Generic;
using Generated.Playground;
using Improbable.Gdk.Core;
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
    public class CubeSpawnerInputBehaviour : MonoBehaviour
    {
        [Require] private PlayerInput.Requirables.Writer playerInputWriter;
        [Require] private CubeSpawner.Requirables.Reader cubeSpawnerReader;
        [Require] private CubeSpawner.Requirables.CommandRequestSender cubeSpawnerCommandSender;
        [Require] private CubeSpawner.Requirables.CommandResponseHandler cubeSpawnerCommandResponseHandler;

        private ILogDispatcher logDispatcher;
        private EntityId ownEntityId;

        public static List<EntityId> GetSpawnedCubes(CubeSpawner.Component spatialOSCubeSpawner)
        {
            if (spatialOSCubeSpawner.NumSpawnedCubes == 0)
            {
                // TODO UTY-1081 remove workaround when lists can be emptied again
                return new List<EntityId>();
            }
            else
            {
                return spatialOSCubeSpawner.SpawnedCubes;
            }
        }

        private void OnEnable()
        {
            var spatialOSComponent = GetComponent<SpatialOSComponent>();
            logDispatcher = spatialOSComponent.Worker.LogDispatcher;
            ownEntityId = spatialOSComponent.SpatialEntityId;

            cubeSpawnerCommandResponseHandler.OnSpawnCubeResponse += OnSpawnCubeResponse;
            cubeSpawnerCommandResponseHandler.OnDeleteSpawnedCubeResponse += OnDeleteSpawnedCubeResponse;
        }

        private void OnSpawnCubeResponse(CubeSpawner.SpawnCube.ReceivedResponse response)
        {
            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent($"Spawn error: {response.Message}"));
            }
        }

        private void OnDeleteSpawnedCubeResponse(CubeSpawner.DeleteSpawnedCube.ReceivedResponse response)
        {
            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent($"Delete error: {response.Message}"));
            }
        }

        private void Update()
        {
            if (playerInputWriter.Authority != Authority.Authoritative)
            {
                // Only send commands if we're the player with input
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendSpawnCubeCommand();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                SendDeleteCubeCommand();
            }
        }

        private void SendSpawnCubeCommand()
        {
            cubeSpawnerCommandSender.SendSpawnCubeRequest(ownEntityId, new Empty());
        }

        private void SendDeleteCubeCommand()
        {
            var spawnedCubes = GetSpawnedCubes(cubeSpawnerReader.Data);

            if (spawnedCubes.Count == 0)
            {
                logDispatcher.HandleLog(LogType.Log, new LogEvent("No cubes left to delete."));
                return;
            }

            cubeSpawnerCommandSender.SendDeleteSpawnedCubeRequest(ownEntityId, new DeleteCubeRequest
            {
                CubeEntityId = spawnedCubes[0]
            });
        }
    }
}
