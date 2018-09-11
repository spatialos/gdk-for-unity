using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion

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
            public ComponentDataArray<TicksSinceLastTransformUpdate> TicksSinceLastUpdate;
            public ComponentDataArray<TransformInternal.Component> Transform;

            [ReadOnly] public ComponentDataArray<Authoritative<TransformInternal.Component>> DenotesHasAuthority;
        }

        [Inject] private Data data;
        [Inject] private WorkerSystem worker;
        [Inject] private TickRateEstimationSystem tickRate;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                var t = data.CurrentTransform[i];
                var transform = new TransformInternal.Component
                {
                    Location = (t.Position - worker.Origin).ToImprobableLocation(),
                    Rotation = t.Orientation.ToImprobableQuaternion(),
                    Velocity = t.Velocity.ToImprobableVelocity(),
                    PhysicsTick = data.Transform[i].PhysicsTick + data.TicksSinceLastUpdate[i].NumberOfTicks,
                    TicksPerSecond = tickRate.PhysicsTicksPerRealSecond
                };

                data.Transform[i] = transform;

                data.TicksSinceLastUpdate[i] = new TicksSinceLastTransformUpdate
                {
                    NumberOfTicks = 0
                };
            }
        }
    }
}
