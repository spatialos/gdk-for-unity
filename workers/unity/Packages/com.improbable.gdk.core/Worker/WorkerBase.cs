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
        public const long WorkerEntityId = -1337;

        public readonly EntityManager EntityManager;
        public readonly Connection Connection;
        public readonly ILogDispatcher LogDispatcher;

        public readonly List<Type> RequiredSpatialSystems = new List<Type>
        {
            typeof(SpatialOSReceiveSystem),
            typeof(SpatialOSSendSystem),
            typeof(CleanReactiveComponentsSystem)
        };

        internal readonly Dictionary<long, Entity> EntityMapping = new Dictionary<long, Entity>();

        protected WorkerBase(ConnectionConfig config, EntityManager entityManager, ILogDispatcher logDispatcher, Vector3 origin)
        {
            FindTranslationUnits();

            EntityManager = entityManager;
            LogDispatcher = logDispatcher;
            Connection = Connect(config);
            if (logDispatcher is ForwardingDispatcher dispatcher)
            {
                dispatcher.SetConnection(Connection);
            }
            var workerEntity = entityManager.CreateEntity(
                typeof(WorkerEntityTag),
                typeof(IsConnected),
                typeof(OnConnected)
                );
            EntityMapping.Add(WorkerEntityId, workerEntity);
            AddAllCommandRequestSenders(workerEntity, WorkerEntityId);
        }

        public bool TryGetEntity(long entityId, out Entity entity)
        {
            return EntityMapping.TryGetValue(entityId, out entity);
        }

        public void Dispose()
        {
            foreach (var translation in TranslationUnits.Values)
            {
                translation.Dispose();
            }
            foreach (var entity in EntityMapping.Values)
            {
                EntityManager.DestroyEntity(entity);
            }
            EntityMapping.Clear();
            ConnectionUtility.Disconnect(Connection);
            Connection.Dispose();
        }

        private static Connection Connect(ConnectionConfig config)
        {
            switch (config)
            {
                case ReceptionistConfig receptionistConfig:
                    if (string.IsNullOrEmpty(config.WorkerId))
                    {
                        config.WorkerId = $"{config.WorkerType}-{Guid.NewGuid()}";
                    }
                    return ConnectionUtility.ConnectToSpatial(receptionistConfig);
                case LocatorConfig locatorConfig:
                    return ConnectionUtility.LocatorConnectToSpatial(locatorConfig);
            }

            throw new InvalidConfigurationException($"Invalid connection config was provided: '{config}' Only" +
                "ReceptionistConfig and LocatorConfig are supported.");
        }

        // section of stuff that will be removed soon.
        // leaving it here until Jamie is done with his refactoring :P

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
