using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatRequestSystem : ComponentSystem
    {
        private struct HeartbeatData
        {
            public readonly int Length;

            [ReadOnly] public ComponentDataArray<PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat>
                HeartbeatRequests;

            [ReadOnly] public ComponentDataArray<PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat>
                HeartbeatResponders;
        }

        [Inject] private HeartbeatData heartbeatData;

        protected override void OnUpdate()
        {
            for (var i = 0; i < heartbeatData.Length; i++)
            {
                var requests = heartbeatData.HeartbeatRequests[i].Requests;
                var responder = heartbeatData.HeartbeatResponders[i].ResponsesToSend;

                foreach (var request in requests)
                {
                    responder.Add(PlayerHeartbeatClient.PlayerHeartbeat.Response.CreateResponse(request, new Empty()));
                }
            }
        }
    }
}
