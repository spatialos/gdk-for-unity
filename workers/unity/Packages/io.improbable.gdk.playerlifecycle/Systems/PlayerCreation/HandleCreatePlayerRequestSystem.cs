using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using Unity.Entities;
using Entity = Improbable.Worker.CInterop.Entity;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandleCreatePlayerRequestSystem : ComponentSystem
    {
        private WorkerSystem workerSystem;
        private CommandSystem commandSystem;
        private EntityReservationSystem entityReservationSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            commandSystem = World.GetExistingSystem<CommandSystem>();
            entityReservationSystem = World.GetExistingSystem<EntityReservationSystem>();
            workerSystem = World.GetExistingSystem<WorkerSystem>();
        }

        private class PlayerCreationRequestContext
        {
            public PlayerCreator.CreatePlayer.ReceivedRequest CreatePlayerRequest;
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

            var context = new PlayerCreationRequestContext { CreatePlayerRequest = receivedRequest };

            if (!IsContextValid(context))
            {
                return;
            }

            var playerEntityTemplate = PlayerLifecycleConfig.CreatePlayerEntityTemplate(
                entityId,
                receivedRequest.CallerWorkerEntityId,
                receivedRequest.Payload.SerializedArguments
            );

            var entityRequest = new WorldCommands.CreateEntity.Request
            (
                playerEntityTemplate,
                entityId,
                context: context
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
                        requestContext.CreatePlayerRequest.RequestId,
                        $"Failed to create player: \"{response.Message}\""
                    );

                    commandSystem.SendResponse(responseFailed);
                }
                else
                {
                    // Need to check if the player creator has migrated, the client will have received an `AuthorityLost`
                    // response so will try again. This will result in 2 players. So we should delete this one.
                    if (!IsContextValid(requestContext))
                    {
                        commandSystem.SendCommand(new WorldCommands.DeleteEntity.Request(response.EntityId.Value));
                    }
                    else
                    {
                        var responseSuccess = new PlayerCreator.CreatePlayer.Response(
                            requestContext.CreatePlayerRequest.RequestId,
                            new CreatePlayerResponse(response.EntityId.Value)
                        );

                        commandSystem.SendResponse(responseSuccess);
                    }
                }
            }
        }

        private bool IsContextValid(PlayerCreationRequestContext ctx)
        {
            return workerSystem.TryGetEntity(ctx.CreatePlayerRequest.EntityId, out var entity) &&
                EntityManager.HasComponent<PlayerCreator.HasAuthority>(entity);
        }

        private static class Errors
        {
            public const string PlayerEntityTemplateNotFound =
                "PlayerLifecycleConfig.CreatePlayerEntityTemplate is not set.";
        }
    }
}
