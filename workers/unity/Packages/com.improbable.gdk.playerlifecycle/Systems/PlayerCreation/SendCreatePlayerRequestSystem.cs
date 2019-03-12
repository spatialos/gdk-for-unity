using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class SendCreatePlayerRequestSystem : ComponentSystem
    {
        private readonly EntityId playerCreatorEntityId = new EntityId(1);

        private CommandSystem commandSystem;
        private WorkerSystem workerSystem;
        private ILogDispatcher logDispatcher;

        private ComponentGroup initializationGroup;

        private byte[] serializedArgumentsCache;

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

        public void RequestPlayerCreation(byte[] serializedArguments = null)
        {
            serializedArgumentsCache = serializedArguments;
            var request = new CreatePlayerRequestType(serializedArguments);
            var createPlayerRequest = new PlayerCreator.CreatePlayer.Request(playerCreatorEntityId, request);
            commandSystem.SendCommand(createPlayerRequest);
        }

        private void RetryCreatePlayerRequest()
        {
            RequestPlayerCreation(serializedArgumentsCache);
        }

        protected override void OnUpdate()
        {
            if (PlayerLifecycleConfig.AutoRequestPlayerCreation && !initializationGroup.IsEmptyIgnoreFilter)
            {
                RequestPlayerCreation();
            }

            // Currently this has a race condition where you can receive two entities
            // The fix for this is more sophisticated server side handling of requests
            var responses = commandSystem.GetResponses<PlayerCreator.CreatePlayer.ReceivedResponse>();

            for (var i = 0; i < responses.Count; i++)
            {
                ref readonly var response = ref responses[i];
                if (response.StatusCode == StatusCode.AuthorityLost)
                {
                    RetryCreatePlayerRequest();
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
