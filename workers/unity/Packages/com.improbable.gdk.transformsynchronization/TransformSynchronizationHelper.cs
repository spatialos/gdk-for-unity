using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationHelper
    {
        public static void AddComponents(ref EntityBuilder entityBuilder, string writeAccess,
            Location location = default(Location),
            Velocity velocity = default(Velocity),
            uint physicsTick = 0,
            float ticksPerSecond = 0)
        {
            AddComponents(ref entityBuilder, writeAccess,
                new Quaternion(1f, 0f, 0f, 0f),
                location,
                velocity,
                physicsTick,
                ticksPerSecond);
        }

        public static void AddComponents(ref EntityBuilder entityBuilder, string writeAccess,
            Quaternion quaternion,
            Location location = default(Location),
            Velocity velocity = default(Velocity),
            uint physicsTick = 0,
            float ticksPerSecond = 0)
        {
            var transform = TransformInternal.Component.CreateSchemaComponentData(
                location,
                quaternion,
                velocity,
                physicsTick,
                ticksPerSecond
            );
            entityBuilder = entityBuilder.AddComponent(transform, writeAccess);
        }

        public static void AddClientSystems(World world)
        {
            world.GetOrCreateManager<DefaultTransformInitializationSystem>();
            world.GetOrCreateManager<TickRateEstimationSystem>();
            world.GetOrCreateManager<InterpolateTransformSystem>();
            world.GetOrCreateManager<GetTransformValueToSetSystem>();
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
            world.GetOrCreateManager<DefaultTransformInitializationSystem>();
            world.GetOrCreateManager<TickRateEstimationSystem>();
            world.GetOrCreateManager<TakeRawTransformUpdateSystem>();
            world.GetOrCreateManager<GetTransformValueToSetSystem>();
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
