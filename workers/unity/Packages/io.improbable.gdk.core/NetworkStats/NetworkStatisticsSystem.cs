using System.Diagnostics;
using Unity.Entities;

namespace Improbable.Gdk.Core.NetworkStats
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateBefore(typeof(SpatialOSReceiveSystem))]
    public class NetworkStatisticsSystem : ComponentSystem
    {
        private const int DefaultBufferSize = 60;

        private readonly NetStats netStats = new NetStats(DefaultBufferSize);
        private readonly NetFrameStats lastIncomingData = NetFrameStats.Pool.Rent();
        private readonly NetFrameStats lastOutgoingData = NetFrameStats.Pool.Rent();

        protected override void OnDestroy()
        {
            NetFrameStats.Pool.Return(lastIncomingData);
            NetFrameStats.Pool.Return(lastOutgoingData);
        }

        protected override void OnUpdate()
        {
            netStats.AddFrame(lastIncomingData, Direction.Incoming);
            netStats.AddFrame(lastOutgoingData, Direction.Outgoing);

            lastIncomingData.Clear();
            lastOutgoingData.Clear();

            var position = netStats.GetSummary(MessageTypeUnion.Update(54), 10, Direction.Incoming);
            UnityEngine.Debug.Log($"Count: {position.Count}, Size: {position.Size}");
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            lastIncomingData.CopyFrom(diff.GetNetStats());
        }

        internal void AddOutgoingSample(NetFrameStats frameStats)
        {
            lastOutgoingData.CopyFrom(frameStats);
        }
    }
}
