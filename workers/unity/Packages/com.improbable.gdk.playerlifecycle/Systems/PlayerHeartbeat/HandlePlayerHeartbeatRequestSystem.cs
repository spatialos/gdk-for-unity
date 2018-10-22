using System;
using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class HandlePlayerHeartbeatRequestSystem : ComponentSystem
    {
        private ComponentGroup group;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            var query = new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.ReadOnly<PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat>(),
                    ComponentType.Create<PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat>(),
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            group = GetComponentGroup(query);
        }

        protected override void OnUpdate()
        {
            var requestsType = GetArchetypeChunkComponentType<PlayerHeartbeatClient.CommandRequests.PlayerHeartbeat>(true);
            var responderType = GetArchetypeChunkComponentType<PlayerHeartbeatClient.CommandResponders.PlayerHeartbeat>();

            var chunkArray = group.CreateArchetypeChunkArray(Allocator.Temp);

            foreach (var chunk in chunkArray)
            {
                var requests = chunk.GetNativeArray(requestsType);
                var responders = chunk.GetNativeArray(responderType);

                for (var i = 0; i < requests.Length; i++)
                {
                    var responsesToSend = responders[i].ResponsesToSend;
                    foreach (var request in requests[i].Requests)
                    {
                        responsesToSend.Add(PlayerHeartbeatClient.PlayerHeartbeat.CreateResponse(request, new Empty()));
                    }
                }
            }

            chunkArray.Dispose();
        }
    }
}
