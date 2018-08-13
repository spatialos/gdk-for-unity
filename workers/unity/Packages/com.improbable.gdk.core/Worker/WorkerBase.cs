using System;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public abstract class WorkerBase : IDisposable
    {
        public World World { get; }

        public MutableView View { get; }

        public Connection Connection { get; private set; }

        public Vector3 Origin { get; }

        public string WorkerId { get; private set; }

        public abstract string GetWorkerType { get; }

        public EntityGameObjectLinker EntityGameObjectLinker { get; }

        public bool UseDynamicId { get; protected set; }

        protected WorkerBase(string workerId, Vector3 origin) : this(workerId, origin, new LoggingDispatcher())
        {
        }

        protected WorkerBase(string workerId, Vector3 origin, ILogDispatcher loggingDispatcher)
        {
            if (string.IsNullOrEmpty(workerId))
            {
                WorkerId = GenerateDynamicWorkerId();

                UseDynamicId = true;
            }
            else
            {
                WorkerId = workerId;
            }

            World = new World(WorkerId);
            WorkerRegistry.SetWorkerForWorld(this);

            View = new MutableView(World, loggingDispatcher);
            Origin = origin;

            EntityGameObjectLinker = new EntityGameObjectLinker(World, View);
        }

        public void Dispose()
        {
            WorkerRegistry.UnsetWorkerForWorld(this);
            View.Dispose();
            World.Dispose();
        }

        public virtual void Connect(ConnectionConfig config)
        {
            switch (config)
            {
                case ReceptionistConfig receptionistConfig:
                    if (UseDynamicId)
                    {
                        WorkerId = GenerateDynamicWorkerId();
                    }

                    Connection = ConnectionUtility.ConnectToSpatial(receptionistConfig, GetWorkerType,
                        WorkerId);
                    break;
                case LocatorConfig locatorConfig:
                    Connection = ConnectionUtility.LocatorConnectToSpatial(locatorConfig, GetWorkerType);
                    break;
                default:
                    throw new InvalidConfigurationException($"Invalid connection config was provided: '{config}' Only" +
                        "ReceptionistConfig and LocatorConfig are supported.");
            }

            Application.quitting += () =>
            {
                ConnectionUtility.Disconnect(Connection);
                Connection = null;
            };

            View.Connect();
        }

        private string GenerateDynamicWorkerId()
        {
            return $"{GetWorkerType}-{Guid.NewGuid()}";
        }

        public virtual void RegisterSystems()
        {
            RegisterCoreSystems();
        }

        protected void RegisterCoreSystems()
        {
            World.GetOrCreateManager<EntityManager>();
            World.GetOrCreateManager<SpatialOSReceiveSystem>();
            World.GetOrCreateManager<SpatialOSSendSystem>();
            World.GetOrCreateManager<CleanReactiveComponentsSystem>();
        }
    }
}
