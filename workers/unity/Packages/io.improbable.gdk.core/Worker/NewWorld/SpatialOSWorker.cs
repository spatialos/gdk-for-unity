using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core.NewWorld
{
    public class SpatialOSWorker : Worker
    {
        public readonly Vector3 Origin;

        private Entity workerEntity;

        internal NetworkStatistics NetworkStatistics { get; } = new NetworkStatistics();

        public Entity WorkerEntity
        {
            get => workerEntity;
            internal set
            {
                EntityIdToEntity.Add(new EntityId(0), value); // this is a strange way of doing this, probably a better way e.g. store as singleton entity
                workerEntity = value;
            }
        }

        public event Action<string> OnDisconnect;

        internal readonly Dictionary<EntityId, Entity> EntityIdToEntity = new Dictionary<EntityId, Entity>();

        protected SpatialOSWorker(IConnectionHandler connectionHandler, string workerType, ILogDispatcher logDispatcher,
            Vector3 origin) : base(connectionHandler, workerType, logDispatcher)
        {
            Origin = origin;
        }

        public static async Task<SpatialOSWorker> CreateSpatialOSWorkerAsync(IConnectionHandlerBuilder connectionHandlerBuilder, string workerType,
            ILogDispatcher logDispatcher, Vector3 origin, CancellationToken? token = null)
        {
            var handler = await connectionHandlerBuilder.CreateAsync(token);
            return new SpatialOSWorker(handler, workerType, logDispatcher, origin);
        }

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
    }
}
