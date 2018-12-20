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

        private ILogDispatcher logDispatcher;
        private EntityId ownEntityId;

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

        private async void SendSpawnCubeCommand()
        {
            var request = new CubeSpawner.SpawnCube.Request(entityId, new Empty());
            var response = await cubeSpawnerCommandSender.SendSpawnCubeCommand(request);
            Debug.Log($"Message: {response.Message}, Request Id: {response.RequestId}");
        }

        private async void SendDeleteCubeCommand()
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
            await cubeSpawnerCommandSender.SendDeleteSpawnedCubeCommand(request);
        }
    }
}
