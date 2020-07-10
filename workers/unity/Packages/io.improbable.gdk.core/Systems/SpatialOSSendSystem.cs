using Improbable.Gdk.Core.NetworkStats;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

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

        private NativeList<JobHandle> replicationHandles;

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();
            networkStatisticsSystem = World.GetOrCreateSystem<NetworkStatisticsSystem>();
            replicationHandles = new NativeList<JobHandle>(Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            JobHandle.CompleteAll(replicationHandles);
            replicationHandles.Clear();

            worker.SendMessages(netFrameStats);
            networkStatisticsSystem.AddOutgoingSample(netFrameStats);
            netFrameStats.Clear();
        }

        public void AddReplicationJobProducer(JobHandle job)
        {
            replicationHandles.Add(job);
        }
    }
}
