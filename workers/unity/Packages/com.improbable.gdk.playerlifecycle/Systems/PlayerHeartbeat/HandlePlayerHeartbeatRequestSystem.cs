using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatRequestSystem : ComponentSystem
    {
        private CommandSystem commandSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            commandSystem = World.GetExistingSystem<CommandSystem>();
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
