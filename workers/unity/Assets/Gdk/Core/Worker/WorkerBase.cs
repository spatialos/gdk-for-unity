using System;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public abstract class WorkerBase
    {
        public World World { get; private set; }

        public MutableView View { get; private set; }

        public Connection Connection { get; private set; }

        public Vector3 Origin { get; private set; }

        public string WorkerId { get; private set; }

        public abstract string GetWorkerType { get; }

        protected WorkerBase(string workerId, Vector3 origin)
        {
            if (string.IsNullOrEmpty(workerId))
            {
                throw new ArgumentException("WorkerId is null or empty.");
            }

            WorkerId = workerId;
            Origin = origin;
        }

        public bool Connect(ConnectionConfig config)
        {
            if (config is ReceptionistConfig)
            {
                Connection = ConnectionUtility.ConnectToSpatial((ReceptionistConfig) config, GetWorkerType, WorkerId);
            }
            else if (config is LocatorConfig)
            {
                Connection = ConnectionUtility.LocatorConnectToSpatial((LocatorConfig) config, GetWorkerType);
            }

            if (Connection == null || !Connection.IsConnected)
            {
                return false;
            }

            World = new World(WorkerId);
            WorkerRegistry.SetWorkerForWorld(this);

            View = new MutableView(World);
            View.Connection = Connection;

            RegisterSystems();

            View.Connect();
            return true;
        }

        public void Clear()
        {
            if (Connection == null || World == null)
            {
                return;
            }

            // This is required because the systems will still tick once when the world is disposed causing exceptions
            foreach (var manager in World.BehaviourManagers)
            {
                var system = manager as ComponentSystem;
                if (system != null)
                {
                    system.Enabled = false;
                }
            }

            World.Dispose();
            ConnectionUtility.Disconnect(Connection);

            View = null;
            World = null;
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
