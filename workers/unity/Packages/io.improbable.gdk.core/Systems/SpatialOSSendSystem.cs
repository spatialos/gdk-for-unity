using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialOSSendSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private NetworkStatisticsSystem networkStatisticsSystem;
        private NetFrameStats netFrameStats = new NetFrameStats();

        private JobHandle replicationHandle;

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();
            networkStatisticsSystem = World.GetOrCreateSystem<NetworkStatisticsSystem>();
        }

        protected override void OnUpdate()
        {
            replicationHandle.Complete();
            replicationHandle = default;

            worker.SendMessages(netFrameStats);
            networkStatisticsSystem.AddOutgoingSample(netFrameStats);
            netFrameStats.Clear();
        }

        public void AddReplicationJobProducer(JobHandle job)
        {
            replicationHandle = JobHandle.CombineDependencies(replicationHandle, job);
        }
    }
}
