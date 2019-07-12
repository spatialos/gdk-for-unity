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

                    var transformHasChanged = false;

                    var newVelocity = TransformUtils.ToFixedPointVector3(transformToSend.Velocity);
                    if (TransformUtils.HasChanged(newVelocity, transform.Velocity))
                    {
                        transform.Velocity = newVelocity;
                        transformHasChanged = true;
                    }

                    var newLocation = TransformUtils.ToFixedPointVector3(transformToSend.Position - worker.Origin);
                    if (TransformUtils.HasChanged(newLocation, transform.Location))
                    {
                        transform.Location = newLocation;
                        transformHasChanged = true;
                    }

                    var newRotation = TransformUtils.ToCompressedQuaternion(transformToSend.Orientation);
                    if (TransformUtils.HasChanged(newRotation, transform.Rotation))
                    {
                        transform.Rotation = newRotation;
                        transformHasChanged = true;
                    }

                    if (!transformHasChanged)
                    {
                        return;
                    }

                    transform.PhysicsTick += ticksSinceLastTransformUpdate.NumberOfTicks;
                    transform.TicksPerSecond = tickRate.PhysicsTicksPerRealSecond;

                    lastTransformSent.TimeSinceLastUpdate = 0.0f;
                    lastTransformSent.Transform = transform;

                    ticksSinceLastTransformUpdate = new TicksSinceLastTransformUpdate
                    {
                        NumberOfTicks = 0
                    };
                });
        }
    }
}
