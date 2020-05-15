using System.Collections.Generic;
using Unity.Profiling;

namespace Improbable.Gdk.Core
{
    public class View
    {
        private readonly HashSet<EntityId> entities = new HashSet<EntityId>();
        public IReadOnlyDictionary<string, string> WorkerFlags => workerFlags;
        private readonly Dictionary<string, string> workerFlags = new Dictionary<string, string>();

        private ProfilerMarker applyDiffMarker = new ProfilerMarker("View.ApplyDiff");

        public HashSet<EntityId> GetEntityIds()
        {
            return entities;
        }

        public bool HasEntity(EntityId entityId)
        {
            return entities.Contains(entityId);
        }

        public string GetWorkerFlag(string name)
        {
            return workerFlags.TryGetValue(name, out var value) ? value : null;
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            using (applyDiffMarker.Auto())
            {
                var entitiesAdded = diff.GetEntitiesAdded();
                foreach (var entity in entitiesAdded)
                {
                    entities.Add(entity);
                }

                var entitiesRemoved = diff.GetEntitiesRemoved();
                foreach (var entity in entitiesRemoved)
                {
                    entities.Remove(entity);
                }

                foreach (var pair in diff.GetWorkerFlagChanges())
                {
                    workerFlags[pair.Item1] = pair.Item2;
                }
            }
        }
    }
}
