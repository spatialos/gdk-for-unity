using System;
using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

// Disable the "variable is never assigned" for injected fields.
#pragma warning disable 649

#endregion

namespace Playground.MonoBehaviours
{
    public class CubeSpawnerInputBehaviour : MonoBehaviour
    {
        [Require] private PlayerInputWriter playerInputWriter;
        [Require] private CubeSpawnerReader cubeSpawnerReader;
        [Require] private CubeSpawnerCommandSender cubeSpawnerCommandSender;

        [Require] private EntityId entityId;
        [Require] private World world;

        [Require] private ILogDispatcher logDispatcher;

        private void OnSpawnCubeResponse(CubeSpawner.SpawnCube.ReceivedResponse response)
        {
            if (response.StatusCode != StatusCode.Success)
            {
                logDispatcher.HandleLog(LogType.Error, new LogEvent($"Spawn error: {response.Message}"));
                throw new Exception("Test Exception");
            }
        }

        private void OnDeleteSpawnedCubeResponse(CubeSpawner.DeleteSpawnedCube.ReceivedResponse response)
        {
            switch (response.StatusCode)
            {
                case StatusCode.Success:
                    logDispatcher.HandleLog(LogType.Log, new LogEvent($"Deleting entity"));
                    break;
                case StatusCode.ApplicationError:
                    logDispatcher.HandleLog(LogType.Log, new LogEvent($"Delete refused: {response.Message}"));
                    break;
                case StatusCode.Timeout:
                case StatusCode.NotFound:
                case StatusCode.AuthorityLost:
                case StatusCode.PermissionDenied:
                case StatusCode.InternalError:
                    logDispatcher.HandleLog(LogType.Error, new LogEvent($"Delete error: {response.Message}"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
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
            var request = new CubeSpawner.SpawnCube.Request(entityId, new Empty());
            cubeSpawnerCommandSender.SendSpawnCubeCommand(request, OnSpawnCubeResponse);
        }

        private void SendDeleteCubeCommand()
        {
            var spawnedCubes = cubeSpawnerReader.Data.SpawnedCubes;

            if (spawnedCubes.Count == 0)
            {
                logDispatcher.HandleLog(LogType.Log, new LogEvent("No cubes left to delete."));
                return;
            }

            var request = new CubeSpawner.DeleteSpawnedCube.Request(entityId, new DeleteCubeRequest
            {
                CubeEntityId = spawnedCubes[0]
            });
            cubeSpawnerCommandSender.SendDeleteSpawnedCubeCommand(request, OnDeleteSpawnedCubeResponse);
        }
    }
}
