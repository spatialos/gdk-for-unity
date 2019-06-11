using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateAfter(typeof(DefaultUpdateLatestTransformSystem))]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class RateLimitedTransformSendSystem : ComponentSystem
    {
        private WorkerSystem worker;
        private TickRateEstimationSystem tickRate;
        private EntityQuery transformGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();
            tickRate = World.GetExistingSystem<TickRateEstimationSystem>();

            transformGroup = GetEntityQuery(
                ComponentType.ReadWrite<LastTransformSentData>(),
                ComponentType.ReadWrite<TransformInternal.Component>(),
                ComponentType.ReadWrite<TicksSinceLastTransformUpdate>(),
                ComponentType.ReadOnly<TransformToSend>(),
                ComponentType.ReadOnly<RateLimitedSendConfig>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>());
        }

        protected override void OnUpdate()
        {
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

            Entities.With(transformGroup).ForEach(
                (RateLimitedSendConfig config, ref TransformInternal.Component transform,
                    ref TransformToSend transformToSend, ref LastTransformSentData lastTransformSent,
                    ref TicksSinceLastTransformUpdate ticksSinceLastTransformUpdate) =>
                {
                    lastTransformSent.TimeSinceLastUpdate += Time.deltaTime;

                    if (lastTransformSent.TimeSinceLastUpdate < 1.0f / config.MaxTransformUpdateRateHz)
                    {
                        return;
                    }

                    var currentTransform = new TransformInternal.Component
                    {
                        Location = (transformToSend.Position - worker.Origin).ToImprobableLocation(),
                        Rotation = transformToSend.Orientation.ToImprobableQuaternion(),
                        Velocity = transformToSend.Velocity.ToImprobableVelocity(),
                        PhysicsTick = transform.PhysicsTick + ticksSinceLastTransformUpdate.NumberOfTicks,
                        TicksPerSecond = tickRate.PhysicsTicksPerRealSecond
                    };

                    if (!(TransformUtils.HasChanged(currentTransform.Location, transform.Location) ||
                        TransformUtils.HasChanged(currentTransform.Rotation, transform.Rotation)))
                    {
                        return;
                    }

                    lastTransformSent.TimeSinceLastUpdate = 0.0f;
                    lastTransformSent.Transform = transform;

                    transform = currentTransform;

                    ticksSinceLastTransformUpdate = new TicksSinceLastTransformUpdate
                    {
                        NumberOfTicks = 0
                    };
                });
        }
    }
}
