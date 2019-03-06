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

        private bool playerCreationRequested;
        private Vector3 spawnPosition;
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

        public void ResetPlayerCreationArguments()
        {
            spawnPosition = Vector3.zero;
            serializedArguments = null;
        }

        public void SetPlayerCreationArguments(Vector3 spawnPosition, byte[] serializedArguments)
        {
            this.spawnPosition = spawnPosition;
            this.serializedArguments = serializedArguments;
        }

        public void RequestPlayerCreation()
        {
            playerCreationRequested = true;
        }

        protected override void OnUpdate()
        {
            if (PlayerLifecycleConfig.AutoRequestPlayerCreation)
            {
                var initEntities = initializationGroup.GetEntityArray();
                for (var i = 0; i < initEntities.Length; ++i)
                {
                    RequestPlayerCreation();
                }
            }

            if (playerCreationRequested)
            {
                var request = new CreatePlayerRequestType
                {
                    Position = Vector3f.FromUnityVector(spawnPosition)
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
                    RequestPlayerCreation();
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
