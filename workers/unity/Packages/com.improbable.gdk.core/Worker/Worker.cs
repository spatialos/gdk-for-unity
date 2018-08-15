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
            var workerSystem = World.GetOrCreateManager<WorkerSystem>();
            workerSystem.Worker = this;

            FindTranslationUnits();

            var entityManager = World.GetOrCreateManager<EntityManager>();
            var entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, new OnConnected());
            entityManager.AddComponentData(entity, new WorkerEntityTag());
            EntityMapping.Add(WorkerEntityId, entity);
            AddAllCommandRequestSenders(entity, WorkerEntityId);
            OnConnect(this);
        }

        public static Worker TryGetWorker(World world)
        {
            var workerSystem = world.GetExistingManager<WorkerSystem>();
            return workerSystem.Worker;
        }

        public bool TryGetEntity(long entityId, out Entity entity)
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
            foreach (var translation in TranslationUnits.Values)
            {
                translation.Dispose();
            }
            EntityMapping.Clear();
            World.Dispose();
            OnDisconnect(this);
            ConnectionUtility.Disconnect(Connection);
            Connection.Dispose();
        }
        
        
        // can be deleted soonish
        public readonly Dictionary<int, ComponentTranslation> TranslationUnits =
            new Dictionary<int, ComponentTranslation>();
        
        public Action<Entity, long> AddAllCommandRequestSenders;

        private void FindTranslationUnits()
        {
            var translationTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentTranslation).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var translationType in translationTypes)
            {
                var translator = (ComponentTranslation)Activator.CreateInstance(translationType, this);
                TranslationUnits.Add(translator.TargetComponentType.TypeIndex, translator);

                AddAllCommandRequestSenders += translator.AddCommandRequestSender;
            } 
        }
    }
}
