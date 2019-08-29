using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialOSSendSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private NetworkStatisticsSystem networkStatisticsSystem;

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();
            networkStatisticsSystem = World.GetOrCreateSystem<NetworkStatisticsSystem>();
        }

        protected override void OnUpdate()
        {
            var stats = NetFrameStats.Pool.Rent();
            worker.SendMessages(stats);
            networkStatisticsSystem.AddOutgoingSample(stats);
        }
    }
}
