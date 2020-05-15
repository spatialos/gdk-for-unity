using System;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Receives incoming messages from the SpatialOS runtime.
    /// </summary>
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    public class SpatialOSReceiveSystem : ComponentSystem
    {
        private WorkerSystem workerSystem;
        private EcsViewSystem ecsViewSystem;
        private EntitySystem entitySystem;
        private NetworkStatisticsSystem networkStatisticsSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            workerSystem = World.GetExistingSystem<WorkerSystem>();
            ecsViewSystem = World.GetOrCreateSystem<EcsViewSystem>();
            entitySystem = World.GetOrCreateSystem<EntitySystem>();
            networkStatisticsSystem = World.GetOrCreateSystem<NetworkStatisticsSystem>();
        }

        protected override void OnUpdate()
        {
            try
            {
                workerSystem.Tick();

                var diff = workerSystem.Diff;
                workerSystem.ApplyDiff(diff);
                ecsViewSystem.ApplyDiff(diff);
                entitySystem.ApplyDiff(diff);
                networkStatisticsSystem.ApplyDiff(diff);
            }
            catch (Exception e)
            {
                workerSystem.LogDispatcher.HandleLog(LogType.Exception, new LogEvent("Exception:")
                    .WithException(e));
            }
        }
    }
}
