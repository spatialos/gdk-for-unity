using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationSystemHelper
    {
        public static void AddClientSystems(World world)
        {
            world.GetOrCreateManager<InitializeEntitiesSystem>();
            world.GetOrCreateManager<TickRateEstimationSystem>();
            world.GetOrCreateManager<InterpolateTransformSystem>();
            world.GetOrCreateManager<GetLatestTransformValueSystem>();
            world.GetOrCreateManager<DefaultApplyLatestTransformSystem>();
            world.GetOrCreateManager<DefaultUpdateLatestTransformSystem>();
            world.GetOrCreateManager<UpdateTransformSystem>();
            world.GetOrCreateManager<UpdatePositionSystem>();
            world.GetOrCreateManager<ResetForAuthorityGainedSystem>();
            world.GetOrCreateManager<SetKinematicFromAuthoritySystem>();
            world.GetOrCreateManager<PositionSendSystem>();
            world.GetOrCreateManager<TransformSendSystem>();
            world.GetOrCreateManager<TickSystem>();
        }

        public static void AddServerSystems(World world)
        {
            world.GetOrCreateManager<InitializeEntitiesSystem>();
            world.GetOrCreateManager<TickRateEstimationSystem>();
            world.GetOrCreateManager<TakeRawTransformUpdateSystem>();
            world.GetOrCreateManager<GetLatestTransformValueSystem>();
            world.GetOrCreateManager<DefaultApplyLatestTransformSystem>();
            world.GetOrCreateManager<DefaultUpdateLatestTransformSystem>();
            world.GetOrCreateManager<UpdateTransformSystem>();
            world.GetOrCreateManager<UpdatePositionSystem>();
            world.GetOrCreateManager<SetKinematicFromAuthoritySystem>();
            world.GetOrCreateManager<ResetForAuthorityGainedSystem>();
            world.GetOrCreateManager<PositionSendSystem>();
            world.GetOrCreateManager<TransformSendSystem>();
            world.GetOrCreateManager<TickSystem>();
        }
    }
}
