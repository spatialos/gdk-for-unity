using System;
using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    public class Worker : IDisposable
    {
        public const long WorkerEntityId = -1337;

        internal readonly Dictionary<EntityId, Entity> EntityMapping = new Dictionary<EntityId, Entity>();

        public readonly Vector3 Origin;
        public readonly string WorkerType;
        public readonly string WorkerId;
        public readonly World World;
        public readonly Connection Connection;
        public readonly ILogDispatcher LogDispatcher;

        public delegate void WorkerEventHandler(Worker worker);
        public static event WorkerEventHandler OnConnect;
        public static event WorkerEventHandler OnDisconnect;

        private Worker(ConnectionConfig config, ILogDispatcher logDispatcher, Vector3 origin)
        {
            Origin = origin;
            WorkerId = config.WorkerId;
            if (string.IsNullOrEmpty(WorkerId))
            {
                WorkerId = $"{config.WorkerType}-{Guid.NewGuid()}";
            }
            
            Connection = ConnectionUtility.Connect(config, WorkerId);
            LogDispatcher = logDispatcher;
            WorkerType = config.WorkerType;
            if (LogDispatcher is ForwardingDispatcher dispatcher)
            {
                dispatcher.SetConnection(Connection);
            }

            World = new World(WorkerId);
            var workerSystem = World.GetOrCreateManager<WorkerSystem>();
            workerSystem.Worker = this;

            var entityManager = World.GetOrCreateManager<EntityManager>();
            var entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new OnConnected());
            entityManager.AddComponentData(entity, new WorkerEntityTag());
            EntityMapping.Add(new EntityId(WorkerEntityId), entity);
            OnConnect?.Invoke(this);
        }

        public static Worker TryGetWorker(World world)
        {
            var workerSystem = world.GetExistingManager<WorkerSystem>();
            return workerSystem.Worker;
        }

        public bool TryGetEntity(EntityId entityId, out Entity entity)
        {
            return EntityMapping.TryGetValue(entityId, out entity);
        }

        public static Worker Connect(ConnectionConfig config, ILogDispatcher logDispatcher, Vector3 origin)
        {
            return new Worker(config, logDispatcher, origin);
        }

        public static void Disconnect(Worker worker)
        {
            worker.Dispose();
        }

        public void Dispose()
        {
            EntityMapping.Clear();
            World.Dispose();
            OnDisconnect?.Invoke(this);
            ConnectionUtility.Disconnect(Connection);
            Connection.Dispose();
        }
    }
}
