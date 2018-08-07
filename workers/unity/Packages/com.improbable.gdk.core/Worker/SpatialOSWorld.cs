using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class SpatialOSWorld : World
    {
        private Vector3 origin;
        private WorkerBase worker;
        private HashSet<ScriptBehaviourManager> spatialOSSystemManagers = new HashSet<ScriptBehaviourManager>();
        private EntityManager entityManager;

        public SpatialOSWorld(string name, Vector3 origin) : base(name)
        {
            entityManager = GetOrCreateManager<EntityManager>();
        }

        public void Connect(ConnectionConfig config, ILogDispatcher logDispatcher)
        {
            if (worker != null)
            {
                throw new NullReferenceException("Called connect while having a Worker already connected to SpatialOS.");
            }

            worker = WorkerRegistry.CreateWorker(config, entityManager, logDispatcher, origin);
            WorkerRegistry.SetWorkerForWorld(worker, this);
            foreach (var type in worker.RequiredSpatialSystems)
            {
                var manager = GetOrCreateManager(type);
                spatialOSSystemManagers.Add(manager);
            }
        }

        public void Disconnect(string reason)
        {
            if (worker == null)
            {
                throw new NullReferenceException("Called disconnect without having a Worker connected to SpatialOS.");
            }

            foreach (var manager in spatialOSSystemManagers)
            {
                DestroyManager(manager);
            }
            spatialOSSystemManagers.Clear();
            WorkerRegistry.UnsetWorkerForWorld(worker, this);
            worker.Dispose();
            worker = null;
        }

        public new void Dispose()
        {
            if (worker != null)
            {
                Disconnect("World is getting disposed.");
            }
            base.Dispose();
        }
    }
}
