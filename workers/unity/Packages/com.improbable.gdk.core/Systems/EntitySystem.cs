using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateBefore(typeof(SpatialOSReceiveSystem))]
    public class EntitySystem : ComponentSystem
    {
        private readonly List<IComponentManager> managers = new List<IComponentManager>();

        private readonly HashSet<EntityId> localEntities = new HashSet<EntityId>();

        private readonly List<EntityId> entitiesAdded = new List<EntityId>();
        private readonly List<EntityId> entitiesRemoved = new List<EntityId>();

        // todo would like to make these all readonly
        // don't think it can be done without allocation without new types
        // could make these spans too + some special type for a set of entities
        // might also want to keep things sorted although it's not faster unless there are a lot of entities added and removed in one tick
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
            return localEntities;
        }

        internal void AddEntity(EntityId entityId)
        {
            if (!entitiesRemoved.Remove(entityId))
            {
                entitiesAdded.Add(entityId);
            }

            localEntities.Add(entityId);
        }

        internal void RemoveEntity(EntityId entityId)
        {
            if (!entitiesAdded.Remove(entityId))
            {
                entitiesRemoved.Add(entityId);
            }

            localEntities.Remove(entityId);
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            var dispatcher = World.GetExistingManager<SpatialOSReceiveSystem>().Dispatcher;
            dispatcher.OnAddEntity(op => AddEntity(new EntityId(op.EntityId)));
            dispatcher.OnRemoveEntity(op => RemoveEntity(new EntityId(op.EntityId)));
        }

        protected override void OnUpdate()
        {
            entitiesAdded.Clear();
            entitiesRemoved.Clear();
        }
    }
}
