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
    public abstract class WorkerBase : IDisposable
    {
        public Connection Connection { get; private set; }

        public Vector3 Origin { get; }

        public string WorkerId { get; }

        public abstract string GetWorkerType { get; }

        

        public bool UseDynamicId { get; protected set; }

        public readonly EntityManager EntityManager;
        public const long WorkerEntityId = -1337;
        public readonly Entity WorkerEntity;

        public readonly ILogDispatcher LogDispatcher;

        public readonly Type[] Systems = new[]
        {
            typeof(SpatialOSReceiveSystem),
            typeof(SpatialOSSendSystem),
            typeof(CleanReactiveComponentsSystem)
        };

        private readonly Dictionary<long, Entity> entityMapping;

        protected WorkerBase(string workerId, ConnectionConfig config, Vector3 origin, EntityManager entityManager)
        {
            FindTranslationUnits();
            // TODO addAllCommandRequestSenders(WorkerEntity, WorkerEntityId);
            entityMapping = new Dictionary<long, Entity>();

            LogDispatcher = new ForwardingDispatcher();
            this.EntityManager = entityManager;
            WorkerId = workerId;
            if (config is ReceptionistConfig)
            {
                if (UseDynamicId)
                {
                    WorkerId = $"{GetWorkerType}-{Guid.NewGuid()}";
                }

                Connection = ConnectionUtility.ConnectToSpatial((ReceptionistConfig)config, GetWorkerType,
                    WorkerId);
            }
            else if (config is LocatorConfig)
            {
                Connection = ConnectionUtility.LocatorConnectToSpatial((LocatorConfig)config, GetWorkerType);
            }
            else
            {
                throw new InvalidConfigurationException($"Invalid connection config was provided: '{config}' Only" +
                    "ReceptionistConfig and LocatorConfig are supported.");
            }
            WorkerEntity = entityManager.CreateEntity(typeof(WorkerEntityTag));

            Application.quitting += () =>
            {
                ConnectionUtility.Disconnect(Connection);
                Connection = null;
            };

            entityManager.AddComponent(WorkerEntity, typeof(IsConnected));
            entityManager.AddComponent(WorkerEntity, typeof(OnConnected));

            Origin = origin;
        }

        public void Dispose()
        {
            ConnectionUtility.Disconnect(Connection);
            EntityManager.DestroyEntity(WorkerEntity);
            // todo destroy all spatial entities
            foreach (var translation in TranslationUnits.Values)
            {
                translation.Dispose();
            }
        }

        internal void CreateEntity(long entityId)
        {
            if (entityMapping.ContainsKey(entityId))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent("Tried to add an entity but there is already an entity associated with that EntityId.")
                    .WithField(LoggingUtils.LoggerName, GetWorkerType)
                    .WithField(LoggingUtils.EntityId, entityId));
                return;
            }

            var entity = EntityManager.CreateEntity();
            EntityManager.AddComponentData(entity, new SpatialEntityId
            {
                EntityId = entityId
            });
            EntityManager.AddComponentData(entity, new NewlyAddedSpatialOSEntity());

            // TODO addAllCommandRequestSenders(entity, entityId);
            entityMapping.Add(entityId, entity);
        }

        public bool TryGetEntity(long entityId, out Entity entity)
        {
            return entityMapping.TryGetValue(entityId, out entity);
        }

        internal void RemoveEntity(long entityId)
        {
            Entity entity;
            if (!TryGetEntity(entityId, out entity))
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent("Tried to delete an entity but there is no entity associated with that EntityId.")
                    .WithField(LoggingUtils.LoggerName, GetWorkerType)
                    .WithField(LoggingUtils.EntityId, entityId));
                return;
            }

            EntityManager.DestroyEntity(entityMapping[entityId]);
            entityMapping.Remove(entityId);
        }



        // section of stuff that will hopefully be removed soonish

        public readonly Dictionary<int, ComponentTranslation> TranslationUnits =
            new Dictionary<int, ComponentTranslation>();

        private Action<Entity, long> addAllCommandRequestSenders;

        private void FindTranslationUnits()
        {
            var translationTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentTranslation).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var translationType in translationTypes)
            {
                var translator = (ComponentTranslation)Activator.CreateInstance(translationType, this);
                TranslationUnits.Add(translator.TargetComponentType.TypeIndex, translator);

                //TODO addAllCommandRequestSenders += translator.AddCommandRequestSender;
            }
        }
    }
}
