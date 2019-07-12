using System.Collections.Generic;
using Improbable.Worker.CInterop;
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
        /// <summary>
        ///     An ECS entity that represents the Worker.
        /// </summary>
        public Entity WorkerEntity;

        public readonly ILogDispatcher LogDispatcher;
        public readonly string WorkerType;
        public readonly string WorkerId;
        public readonly Vector3 Origin;

        internal readonly View View;

        internal readonly Dictionary<EntityId, Entity> EntityIdToEntity = new Dictionary<EntityId, Entity>();

        internal WorkerInWorld Worker;

        internal ViewDiff Diff => Worker.ViewDiff;
        internal MessagesToSend MessagesToSend => Worker.MessagesToSend;

        public WorkerSystem(WorkerInWorld worker)
        {
            Worker = worker;

            LogDispatcher = worker.LogDispatcher;
            WorkerType = worker.WorkerType;
            WorkerId = worker.WorkerId;
            Origin = worker.Origin;
            View = worker.View;
        }

        /// <summary>
        ///     Attempts to find an ECS entity associated with a SpatialOS entity ID.
        /// </summary>
        /// <param name="entityId">The SpatialOS entity ID.</param>
        /// <param name="entity">
        ///     When this method returns, contains the ECS entity associated with the SpatialOS entity ID if one was
        ///     found, else the default value for <see cref="Entity" />.
        /// </param>
        /// <returns>
        ///     True, if an ECS entity associated with the SpatialOS entity ID was found, false otherwise.
        /// </returns>
        public bool TryGetEntity(EntityId entityId, out Entity entity)
        {
            return EntityIdToEntity.TryGetValue(entityId, out entity);
        }

        /// <summary>
        ///     Checks whether a SpatialOS entity is checked out on this worker.
        /// </summary>
        /// <param name="entityId">The SpatialOS entity ID to check for.</param>
        /// <returns>True, if the SpatialOS entity is checked out on this worker, false otherwise.</returns>
        public bool HasEntity(EntityId entityId)
        {
            return EntityIdToEntity.ContainsKey(entityId);
        }

        public void SendLogMessage(string message, string loggerName, LogLevel logLevel, EntityId? entityId)
        {
            Worker.MessagesToSend.AddLogMessage(new LogMessageToSend(message, loggerName, logLevel, entityId?.Id));
        }

        public void SendMetrics(Metrics metrics)
        {
            Worker.MessagesToSend.AddMetrics(metrics);
        }

        internal void Tick()
        {
            Worker.Tick();
        }

        internal void SendMessages()
        {
            Worker.EnsureMessagesFlushed();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            var entityManager = World.EntityManager;
            WorkerEntity = entityManager.CreateEntity(typeof(OnConnected), typeof(WorkerEntityTag));
            EntityIdToEntity.Add(new EntityId(0), WorkerEntity);
            Enabled = false;
        }

        protected override void OnUpdate()
        {
        }
    }
}
