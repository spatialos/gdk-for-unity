using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateAfter(typeof(DefaultUpdateLatestTransformSystem))]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class RateLimitedTransformSendSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public ComponentDataArray<TransformInternal.Component> TransformComponent;
            public ComponentDataArray<TicksSinceLastTransformUpdate> TicksSinceLastUpdate;
            public ComponentDataArray<LastTransformSentData> LastTransformSent;
            [ReadOnly] public ComponentDataArray<TransformToSend> TransformToSend;
            [ReadOnly] public SharedComponentDataArray<RateLimitedSendConfig> RateLimitedConfig;

            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>> DenotesAuthoritative;
        }

        [Inject] private Data data;
        [Inject] private WorkerSystem worker;
        [Inject] private TickRateEstimationSystem tickRate;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                var lastTransformSent = data.LastTransformSent[i];
                lastTransformSent.TimeSinceLastUpdate += Time.deltaTime;
                data.LastTransformSent[i] = lastTransformSent;

                if (lastTransformSent.TimeSinceLastUpdate <
                    1.0f / data.RateLimitedConfig[i].MaxTransformUpdateRateHz)
                {
                    continue;
                }

                var transform = data.TransformComponent[i];

                var transformToSend = data.TransformToSend[i];

                var currentTransform = new TransformInternal.Component();
                currentTransform.Location = (transformToSend.Position - worker.Origin).ToImprobableLocation();
                currentTransform.Rotation = transformToSend.Orientation.ToImprobableQuaternion();
                currentTransform.Velocity = transformToSend.Velocity.ToImprobableVelocity();
                currentTransform.PhysicsTick = transform.PhysicsTick + data.TicksSinceLastUpdate[i].NumberOfTicks;
                currentTransform.TicksPerSecond = tickRate.PhysicsTicksPerRealSecond;

                var locationHasChanged = TransformUtils.HasChanged(currentTransform.Location, transform.Location);
                var rotationHasChanged = TransformUtils.HasChanged(currentTransform.Rotation, transform.Rotation);

                if (!locationHasChanged && !rotationHasChanged)
                {
                    continue;
                }

                lastTransformSent.TimeSinceLastUpdate = 0.0f;
                lastTransformSent.Transform = transform;
                data.LastTransformSent[i] = lastTransformSent;

                data.TransformComponent[i] = currentTransform;

                data.TicksSinceLastUpdate[i] = new TicksSinceLastTransformUpdate
                {
                    NumberOfTicks = 0
                };
            }
        }
    }
}
