using System;
using System.Threading.Tasks;
using Improbable.Common;
using Improbable.Gdk.Core;
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

        private ILogDispatcher logDispatcher;
        private EntityId ownEntityId;

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                await SendSpawnCubeCommand();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                await SendDeleteCubeCommand();
            }
        }

        private async Task SendSpawnCubeCommand()
        {
            var request = new CubeSpawner.SpawnCube.Request(entityId, new Empty());
            try
            {
                var response = await cubeSpawnerCommandSender.SendSpawnCubeCommandAsync(request);
                Debug.Log($"Message: {response.Message}, Request Id: {response.RequestId}");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        private async Task SendDeleteCubeCommand()
        {
            var spawnedCubes = cubeSpawnerReader.Data.SpawnedCubes;

            if (spawnedCubes.Count == 0)
            {
                //logDispatcher.HandleLog(LogType.Log, new LogEvent("No cubes left to delete."));
                return;
            }

            var request = new CubeSpawner.DeleteSpawnedCube.Request(entityId, new DeleteCubeRequest
            {
                CubeEntityId = spawnedCubes[0]
            });

            try
            {
                await cubeSpawnerCommandSender.SendDeleteSpawnedCubeCommandAsync(request);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }
}
