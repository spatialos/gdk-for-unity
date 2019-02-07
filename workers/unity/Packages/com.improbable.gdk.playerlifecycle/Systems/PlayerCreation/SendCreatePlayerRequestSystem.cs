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
                ComponentType.ReadOnly<ShouldRequestPlayerTag>()
            );
        }

        protected override void OnUpdate()
        {
            if (!initializationGroup.IsEmptyIgnoreFilter)
            {
                var entities = initializationGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; ++i)
                {
                    PostUpdateCommands.AddComponent(entities[i], new ShouldRequestPlayerTag());
                }
            }

            if (!playerSpawnGroup.IsEmptyIgnoreFilter)
            {
                var entities = playerSpawnGroup.GetEntityArray();
                for (var i = 0; i < entities.Length; ++i)
                {
                    var request = new CreatePlayerRequestType
                    {
                        Position = new Vector3f(0, 0, 0)
                    };

                    var createPlayerRequest = new PlayerCreator.CreatePlayer.Request(playerCreatorEntityId, request);

                    commandSystem.SendCommand(createPlayerRequest);
                    PostUpdateCommands.RemoveComponent<ShouldRequestPlayerTag>(entities[i]);
                }
            }

            // Currently this has a race condition where you can receive two entities
            // The fix for this is more sophisticated server side handling of requests
            var responses = commandSystem.GetResponses<PlayerCreator.CreatePlayer.ReceivedResponse>();
            foreach (var response in responses)
            {
                if (response.StatusCode == StatusCode.AuthorityLost)
                {
                    PostUpdateCommands.AddComponent(response.SendingEntity, new ShouldRequestPlayerTag());
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
