using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.NetworkStats;
using Improbable.Worker.CInterop;
using Unity.Entities;
using Unity.Profiling;
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
        public readonly EntityId WorkerEntityId;

        /// <summary>
        ///     Denotes whether the underlying worker is connected or not.
        /// </summary>
        public bool IsConnected => Worker.IsConnected;

        public IReadOnlyDictionary<string, string> WorkerFlags => Worker.WorkerFlags;

        internal readonly Dictionary<EntityId, Entity> EntityIdToEntity = new Dictionary<EntityId, Entity>();

        internal readonly WorkerInWorld Worker;

        internal ViewDiff Diff => Worker.ViewDiff;
        internal MessagesToSend MessagesToSend => Worker.MessagesToSend;

        private ProfilerMarker tickMarker = new ProfilerMarker("WorkerSystem.Tick");
        private ProfilerMarker sendMessagesMarker = new ProfilerMarker("WorkerSystem.SendMessages");

        public WorkerSystem(WorkerInWorld worker)
        {
            Worker = worker;

            LogDispatcher = worker.LogDispatcher;
            WorkerType = worker.WorkerType;
            WorkerId = worker.WorkerId;
            Origin = worker.Origin;
            WorkerEntityId = worker.WorkerEntityId;
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

        public Entity GetEntity(EntityId entityId)
        {
            if (!EntityIdToEntity.TryGetValue(entityId, out var entity))
            {
                throw new ArgumentException($"Unknown EntityId {entityId}", nameof(entityId));
            }

            return entity;
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

        public void SendAddComponentRequest(EntityId entityId, ISpatialComponentSnapshot snapshot, UpdateParameters? parameters = null)
        {
            Worker.MessagesToSend.DynamicComponentRequest(new AddComponentRequestToSend(entityId, snapshot.Serialize(), parameters));
        }

        public void SendRemoveComponentRequest<T>(EntityId entityId, UpdateParameters? parameters = null) where T : ISpatialComponentData
        {
            var componentId = ComponentDatabase.ComponentType<T>.ComponentId;
            SendRemoveComponentRequest(entityId, componentId, parameters);
        }

        public void SendRemoveComponentRequest(EntityId entityId, uint componentId, UpdateParameters? parameters = null)
        {
            Worker.MessagesToSend.DynamicComponentRequest(new RemoveComponentRequestToSend(entityId, componentId, parameters));
        }

        public void SendLogMessage(string message, string loggerName, LogLevel logLevel, EntityId? entityId)
        {
            Worker.MessagesToSend.AddLogMessage(new LogMessageToSend(message, loggerName, logLevel, entityId?.Id));
        }

        /// <summary>
        ///     Gets the value for a given worker flag.
        /// </summary>
        /// <param name="key">The key of the worker flag.</param>
        /// <returns>The value of the flag, if it exists, null otherwise.</returns>
        public string GetWorkerFlag(string key)
        {
            return Worker.GetWorkerFlag(key);
        }

        public void SendMetrics(Metrics metrics)
        {
            Worker.MessagesToSend.AddMetrics(metrics);
        }

        internal void Tick()
        {
            using (tickMarker.Auto())
            {
                Worker.Tick();
            }
        }

        internal void SendMessages(NetFrameStats frameStats)
        {
            using (sendMessagesMarker.Auto())
            {
                Worker.EnsureMessagesFlushed(frameStats);
            }
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
