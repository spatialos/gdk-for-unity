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

            playerSpawnGroup = GetComponentGroup(
                ComponentType.ReadOnly<CreatePlayerRequestTrigger>()
            );
        }

        protected override void OnUpdate()
        {
            var initEntities = initializationGroup.GetEntityArray();
            for (var i = 0; i < initEntities.Length; ++i)
            {
                if (PlayerLifecycleConfig.AutoRequestPlayerCreation)
                {
                    var tag = new CreatePlayerRequestTrigger();
                    if (PlayerLifecycleHelper.SerializeArguments(new PlayerCreationParams("playerName"), out var obj))
                    {
                        tag.SerializedArguments = obj;
                    }

                    PostUpdateCommands.AddSharedComponent(initEntities[i], tag);
                }
            }

            var spawnEntities = playerSpawnGroup.GetEntityArray();
            var requestTags = playerSpawnGroup.GetSharedComponentDataArray<CreatePlayerRequestTrigger>();
            for (var i = 0; i < spawnEntities.Length; ++i)
            {
                var request = new CreatePlayerRequestType
                {
                    Position = requestTags[i].SpawnPosition,
                };

                var serializedArguments = requestTags[i].SerializedArguments;
                if (serializedArguments != null)
                {
                    request.SerializedArguments = serializedArguments;
                }

                var createPlayerRequest = new PlayerCreator.CreatePlayer.Request(playerCreatorEntityId, request);

                commandSystem.SendCommand(createPlayerRequest);
                PostUpdateCommands.RemoveComponent<CreatePlayerRequestTrigger>(spawnEntities[i]);
            }

            // Currently this has a race condition where you can receive two entities
            // The fix for this is more sophisticated server side handling of requests
            var responses = commandSystem.GetResponses<PlayerCreator.CreatePlayer.ReceivedResponse>();
            for (var i = 0; i < responses.Count; i++)
            {
                ref readonly var response = ref responses[i];
                if (response.StatusCode == StatusCode.AuthorityLost)
                {
                    PostUpdateCommands.AddSharedComponent(response.SendingEntity, new CreatePlayerRequestTrigger());
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
