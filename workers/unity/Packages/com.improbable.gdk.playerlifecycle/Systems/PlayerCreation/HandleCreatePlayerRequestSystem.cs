using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.PlayerLifecycle;
using Improbable.Worker.CInterop;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandleCreatePlayerRequestSystem : ComponentSystem
    {
        private CommandSystem commandSystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            commandSystem = World.GetExistingManager<CommandSystem>();
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
            foreach (var request in requests)
            {
                var playerEntityTemplate = PlayerLifecycleConfig.CreatePlayerEntityTemplate(
                    request.CallerWorkerId,
                    request.Payload.Position
                );

                var entityRequest = new WorldCommands.CreateEntity.Request
                (
                    playerEntityTemplate,
                    context: new PlayerCreationRequestContext
                    {
                        createPlayerRequest = request
                    }
                );

                commandSystem.SendCommand(entityRequest);
            }
        }

        private void HandlePlayerCreateEntityResponses()
        {
            var responses = commandSystem.GetResponses<WorldCommands.CreateEntity.ReceivedResponse>();
            foreach (var response in responses)
            {
                if (!(response.Context is PlayerCreationRequestContext requestContext))
                {
                    // Ignore non-player entity creation requests
                    continue;
                }


                if (response.StatusCode != StatusCode.Success || !response.EntityId.HasValue)
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
                        new CreatePlayerResponseType
                        {
                            CreatedEntityId = response.EntityId.Value
                        }
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
