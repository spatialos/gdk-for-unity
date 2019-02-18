using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatRequestSystem : ComponentSystem
    {
        private CommandSystem commandSystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            commandSystem = World.GetExistingManager<CommandSystem>();
        }

        protected override void OnUpdate()
        {
            var requests = commandSystem.GetRequests<PlayerHeartbeatClient.PlayerHeartbeat.ReceivedRequest>();
            for (var i = 0; i < requests.Count; i++)
            {
                ref readonly var receivedRequest = ref requests[i];
                var response = new PlayerHeartbeatClient.PlayerHeartbeat.Response
                {
                    RequestId = receivedRequest.RequestId,
                    Payload = new Empty(),
                };

                commandSystem.SendResponse(response);
            }
        }
    }
}
