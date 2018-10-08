using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.PlayerLifecycle;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandleCreatePlayerRequestSystem : ComponentSystem
    {
        private struct CreatePlayerData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandRequests.CreatePlayer> CreatePlayerRequests;
            [ReadOnly] public ComponentDataArray<WorldCommands.CreateEntity.CommandSender> CreateEntitySender;
        }

        [Inject] private CreatePlayerData createPlayerData;

        private struct EntityCreationResponseData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<WorldCommands.CreateEntity.CommandResponses> CreateEntityResponses;
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandResponders.CreatePlayer> CreatePlayerResponders;
        }

        [Inject] private EntityCreationResponseData entityCreationResponseData;

        private class PlayerCreationRequestContext
        {
            public PlayerCreator.CreatePlayer.ReceivedRequest createPlayerRequest;
        }

        protected override void OnUpdate()
        {
            for (var i = 0; i < createPlayerData.Length; i++)
            {
                var requests = createPlayerData.CreatePlayerRequests[i].Requests;
                var createEntitySender = createPlayerData.CreateEntitySender[i];

                foreach (var request in requests)
                {
                    if (PlayerLifecycleConfig.CreatePlayerEntityTemplate == null)
                    {
                        throw new InvalidOperationException(Errors.PlayerEntityTemplateNotFound);
                    }

                    var playerEntity = PlayerLifecycleConfig.CreatePlayerEntityTemplate(request.CallerWorkerId,
                        request.Payload.Position);
                    createEntitySender.RequestsToSend.Add(WorldCommands.CreateEntity.CreateRequest
                    (
                        playerEntity,
                        context: new PlayerCreationRequestContext
                        {
                            createPlayerRequest = request
                        }
                    ));
                }
            }

            for (var i = 0; i < entityCreationResponseData.Length; ++i)
            {
                var entityCreationResponses = entityCreationResponseData.CreateEntityResponses[i];
                var responder = entityCreationResponseData.CreatePlayerResponders[i];

                foreach (var receivedResponse in entityCreationResponses.Responses)
                {
                    if (!(receivedResponse.Context is PlayerCreationRequestContext requestContext))
                    {
                        // Ignore non-player entity creation requests
                        continue;
                    }

                    if (receivedResponse.StatusCode != StatusCode.Success || !receivedResponse.EntityId.HasValue)
                    {
                        responder.ResponsesToSend.Add(PlayerCreator.CreatePlayer
                            .CreateResponseFailure(requestContext.createPlayerRequest,
                                $"Failed to create player: \"{receivedResponse.Message}\""));

                        continue;
                    }

                    responder.ResponsesToSend.Add(PlayerCreator.CreatePlayer
                        .CreateResponse(requestContext.createPlayerRequest, new CreatePlayerResponseType
                        {
                            CreatedEntityId = receivedResponse.EntityId.Value
                        }));
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
