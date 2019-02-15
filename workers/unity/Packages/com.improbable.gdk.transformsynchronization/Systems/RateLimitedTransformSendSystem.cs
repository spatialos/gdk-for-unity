using Improbable.Gdk.Core;
using Improbable.Transform;
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
        private ComponentGroup transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();
            tickRate = World.GetExistingManager<TickRateEstimationSystem>();

            transformGroup = GetComponentGroup(
                ComponentType.Create<LastTransformSentData>(),
                ComponentType.Create<TransformInternal.Component>(),
                ComponentType.Create<TicksSinceLastTransformUpdate>(),
                ComponentType.ReadOnly<TransformToSend>(),
                ComponentType.ReadOnly<RateLimitedSendConfig>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>());
        }

        protected override void OnUpdate()
        {
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.Authoritative);

            var rateLimitedConfigArray = transformGroup.GetSharedComponentDataArray<RateLimitedSendConfig>();
            var transformArray = transformGroup.GetComponentDataArray<TransformInternal.Component>();
            var transformToSendArray = transformGroup.GetComponentDataArray<TransformToSend>();
            var lastTransformSentArray = transformGroup.GetComponentDataArray<LastTransformSentData>();
            var ticksSinceLastUpdateArray = transformGroup.GetComponentDataArray<TicksSinceLastTransformUpdate>();

            for (int i = 0; i < transformArray.Length; ++i)
            {
                var lastTransformSent = lastTransformSentArray[i];
                lastTransformSent.TimeSinceLastUpdate += Time.deltaTime;
                lastTransformSentArray[i] = lastTransformSent;

                if (lastTransformSent.TimeSinceLastUpdate <
                    1.0f / rateLimitedConfigArray[i].MaxTransformUpdateRateHz)
                {
                    continue;
                }

                var transform = transformArray[i];

                var transformToSend = transformToSendArray[i];

                var currentTransform = new TransformInternal.Component
                {
                    Location = (transformToSend.Position - worker.Origin).ToImprobableLocation(),
                    Rotation = transformToSend.Orientation.ToImprobableQuaternion(),
                    Velocity = transformToSend.Velocity.ToImprobableVelocity(),
                    PhysicsTick = transform.PhysicsTick + ticksSinceLastUpdateArray[i].NumberOfTicks,
                    TicksPerSecond = tickRate.PhysicsTicksPerRealSecond
                };

                if (!(TransformUtils.HasChanged(currentTransform.Location, transform.Location) ||
                    TransformUtils.HasChanged(currentTransform.Rotation, transform.Rotation)))
                {
                    continue;
                }

                lastTransformSent.TimeSinceLastUpdate = 0.0f;
                lastTransformSent.Transform = transform;
                lastTransformSentArray[i] = lastTransformSent;

                transformArray[i] = currentTransform;

                ticksSinceLastUpdateArray[i] = new TicksSinceLastTransformUpdate
                {
                    NumberOfTicks = 0
                };
            }
        }
    }
}
