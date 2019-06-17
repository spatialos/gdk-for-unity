using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSReceiveGroup.InternalSpatialOSReceiveGroup))]
    [UpdateBefore(typeof(SpatialOSReceiveSystem))]
    public class EntitySystem : ComponentSystem
    {
        private readonly List<EntityId> entitiesAdded = new List<EntityId>();
        private readonly List<EntityId> entitiesRemoved = new List<EntityId>();

        private WorkerSystem workerSystem;

        public List<EntityId> GetEntitiesAdded()
        {
            return entitiesAdded;
        }

        public List<EntityId> GetEntitiesRemoved()
        {
            return entitiesRemoved;
        }

        public HashSet<EntityId> GetEntitiesInView()
        {
            return workerSystem.View.GetEntityIds();
        }

        internal void ApplyDiff(ViewDiff diff)
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
