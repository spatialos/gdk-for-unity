using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationSystemHelper
    {
        public static void AddSystems(World world)
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
