using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateAfter(typeof(DefaultUpdateLatestTransformSystem))]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    public class UpdateTransformSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public ComponentDataArray<TransformToSend> CurrentTransform;
            public ComponentDataArray<TicksSinceLastUpdate> TicksSinceLastUpdate;
            public ComponentDataArray<Transform.Component> Transform;

            [ReadOnly] public ComponentDataArray<Authoritative<Transform.Component>> DenotesHasAuthority;
        }

        [Inject] private Data data;
        [Inject] private WorkerSystem worker;
        [Inject] private TickRateEstimationSystem tickRate;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                var t = data.CurrentTransform[i];
                var transform = new Transform.Component
                {
                    Location = (t.Position - worker.Origin).ToImprobableLocation(),
                    Rotation = t.Orientation.ToImprobableQuaternion(),
                    Velocity = t.Velocity.ToImprobableVelocity(),
                    PhysicsTick = data.Transform[i].PhysicsTick + data.TicksSinceLastUpdate[i].NumberOfTicks,
                    TicksPerSecond = tickRate.PhysicsTicksPerRealSecond
                };

                data.Transform[i] = transform;

                data.TicksSinceLastUpdate[i] = new TicksSinceLastUpdate
                {
                    NumberOfTicks = 0
                };
            }
        }
    }
}
