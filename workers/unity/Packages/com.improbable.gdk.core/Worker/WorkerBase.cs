using System;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public abstract class WorkerBase : IDisposable
    {
        public World World { get; private set; }

        public MutableView View { get; private set; }

        public Connection Connection { get; private set; }

        public Vector3 Origin { get; private set; }

        public string WorkerId { get; private set; }

        public abstract string GetWorkerType { get; }

        protected WorkerBase(string workerId, Vector3 origin) : this(workerId, origin, new LoggingDispatcher())
        {
        }

        protected WorkerBase(string workerId, Vector3 origin, ILogDispatcher loggingDispatcher)
        {
            if (string.IsNullOrEmpty(workerId))
            {
                throw new ArgumentException("WorkerId is null or empty.", nameof(workerId));
            }

            WorkerId = workerId;
            World = new World(WorkerId);
            WorkerRegistry.SetWorkerForWorld(this);

            View = new MutableView(World, loggingDispatcher);
            Origin = origin;
        }

        public void Dispose()
        {
            WorkerRegistry.UnsetWorkerForWorld(this);
            View.Dispose();
            World.Dispose();
        }

        public void Connect(ConnectionConfig config)
        {
            if (config is ReceptionistConfig)
            {
                Connection = ConnectionUtility.ConnectToSpatial((ReceptionistConfig) config, GetWorkerType, WorkerId);
            }
            else if (config is LocatorConfig)
            {
                Connection = ConnectionUtility.LocatorConnectToSpatial((LocatorConfig) config, GetWorkerType);
            }
            else
            {
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
