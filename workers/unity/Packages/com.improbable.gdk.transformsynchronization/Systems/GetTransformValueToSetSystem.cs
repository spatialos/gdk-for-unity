using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

#region Diagnostic control

#pragma warning disable 649
// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

#endregion


namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(FixedUpdate.PhysicsFixedUpdate))]
    public class GetTransformValueToSetSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            public BufferArray<BufferedTransform> TransformBuffer;
            public ComponentDataArray<TransformToSet> CurrentTransform;
            [ReadOnly] public ComponentDataArray<NotAuthoritative<TransformInternal.Component>> DenotesNotAuthoritative;
        }

        [Inject] private Data data;
        [Inject] private WorkerSystem worker;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                var buffer = data.TransformBuffer[i];
                if (buffer.Length == 0)
                {
                    continue;
                }

                var bufferHead = buffer[0];

                var currentTransform = new TransformToSet
                {
                    Position = bufferHead.Position + worker.Origin,
                    Orientation = bufferHead.Orientation,
                    Velocity = bufferHead.Velocity,
                    ApproximateRemoteTick = bufferHead.PhysicsTick
                };

                data.CurrentTransform[i] = currentTransform;

                buffer.RemoveAt(0);
            }
        }
    }
}
