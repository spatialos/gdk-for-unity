using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    public class WorkerSystem : ComponentSystem
    {
        public Vector3 Origin;
        public Connection Connection;
        public ILogDispatcher LogDispatcher;
        public Entity WorkerEntity;
        public string WorkerType;

        internal readonly Dictionary<EntityId, Entity> EntityIdToEntity = new Dictionary<EntityId, Entity>();

        public bool TryGetEntity(EntityId entityId, out Entity entity)
        {
            return EntityIdToEntity.TryGetValue(entityId, out entity);
        }

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            var entityManager = World.GetOrCreateManager<EntityManager>();
            WorkerEntity = entityManager.CreateEntity(typeof(OnConnected), typeof(WorkerEntityTag));
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}
