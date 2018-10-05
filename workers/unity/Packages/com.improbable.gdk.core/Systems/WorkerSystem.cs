using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A SpatialOS worker instance.
    /// </summary>
    [DisableAutoCreation]
    public class WorkerSystem : ComponentSystem
    {
        public readonly Connection Connection;
        public readonly ILogDispatcher LogDispatcher;
        public readonly string WorkerType;
        public readonly Vector3 Origin;

        /// <summary>
        ///     An ECS entity that represents the Worker.
        /// </summary>
        public Entity WorkerEntity;

        internal readonly Dictionary<EntityId, Entity> EntityIdToEntity = new Dictionary<EntityId, Entity>();

        public WorkerSystem(Connection connection, ILogDispatcher logDispatcher, string workerType, Vector3 origin)
        {
            Connection = connection;
            LogDispatcher = logDispatcher;
            WorkerType = workerType;
            Origin = origin;
        }

        /// <summary>
        ///     Attempts to find an ECS entity associated with a SpatialOS entity ID.
        /// </summary>
        /// <param name="entityId">The SpatialOS entity ID.</param>
        /// <param name="entity">
        ///     When this method returns, contains the ECS entity associated with the SpatialOS entity ID if one was
        ///     found, else the default value for <see cref="Entity"/>.
        /// </param>
        /// <returns>
        ///     True, if an ECS entity associated with the SpatialOS entity ID was found, false otherwise.
        /// </returns>
        public bool TryGetEntity(EntityId entityId, out Entity entity)
        {
            return EntityIdToEntity.TryGetValue(entityId, out entity);
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            var entityManager = World.GetOrCreateManager<EntityManager>();
            WorkerEntity = entityManager.CreateEntity(typeof(OnConnected), typeof(WorkerEntityTag));
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}
