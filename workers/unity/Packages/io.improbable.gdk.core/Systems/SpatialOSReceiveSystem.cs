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
        private WorkerSystem worker;
        private EcsViewSystem ecsViewSystem;
        private EntitySystem entitySystem;
        private NetworkStatisticsSystem networkStatisticsSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();
            ecsViewSystem = World.GetOrCreateSystem<EcsViewSystem>();
            entitySystem = World.GetOrCreateSystem<EntitySystem>();
            networkStatisticsSystem = World.GetOrCreateSystem<NetworkStatisticsSystem>();
        }

        protected override void OnUpdate()
        {
            try
            {
                worker.Tick();

                var diff = worker.Diff;
                worker.View.ApplyDiff(diff);
                ecsViewSystem.ApplyDiff(diff);
                entitySystem.ApplyDiff(diff);
                networkStatisticsSystem.ApplyDiff(diff);
            }
            catch (Exception e)
            {
                worker.LogDispatcher.HandleLog(LogType.Exception, new LogEvent("Exception:")
                    .WithException(e));
            }
        }
    }
}
