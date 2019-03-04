using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class SendCreatePlayerRequestSystem : ComponentSystem
    {
        private readonly EntityId playerCreatorEntityId = new EntityId(1);

        private CommandSystem commandSystem;
        private WorkerSystem workerSystem;
        private ILogDispatcher logDispatcher;

        private ComponentGroup initializationGroup;
        private ComponentGroup playerSpawnGroup;

        private bool playerCreationRequested;
        private Vector3f spawnPosition;
        private byte[] serializedArguments;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            workerSystem = World.GetExistingManager<WorkerSystem>();
            commandSystem = World.GetExistingManager<CommandSystem>();
            logDispatcher = workerSystem.LogDispatcher;

            initializationGroup = GetComponentGroup(
                ComponentType.ReadOnly<WorkerEntityTag>(),
                ComponentType.ReadOnly<OnConnected>()
            );
        }

        public void QueuePlayerCreationRequest(Vector3 spawnPosition = default, byte[] serializedArguments = null)
        {
            playerCreationRequested = true;
            this.spawnPosition = Vector3f.FromUnityVector(spawnPosition);
            this.serializedArguments = serializedArguments;
        }

        protected override void OnUpdate()
        {
            if (PlayerLifecycleConfig.AutoRequestPlayerCreation)
            {
                var initEntities = initializationGroup.GetEntityArray();
                for (var i = 0; i < initEntities.Length; ++i)
                {
                    QueuePlayerCreationRequest();
                }
            }

            if (playerCreationRequested)
            {
                var request = new CreatePlayerRequestType
                {
                    Position = spawnPosition
                };

                if (serializedArguments != null)
                {
                    request.SerializedArguments = serializedArguments;
                }

                var createPlayerRequest = new PlayerCreator.CreatePlayer.Request(playerCreatorEntityId, request);

                commandSystem.SendCommand(createPlayerRequest);
                playerCreationRequested = false;
            }

            // Currently this has a race condition where you can receive two entities
            // The fix for this is more sophisticated server side handling of requests
            var responses = commandSystem.GetResponses<PlayerCreator.CreatePlayer.ReceivedResponse>();

            for (var i = 0; i < responses.Count; i++)
            {
                ref readonly var response = ref responses[i];
                if (response.StatusCode == StatusCode.AuthorityLost)
                {
                    QueuePlayerCreationRequest();
                }
                else if (response.StatusCode != StatusCode.Success)
                {
                    logDispatcher.HandleLog(LogType.Error, new LogEvent(
                        $"Create player request failed: {response.Message}"
                    ));
                }
            }
        }
    }
}
