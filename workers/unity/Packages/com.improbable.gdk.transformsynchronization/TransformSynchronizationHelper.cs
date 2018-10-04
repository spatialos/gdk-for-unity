using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Entities;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationHelper
    {
        public static EntityBuilder AddTransformSynchronizationComponents(this EntityBuilder entityBuilder,
            string writeAccess,
            Vector3 location = default(Vector3),
            Vector3 velocity = default(Vector3))
        {
            return entityBuilder.AddTransformSynchronizationComponents(writeAccess,
                Quaternion.identity,
                location,
                velocity);
        }

        public static EntityBuilder AddTransformSynchronizationComponents(this EntityBuilder entityBuilder,
            string writeAccess,
            Quaternion rotation,
            Vector3 location = default(Vector3),
            Vector3 velocity = default(Vector3))
        {
            var transform = TransformInternal.Component.CreateSchemaComponentData(
                location.ToImprobableLocation(),
                rotation.ToImprobableQuaternion(),
                velocity.ToImprobableVelocity(),
                0,
                1f / Time.fixedDeltaTime
            );
            return entityBuilder.AddComponent(transform, writeAccess);
        }

        public static void AddClientSystems(World world)
        {
            world.GetOrCreateManager<TickRateEstimationSystem>();
            world.GetOrCreateManager<DirectTransformUpdateSystem>();
            world.GetOrCreateManager<InterpolateTransformSystem>();
            world.GetOrCreateManager<GetTransformValueToSetSystem>();
            world.GetOrCreateManager<DefaultApplyLatestTransformSystem>();
            world.GetOrCreateManager<DefaultUpdateLatestTransformSystem>();
            world.GetOrCreateManager<RateLimitedPositionSendSystem>();
            world.GetOrCreateManager<RateLimitedTransformSendSystem>();
            world.GetOrCreateManager<ResetForAuthorityGainedSystem>();
            world.GetOrCreateManager<SetKinematicFromAuthoritySystem>();
            world.GetOrCreateManager<TickSystem>();
        }

        public static void AddServerSystems(World world)
        {
            world.GetOrCreateManager<TickRateEstimationSystem>();
            world.GetOrCreateManager<DirectTransformUpdateSystem>();
            world.GetOrCreateManager<InterpolateTransformSystem>();
            world.GetOrCreateManager<GetTransformValueToSetSystem>();
            world.GetOrCreateManager<DefaultApplyLatestTransformSystem>();
            world.GetOrCreateManager<DefaultUpdateLatestTransformSystem>();
            world.GetOrCreateManager<RateLimitedPositionSendSystem>();
            world.GetOrCreateManager<RateLimitedTransformSendSystem>();
            world.GetOrCreateManager<ResetForAuthorityGainedSystem>();
            world.GetOrCreateManager<SetKinematicFromAuthoritySystem>();
            world.GetOrCreateManager<TickSystem>();
        }
    }
}
