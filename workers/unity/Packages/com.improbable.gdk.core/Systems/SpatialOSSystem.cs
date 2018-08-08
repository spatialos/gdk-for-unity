using System;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [DisableAutoCreation]
    public abstract class SpatialOSSystem : ComponentSystem
    {
        public WorkerBase Worker { get; private set; }
        public SpatialOSWorld SpatialWorld { get; private set; }
        public Connection Connection { get; private set; }

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            SpatialWorld = World as SpatialOSWorld;
            if (SpatialWorld == null)
            {
                throw new ArgumentException("This system should only be run when connected to SpatialOS.");
            }

            Worker = WorkerRegistry.GetWorkerForWorld(World);
            Connection = Worker.Connection;
        }

        protected override void OnUpdate()
        {
            if (Connection == null)
            {
                throw new ArgumentException("This system should only be run when connected to SpatialOS.");
            }
        }
    }
}
