using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Components;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    public class Worker : IDisposable
    {
        public const long WorkerEntityId = -1337;

        internal readonly Dictionary<long, Entity> EntityMapping = new Dictionary<long, Entity>();

        public readonly Vector3 Origin;
        public readonly string WorkerType;
        public readonly string WorkerId;
        public readonly World World;
        public readonly Connection Connection;
        public readonly ILogDispatcher LogDispatcher;

        // callbacks for monobehaviour
        public static Action<Worker> OnConnect;
        public static Action<Worker> OnDisconnect;

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
            var entityManager = World.GetOrCreateManager<EntityManager>();

            var workerConfig = new WorkerConfig
            {
                Worker = this
            };

            var entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new OnConnected());
            entityManager.AddSharedComponentData(entity, workerConfig);
            EntityMapping.Add(WorkerEntityId, entity);

            var translationUnityRegistry = new TranslationUnityRegistry(this);
            translationUnityRegistry.AddAllCommandRequestSenders(entity, WorkerEntityId);
        }

        public bool TryGetEntity(long entityId, out Entity entity)
        {
            return EntityMapping.TryGetValue(entityId, out entity);
        }

        public static Worker Connect(ConnectionConfig config, ILogDispatcher logDispatcher, Vector3 origin)
        {
            var worker = new Worker(config, logDispatcher, origin);
            OnConnect(worker);
            return worker;
        }

        public static void Disconnect(Worker worker)
        {
            OnDisconnect(worker);
            worker.Dispose();
        }

        public void Dispose()
        {
            EntityMapping.Clear();
            World.Dispose();
            ConnectionUtility.Disconnect(Connection);
            Connection.Dispose();
        }
    }
}
