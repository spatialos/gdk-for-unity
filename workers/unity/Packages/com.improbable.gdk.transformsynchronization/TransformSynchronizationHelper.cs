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
            var transformSnapshot = TransformUtils.CreateTransformSnapshot(location, rotation, velocity);
            template.AddComponent(transformSnapshot, writeAccess);
        }

        public static void AddClientSystems(World world)
        {
            world.GetOrCreateSystem<TickRateEstimationSystem>();
            world.GetOrCreateSystem<DirectTransformUpdateSystem>();
            world.GetOrCreateSystem<InterpolateTransformSystem>();
            world.GetOrCreateSystem<GetTransformValueToSetSystem>();
            world.GetOrCreateSystem<DefaultApplyLatestTransformSystem>();
            world.GetOrCreateSystem<DefaultUpdateLatestTransformSystem>();
            world.GetOrCreateSystem<RateLimitedPositionSendSystem>();
            world.GetOrCreateSystem<RateLimitedTransformSendSystem>();
            world.GetOrCreateSystem<ResetForAuthorityGainedSystem>();
            world.GetOrCreateSystem<SetKinematicFromAuthoritySystem>();
            world.GetOrCreateSystem<TickSystem>();
        }

        public static void AddServerSystems(World world)
        {
            world.GetOrCreateSystem<TickRateEstimationSystem>();
            world.GetOrCreateSystem<DirectTransformUpdateSystem>();
            world.GetOrCreateSystem<InterpolateTransformSystem>();
            world.GetOrCreateSystem<GetTransformValueToSetSystem>();
            world.GetOrCreateSystem<DefaultApplyLatestTransformSystem>();
            world.GetOrCreateSystem<DefaultUpdateLatestTransformSystem>();
            world.GetOrCreateSystem<RateLimitedPositionSendSystem>();
            world.GetOrCreateSystem<RateLimitedTransformSendSystem>();
            world.GetOrCreateSystem<ResetForAuthorityGainedSystem>();
            world.GetOrCreateSystem<SetKinematicFromAuthoritySystem>();
            world.GetOrCreateSystem<TickSystem>();
        }
    }
}
