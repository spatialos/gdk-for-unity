using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public class MetricsFrame
    {
        internal readonly Dictionary<uint, ComponentMetrics> UpdateMetrics = new Dictionary<uint, ComponentMetrics>();

        public void AddUpdate(uint componentId, uint size)
        {
            if (!UpdateMetrics.TryGetValue(componentId, out var metrics))
            {
                metrics = new ComponentMetrics();
            }

            metrics.Count += 1;
            metrics.Size += size;

            UpdateMetrics[componentId] = metrics;
        }

        public ComponentMetrics GetComponentMetrics(uint componentId)
        {
            return UpdateMetrics.TryGetValue(componentId, out var metrics) ? metrics : new ComponentMetrics();
        }

        public void Clear()
        {
            UpdateMetrics.Clear();
        }

        public struct ComponentMetrics
        {
            public uint Count;
            public uint Size;
        }
    }
}
