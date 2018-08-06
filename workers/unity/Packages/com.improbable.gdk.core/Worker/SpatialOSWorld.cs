using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class SpatialOSWorld : World
    {
        private WorkerBase worker;
        private List<ScriptBehaviourManager> spatialOSSystemManagers;
        public EntityGameObjectLinker EntityGameObjectLinker { get; }

        public SpatialOSWorld(string name) : base(name)
        {
            EntityGameObjectLinker = new EntityGameObjectLinker(this);
        }

        public void Connect(ConnectionConfig config, Vector3 origin)
        {
            // Create worker, connect to fabric and register worker-specific systems
            worker = WorkerRegistry.CreateWorker(config, origin);
            foreach (var type in worker.RequiredSpatialSystems)
            {
                var manager = GetOrCreateManager(type);
                spatialOSSystemManagers.Add(manager);
            }
            WorkerRegistry.SetWorkerForWorld(worker, this);
        }

        public void Disconnect(string reason)
        {
            foreach (var manager in spatialOSSystemManagers)
            {
                DestroyManager(manager);
            }
            WorkerRegistry.UnsetWorkerForWorld(worker, this);
            Dispose();
        }

        public new void Dispose()
        {
            base.Dispose();
            worker.Dispose();
            worker = null;
        }
    }
}
