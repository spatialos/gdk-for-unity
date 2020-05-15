using System.Collections.Generic;
using Unity.Profiling;

namespace Improbable.Gdk.Core
{
    public class View
    {
        public IReadOnlyDictionary<string, string> WorkerFlags => workerFlags;
        private readonly Dictionary<string, string> workerFlags = new Dictionary<string, string>();

        private ProfilerMarker applyDiffMarker = new ProfilerMarker("View.ApplyDiff");

        public string GetWorkerFlag(string name)
        {
            return workerFlags.TryGetValue(name, out var value) ? value : null;
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            using (applyDiffMarker.Auto())
            {
                foreach (var pair in diff.GetWorkerFlagChanges())
                {
                    workerFlags[pair.Item1] = pair.Item2;
                }
            }
        }
    }
}
