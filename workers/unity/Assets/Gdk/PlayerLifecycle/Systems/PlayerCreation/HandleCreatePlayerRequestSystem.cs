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
        public struct Data
        {
            public int Length;
            public ComponentArray<CommandRequests<PlayerCreator.CreatePlayer.Request>> CreatePlayerRequests;
            [ReadOnly] public ComponentDataArray<WorldCommandSender> WorldCommandSenders;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var requests = data.CreatePlayerRequests[i].Buffer;

                foreach (var request in requests)
                {
                    if (PlayerLifecycleConfig.CreatePlayerEntityTemplate == null)
                    {
                        throw new InvalidOperationException(Errors.PlayerEntityTemplateNotFound);
                    }

                    var playerEntity = PlayerLifecycleConfig.CreatePlayerEntityTemplate(request.CallerAttributeSet,
                        request.RawRequest.Position);
                    data.WorldCommandSenders[i].SendCreateEntityRequest(playerEntity);
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
