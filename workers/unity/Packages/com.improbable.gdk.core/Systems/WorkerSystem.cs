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

        public readonly Connection Connection;
        public readonly ILogDispatcher LogDispatcher;
        public readonly string WorkerType;
        public readonly Vector3 Origin;

        internal MessagesToSend MessagesToSend;

        internal readonly View View = new View();
        internal readonly IConnectionHandler ConnectionHandler;

        internal readonly Dictionary<EntityId, Entity> EntityIdToEntity = new Dictionary<EntityId, Entity>();

        internal ViewDiff Diff;

        public WorkerSystem(IConnectionHandler connectionHandler, Connection connection, ILogDispatcher logDispatcher, string workerType, Vector3 origin)
        {
            Connection = connection;
            LogDispatcher = logDispatcher;
            WorkerType = workerType;
            Origin = origin;
            ConnectionHandler = connectionHandler;

            MessagesToSend = connectionHandler.GetMessagesToSendContainer();
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
            MessagesToSend.AddLogMessage(new LogMessageToSend(message, loggerName, logLevel, entityId?.Id));
        }

        public void SendMetrics(Metrics metrics)
        {
            MessagesToSend.AddMetrics(metrics);
        }

        internal void GetMessages()
        {
            ConnectionHandler.GetMessagesReceived(ref Diff);
        }

        internal void SendMessages()
        {
            ConnectionHandler.PushMessagesToSend(MessagesToSend);
            MessagesToSend = ConnectionHandler.GetMessagesToSendContainer();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            var entityManager = World.EntityManager;
            WorkerEntity = entityManager.CreateEntity(typeof(OnConnected), typeof(WorkerEntityTag));
            EntityIdToEntity.Add(new EntityId(0), WorkerEntity);
            Enabled = false;
        }

        protected override void OnDestroy()
        {
            ConnectionHandler.Dispose();
            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
        }
    }
}
