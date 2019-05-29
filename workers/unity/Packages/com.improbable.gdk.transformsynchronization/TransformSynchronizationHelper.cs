using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public static class TransformSynchronizationHelper
    {
        public static void AddTransformSynchronizationComponents(EntityTemplate template,
            string writeAccess,
            Vector3 location = default(Vector3),
            Vector3 velocity = default(Vector3))
        {
            AddTransformSynchronizationComponents(template, writeAccess,
                UnityEngine.Quaternion.identity,
                location,
                velocity);
        }

        public static void AddTransformSynchronizationComponents(EntityTemplate template,
            string writeAccess,
            UnityEngine.Quaternion rotation,
            Vector3 location = default(Vector3),
            Vector3 velocity = default(Vector3))
        {
            var transform = new TransformInternal.Snapshot
            {
                Location = location.ToImprobableLocation(),
                Rotation = rotation.ToImprobableQuaternion(),
                Velocity = velocity.ToImprobableVelocity(),
                TicksPerSecond = 1f / Time.fixedDeltaTime
            };

            template.AddComponent(transform, writeAccess);
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
