using System;
using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandleCreatePlayerRequestSystem : ComponentSystem
    {
        private struct CreatePlayerData
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandRequests.CreatePlayer> CreatePlayerRequests;
            public ComponentDataArray<PlayerCreator.CommandResponders.CreatePlayer> CreatePlayerResponders;
            public ComponentDataArray<WorldCommands.CreateEntity.CommandSender> CreateEntitySender;
        }

        [Inject] private CreatePlayerData createPlayerData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < createPlayerData.Length; i++)
            {
                var requests = createPlayerData.CreatePlayerRequests[i].Requests;
                var responder = createPlayerData.CreatePlayerResponders[i];

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
                        Entity = playerEntity
                    });

                    responder.ResponsesToSend.Add(
                        PlayerCreator.CreatePlayer.Response.CreateResponse(request, new CreatePlayerResponseType()));
                }

                createPlayerData.CreatePlayerResponders[i] = responder;
                createPlayerData.CreateEntitySender[i] = createEntitySender;
            }
        }

        internal static class Errors
        {
            public const string PlayerEntityTemplateNotFound =
                "PlayerLifecycleConfig.CreatePlayerEntityTemplate is not set.";
        }
    }
}
