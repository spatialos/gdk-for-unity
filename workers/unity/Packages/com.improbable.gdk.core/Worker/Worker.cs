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
        public delegate void WorkerEventHandler(Worker worker);

        public static event WorkerEventHandler OnConnect;
        public static event WorkerEventHandler OnDisconnect;

        public readonly Vector3 Origin;
        public readonly string WorkerType;
        public readonly string WorkerId;
        public readonly Entity WorkerEntity;
        public readonly ILogDispatcher LogDispatcher;

        public Connection Connection { get; private set; }
        public World World { get; private set; }


        internal readonly Dictionary<EntityId, Entity> EntityMapping = new Dictionary<EntityId, Entity>();

        private Worker(ConnectionConfig config, ILogDispatcher logDispatcher, Vector3 origin)
        {
            Origin = origin;
            WorkerId = string.IsNullOrEmpty(config.WorkerId)
                ? $"{config.WorkerType}-{Guid.NewGuid()}"
                : config.WorkerId;

            Connection = ConnectionUtility.Connect(config, WorkerId);
            LogDispatcher = logDispatcher;
            WorkerType = config.WorkerType;
            logDispatcher.Connection = Connection;

            World = new World(WorkerId);
            World.CreateManager<WorkerSystem>(this);
            var entityManager = World.GetOrCreateManager<EntityManager>();
            WorkerEntity = entityManager.CreateEntity(typeof(OnConnected), typeof(WorkerEntityTag));
            OnConnect?.Invoke(this);
        }

        public static Worker GetWorkerFromWorld(World world)
        {
            var workerSystem = world.GetExistingManager<WorkerSystem>();
            if (workerSystem == null)
            {
                throw new NullReferenceException("This world does not have a worker associated with it.");
            }

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
            World?.Dispose();
            World = null;
            OnDisconnect?.Invoke(this);
            ConnectionUtility.Disconnect(Connection);
            Connection?.Dispose();
            Connection = null;
        }
    }
}
