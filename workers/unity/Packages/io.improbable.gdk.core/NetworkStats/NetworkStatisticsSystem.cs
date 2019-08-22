using System.Collections.Generic;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public class NetworkStatisticsSystem : ComponentSystem
    {
        private const int DefaultBufferSize = 60;

        private Queue<NetStats> incomingNetworkStats;

        protected override void OnCreate()
        {
            Enabled = false;
            incomingNetworkStats = new Queue<NetStats>(DefaultBufferSize);
        }

        protected override void OnUpdate()
        {
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            if (incomingNetworkStats.Count == DefaultBufferSize)
            {
                NetStats.Pool.Return(incomingNetworkStats.Dequeue());
            }

            var data = NetStats.Pool.Rent();
            data.CopyFrom(diff.GetNetStats());
            incomingNetworkStats.Enqueue(data);
        }
    }
}
