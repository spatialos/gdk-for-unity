using System;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;

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
                Profiler.BeginSample("GetMessages");
                worker.Tick();
                Profiler.EndSample();

                var diff = worker.Diff;

                Profiler.BeginSample("ApplyDiff ECS");
                ecsViewSystem.ApplyDiff(diff);
                Profiler.EndSample();

                Profiler.BeginSample("ApplyDiff Entity");
                entitySystem.ApplyDiff(diff);
                Profiler.EndSample();

                Profiler.BeginSample("ApplyDiff Networking Statistics");
                networkStatisticsSystem.ApplyDiff(diff);
                Profiler.EndSample();
            }
            catch (Exception e)
            {
                worker.LogDispatcher.HandleLog(LogType.Exception, new LogEvent("Exception:")
                    .WithException(e));
            }
        }
    }
}
