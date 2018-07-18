using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransfromSynchronizationSystemHelper
    {
        public static void RegisterClientSystems(World world)
        {
            world.GetOrCreateManager<TickSystem>();
            world.GetOrCreateManager<LocalTransformSyncSystem>();
            world.GetOrCreateManager<InterpolateTransformSystem>();
            world.GetOrCreateManager<ApplyTransformUpdatesSystem>();
            world.GetOrCreateManager<TransformSendSystem>();
            world.GetOrCreateManager<PositionSendSystem>();
        }

        public static void RegisterServerSystems(World world)
        {
            world.GetOrCreateManager<TickSystem>();
            world.GetOrCreateManager<LocalTransformSyncSystem>();
            world.GetOrCreateManager<InterpolateTransformSystem>();
            world.GetOrCreateManager<ApplyTransformUpdatesSystem>();
            world.GetOrCreateManager<TransformSendSystem>();
            world.GetOrCreateManager<PositionSendSystem>();
        }
    }
}
