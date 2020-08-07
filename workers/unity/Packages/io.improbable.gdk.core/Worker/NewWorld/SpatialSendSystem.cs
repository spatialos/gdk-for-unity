using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;
using Unity.Profiling;

namespace Improbable.Gdk.Core.NewWorld
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class SpatialSendSystem : ComponentSystem
    {
        private SpatialOSWorker worker;
        private readonly NetFrameStats netFrameStats = new NetFrameStats();
        private ProfilerMarker sendMessagesMarker = new ProfilerMarker("SpatialSend.SendMessages");

        protected override void OnCreate()
        {
            base.OnCreate();
            worker = World.GetWorker();
        }

        protected override void OnUpdate()
        {
            using (sendMessagesMarker.Auto())
            {
                worker.EnsureMessagesFlushed(netFrameStats); // maybe no need to copy
            }

            worker.NetworkStatistics.ApplyOutgoingDiff(netFrameStats);
            netFrameStats.Clear();
        }
    }
}
