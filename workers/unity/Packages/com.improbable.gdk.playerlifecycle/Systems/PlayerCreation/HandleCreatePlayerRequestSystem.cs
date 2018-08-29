using System;
using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandleCreatePlayerRequestSystem : ComponentSystem
    {
        private struct CreatePlayerData
        {
            public readonly int Length;

            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandRequests.CreatePlayer> CreatePlayerRequests;

            public ComponentDataArray<WorldCommands.CreateEntity.CommandSender> CreateEntitySender;
        }

        [Inject] private CreatePlayerData createPlayerData;

        private struct EntityCreationResponses
        {
            public readonly int Length;
            public ComponentDataArray<PlayerCreator.CommandResponders.CreatePlayer> CreatePlayerResponders;
            public ComponentDataArray<WorldCommands.CreateEntity.CommandResponses> CreateEntityResponses;
        }

        [Inject] private EntityCreationResponses entityCreationResponseData;

        private class EntityCreationRequestContext
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

                    var playerEntity = PlayerLifecycleConfig.CreatePlayerEntityTemplate(request.CallerAttributeSet,
                        request.Payload.Position);
                    createEntitySender.RequestsToSend.Add(new WorldCommands.CreateEntity.Request
                    {
                        Entity = playerEntity,
                        Context = new EntityCreationRequestContext
                        {
                            createPlayerRequest = request
                        }
                    });
                }

                createPlayerData.CreateEntitySender[i] = createEntitySender;
            }

            for (var i = 0; i < entityCreationResponseData.Length; ++i)
            {
                var entityCreationResponses = entityCreationResponseData.CreateEntityResponses[i];
                var responder = entityCreationResponseData.CreatePlayerResponders[i];

                foreach (var receivedResponse in entityCreationResponses.Responses)
                {
                    if (!(receivedResponse.Context is EntityCreationRequestContext requestContext))
                    {
                        // Ignore non-player entity creation requests
                        continue;
                    }

                    var op = receivedResponse.Op;

                    if (op.StatusCode != StatusCode.Success || !op.EntityId.HasValue)
                    {
                        responder.ResponsesToSend.Add(PlayerCreator.CreatePlayer
                            .CreateResponseFailure(requestContext.createPlayerRequest,
                                $"Failed to create player: \"{op.Message}\""));

                        continue;
                    }

                    responder.ResponsesToSend.Add(PlayerCreator.CreatePlayer
                        .CreateResponse(requestContext.createPlayerRequest, new CreatePlayerResponseType
                        {
                            CreatedEntityId = op.EntityId.Value
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
