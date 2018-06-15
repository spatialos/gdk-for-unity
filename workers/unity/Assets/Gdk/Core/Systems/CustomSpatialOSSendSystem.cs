using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSSendGroup.CustomSpatialOSSendGroup))]
    public abstract class CustomSpatialOSSendSystem<T> : ComponentSystem where T : struct, ISpatialComponentData
    {
        private SpatialOSSendSystem spatialOSSendSystem;

        protected WorkerBase worker;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);

            worker = WorkerRegistry.GetWorkerForWorld(World);

            spatialOSSendSystem = World.GetOrCreateManager<SpatialOSSendSystem>();
            spatialOSSendSystem.RegisterCustomReplicationSystem(typeof(T));
        }
    }
}
