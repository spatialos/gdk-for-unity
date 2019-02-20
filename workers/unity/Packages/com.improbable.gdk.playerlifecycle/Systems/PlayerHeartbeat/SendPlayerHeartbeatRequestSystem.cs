using Improbable.Common;
using Improbable.Gdk.Core;
using Improbable.Gdk.ReactiveComponents;
using Improbable.PlayerLifecycle;
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
        private CommandSystem commandSystem;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            group = GetComponentGroup(
                ComponentType.ReadOnly<PlayerHeartbeatServer.ComponentAuthority>(),
                ComponentType.Create<HeartbeatData>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            );
            group.SetFilter(new PlayerHeartbeatServer.ComponentAuthority(true));

            commandSystem = World.GetExistingManager<CommandSystem>();
        }

        protected override void OnUpdate()
        {
            if (Time.time < timeOfNextHeartbeat)
            {
                return;
            }

            timeOfNextHeartbeat = Time.time + PlayerLifecycleConfig.PlayerHeartbeatIntervalSeconds;
            var spatialIDData = group.GetComponentDataArray<SpatialEntityId>();

            for (var i = 0; i < spatialIDData.Length; i++)
            {
                commandSystem.SendCommand(
                    new PlayerHeartbeatClient.PlayerHeartbeat.Request(spatialIDData[i].EntityId, new Empty()));
            }
        }
    }
}
