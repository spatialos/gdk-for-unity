using System.Diagnostics;
using Unity.Entities;
using Unity.Profiling;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Improbable.Gdk.Core.NetworkStats
{
    /*
     *     This system runs before the receive system such that the order is:
     *
     *     NetworkStatisticsSystem -> SpatialOSReceiveSystem -> ... -> SpatialOSSendSystem
     *
     *     When both SpatialOSReceiveSystem and SpatialOSSendSystem runs they provide
     *     network data to this system. This will store them until the next time it runs when it
     *     will push this data in the underlying data storage and reset the temporary storage.
     */
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateBefore(typeof(SpatialOSReceiveSystem))]
    public class NetworkStatisticsSystem : ComponentSystem
    {
        private const int DefaultBufferSize = 60;

        private readonly NetStats netStats = new NetStats(DefaultBufferSize);
        private readonly NetFrameStats lastIncomingData = new NetFrameStats();
        private readonly NetFrameStats lastOutgoingData = new NetFrameStats();
        private ProfilerMarker applyDiffMarker = new ProfilerMarker("NetworkStatisticsSystem.ApplyDiff");

        private float lastFrameTime;

#if !UNITY_EDITOR
        protected override void OnCreate()
        {
            Enabled = false;
        }
#endif

        public (DataPoint, float) GetSummary(MessageTypeUnion messageType, int numFrames, Direction direction)
        {
            return netStats.GetSummary(messageType, numFrames, direction);
        }

        protected override void OnUpdate()
        {
            netStats.SetFrameStats(lastIncomingData, Direction.Incoming);
            netStats.SetFrameStats(lastOutgoingData, Direction.Outgoing);
            netStats.SetFrameTime(lastFrameTime);
            netStats.FinishFrame();

            lastIncomingData.Clear();
            lastOutgoingData.Clear();
            lastFrameTime = Time.deltaTime;
        }

        [Conditional("UNITY_EDITOR")]
        internal void ApplyDiff(ViewDiff diff)
        {
            using (applyDiffMarker.Auto())
            {
                lastIncomingData.CopyFrom(diff.GetNetStats());
            }
        }

        [Conditional("UNITY_EDITOR")]
        internal void AddOutgoingSample(NetFrameStats frameStats)
        {
            lastOutgoingData.CopyFrom(frameStats);
        }
    }
}
