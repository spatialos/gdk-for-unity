using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.NetworkStats;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public class NetworkStatisticsSystem : ComponentSystem
    {
        private const int DefaultBufferSize = 60;

        private Queue<Dictionary<uint, DataPoint>> updateMetrics;
        private readonly Pool pool = new Pool();

        protected override void OnCreate()
        {
            Enabled = false;
            updateMetrics = new Queue<Dictionary<uint, DataPoint>>(DefaultBufferSize);
        }

        protected override void OnUpdate()
        {
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            if (updateMetrics.Count == DefaultBufferSize)
            {
                pool.Return(updateMetrics.Dequeue());
            }

            var data = pool.Rent();

            foreach (var datapoint in diff.GetNetStats().Updates)
            {
                data[datapoint.Key] = datapoint.Value;
            }

            updateMetrics.Enqueue(data);
        }

        private class Pool
        {
            private readonly Queue<Dictionary<uint, DataPoint>> data =
                new Queue<Dictionary<uint, DataPoint>>();

            public Pool()
            {
                // Pre-populate the pool with DefaultBufferSize dictionaries.
                for (var i = 0; i < DefaultBufferSize; i++)
                {
                    data.Enqueue(ComponentDatabase.IdsToComponents.ToDictionary(pair => pair.Key, pair => new DataPoint()));
                }
            }

            public Dictionary<uint, DataPoint> Rent()
            {
                if (data.Count != 0)
                {
                    return data.Dequeue();
                }

                return ComponentDatabase.IdsToComponents
                    .ToDictionary(
                        pair => pair.Key,
                        pair => new DataPoint());
            }

            public void Return(Dictionary<uint, DataPoint> element)
            {
                foreach (var pair in ComponentDatabase.IdsToComponents)
                {
                    element[pair.Key] = new DataPoint();
                }

                data.Enqueue(element);
            }
        }
    }
}
