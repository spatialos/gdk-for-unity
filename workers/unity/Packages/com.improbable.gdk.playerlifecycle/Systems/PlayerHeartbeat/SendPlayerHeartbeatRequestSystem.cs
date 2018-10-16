using System;
using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Improbable.Gdk.PlayerLifecycle
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(SpatialOSUpdateGroup))]
    public class SendPlayerHeartbeatRequestSystem : ComponentSystem
    {
        private float timeOfNextHeartbeat = Time.time + PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds;
        private ComponentGroup group;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            var query = new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat>(),
                    ComponentType.ReadOnly<Authoritative<PlayerHeartbeatServer.Component>>(),
                    ComponentType.ReadOnly<HeartbeatData>(),
                    ComponentType.ReadOnly<SpatialEntityId>(),
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>()
            };

            group = GetComponentGroup(query);
        }

        protected override void OnUpdate()
        {
            if (Time.time < timeOfNextHeartbeat)
            {
                return;
            }

            timeOfNextHeartbeat = Time.time + PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds;

            var chunkArray = group.CreateArchetypeChunkArray(Allocator.Temp);

            var senderType = GetArchetypeChunkComponentType<PlayerHeartbeatClient.CommandSenders.PlayerHeartbeat>();
            var spatialIdType = GetArchetypeChunkComponentType<SpatialEntityId>(true);

            foreach (var chunk in chunkArray)
            {
                var requestSenders = chunk.GetNativeArray(senderType);
                var spatialIds = chunk.GetNativeArray(spatialIdType);

                for (var i = 0; i < requestSenders.Length; i++)
                {
                    requestSenders[i].RequestsToSend
                        .Add(PlayerHeartbeatClient.PlayerHeartbeat.CreateRequest(spatialIds[i].EntityId, new Empty()));
                }
            }

            chunkArray.Dispose();
        }
    }
}
