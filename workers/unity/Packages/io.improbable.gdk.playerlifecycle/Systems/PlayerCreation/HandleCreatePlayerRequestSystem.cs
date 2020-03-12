using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandleCreatePlayerRequestSystem : ComponentSystem
    {
        private CommandSystem commandSystem;
        private EntityReservationSystem entityReservationSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            commandSystem = World.GetExistingSystem<CommandSystem>();
            entityReservationSystem = World.GetOrCreateSystem<EntityReservationSystem>();
        }

        private class PlayerCreationRequestContext
        {
            public PlayerCreator.CreatePlayer.ReceivedRequest createPlayerRequest;
        }

        protected override void OnUpdate()
        {
            HandlePlayerCreateRequests();
            HandlePlayerCreateEntityResponses();
        }

        private void HandlePlayerCreateRequests()
        {
            if (PlayerLifecycleConfig.CreatePlayerEntityTemplate == null)
            {
                throw new InvalidOperationException(Errors.PlayerEntityTemplateNotFound);
            }

            var requests = commandSystem.GetRequests<PlayerCreator.CreatePlayer.ReceivedRequest>();
            for (var i = 0; i < requests.Count; ++i)
            {
                ref readonly var request = ref requests[i];

                SpawnPlayerEntity(request);
            }
        }

        private async void SpawnPlayerEntity(PlayerCreator.CreatePlayer.ReceivedRequest receivedRequest)
        {
            var entityId = await entityReservationSystem.GetAsync();

            var playerEntityTemplate = PlayerLifecycleConfig.CreatePlayerEntityTemplate(
                entityId,
                receivedRequest.CallerWorkerId,
                receivedRequest.Payload.SerializedArguments
            );

            var entityRequest = new WorldCommands.CreateEntity.Request
            (
                playerEntityTemplate,
                entityId,
                context: new PlayerCreationRequestContext
                {
                    createPlayerRequest = receivedRequest
                }
            );

            commandSystem.SendCommand(entityRequest);
        }

        private void HandlePlayerCreateEntityResponses()
        {
            var responses = commandSystem.GetResponses<WorldCommands.CreateEntity.ReceivedResponse>();
            for (var i = 0; i < responses.Count; i++)
            {
                ref readonly var response = ref responses[i];
                if (!(response.Context is PlayerCreationRequestContext requestContext))
                {
                    // Ignore non-player entity creation requests
                    continue;
                }

                if (response.StatusCode != StatusCode.Success)
                {
                    var responseFailed = new PlayerCreator.CreatePlayer.Response(
                        requestContext.createPlayerRequest.RequestId,
                        $"Failed to create player: \"{response.Message}\""
                    );

                    commandSystem.SendResponse(responseFailed);
                }
                else
                {
                    var responseSuccess = new PlayerCreator.CreatePlayer.Response(
                        requestContext.createPlayerRequest.RequestId,
                        new CreatePlayerResponse(response.EntityId.Value)
                    );

                    commandSystem.SendResponse(responseSuccess);
                }
            }
        }

        internal static class Errors
        {
            public const string PlayerEntityTemplateNotFound =
                "PlayerLifecycleConfig.CreatePlayerEntityTemplate is not set.";
        }
    }
}
