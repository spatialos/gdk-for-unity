using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

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
        [Require] private CubeSpawnerCommandReceiver cubeSpawnerCommandResponseHandler;

        [Require] private EntityId entityId;
        [Require] private World world;

        private ILogDispatcher logDispatcher;
        private EntityId ownEntityId;

        private void OnEnable()
        {
            //cubeSpawnerCommandResponseHandler.OnSpawnCubeResponse += OnSpawnCubeResponse;
            //cubeSpawnerCommandResponseHandler.OnDeleteSpawnedCubeResponse += OnDeleteSpawnedCubeResponse;
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
            var request = CubeSpawner.SpawnCube.CreateRequest(entityId, new Empty());
            cubeSpawnerCommandSender.SendSpawnCubeCommand(request);
        }

        private void SendDeleteCubeCommand()
        {
            var spawnedCubes = cubeSpawnerReader.Data.SpawnedCubes;

            if (spawnedCubes.Count == 0)
            {
                logDispatcher.HandleLog(LogType.Log, new LogEvent("No cubes left to delete."));
                return;
            }

            var request = CubeSpawner.DeleteSpawnedCube.CreateRequest(entityId, new DeleteCubeRequest
            {
                CubeEntityId = spawnedCubes[0]
            });
            cubeSpawnerCommandSender.SendDeleteSpawnedCubeCommand(request);
        }
    }
}
