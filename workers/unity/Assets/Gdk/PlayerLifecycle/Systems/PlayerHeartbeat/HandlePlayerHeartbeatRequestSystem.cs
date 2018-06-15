using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatRequestSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;

            public ComponentArray<CommandRequests<PlayerHeartbeatClient.PlayerHeartbeat.Request>>
                PlayerHeartbeatRequests;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (var i = 0; i < data.Length; i++)
            {
                var requests = data.PlayerHeartbeatRequests[i].Buffer;

                foreach (var request in requests)
                {
                    request.SendPlayerHeartbeatResponse(new Empty());
                }
            }
        }
    }
}
