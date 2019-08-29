using System.Diagnostics;
using Unity.Entities;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        private float lastFrameTime;

#if !UNITY_EDITOR
        protected override void OnCreate()
        {
            Enabled = false;
        }
#endif

        protected override void OnDestroy()
        {
            NetFrameStats.Pool.Return(lastIncomingData);
            NetFrameStats.Pool.Return(lastOutgoingData);
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
            lastIncomingData.CopyFrom(diff.GetNetStats());
        }

        [Conditional("UNITY_EDITOR")]
        internal void AddOutgoingSample(NetFrameStats frameStats)
        {
            lastOutgoingData.CopyFrom(frameStats);
        }
    }
}
