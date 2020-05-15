using System.Collections.Generic;
using Unity.Entities;
using Unity.Profiling;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateBefore(typeof(SpatialOSReceiveSystem))]
    public class EntitySystem : ComponentSystem
    {
        public int ViewVersion { get; private set; }

        private readonly List<EntityId> entitiesAdded = new List<EntityId>();
        private readonly List<EntityId> entitiesRemoved = new List<EntityId>();

        private ProfilerMarker applyDiffMarker = new ProfilerMarker("EntitySystem.ApplyDiff");

        private WorkerSystem workerSystem;

        public List<EntityId> GetEntitiesAdded()
        {
            return entitiesAdded;
        }

        public List<EntityId> GetEntitiesRemoved()
        {
            return entitiesRemoved;
        }

        internal void ApplyDiff(ViewDiff diff)
        {
            using (applyDiffMarker.Auto())
            {
                entitiesAdded.Clear();
                entitiesRemoved.Clear();

                // todo decide on a container and remove this
                foreach (var entityId in diff.GetEntitiesAdded())
                {
                    entitiesAdded.Add(entityId);
                }

                foreach (var entityId in diff.GetEntitiesRemoved())
                {
                    entitiesRemoved.Add(entityId);
                }

                if (entitiesAdded.Count != 0 || entitiesRemoved.Count != 0)
                {
                    ViewVersion += 1;
                }
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            workerSystem = World.GetExistingSystem<WorkerSystem>();
        }

        protected override void OnUpdate()
        {
            entitiesAdded.Clear();
            entitiesRemoved.Clear();
        }
    }
}
