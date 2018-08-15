using System;
using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
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
            [ReadOnly] public ComponentDataArray<PlayerCreator.CommandResponders.CreatePlayer> CreatePlayerResponders;
            // TODO: [ReadOnly] public ComponentDataArray<WorldCommandSender> WorldCommandSenders;
        }

        [Inject] private CreatePlayerData createPlayerData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < createPlayerData.Length; i++)
            {
                var requests = createPlayerData.CreatePlayerRequests[i].Requests;
                var responders = createPlayerData.CreatePlayerResponders[i].ResponsesToSend;

                foreach (var request in requests)
                {
                    if (PlayerLifecycleConfig.CreatePlayerEntityTemplate == null)
                    {
                        throw new InvalidOperationException(Errors.PlayerEntityTemplateNotFound);
                    }

                    var playerEntity = PlayerLifecycleConfig.CreatePlayerEntityTemplate(request.CallerAttributeSet,
                        request.RawRequest.Position);
                    // TODO: createPlayerData.WorldCommandSenders[i].SendCreateEntityRequest(playerEntity);

                    // TODO: responders.Add(PlayerCreator.CreatePlayer.Response.CreateResponse(request, new CreatePlayerResponseType(somePlayerEntityIdHere)));
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
