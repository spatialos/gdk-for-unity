using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
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
    [UpdateInGroup(typeof(FixedUpdate))]
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

                var currentTransform = new TransformToSet
                {
                    Position = buffer[0].Position + worker.Origin,
                    Orientation = buffer[0].Orientation
                };

                data.CurrentTransform[i] = currentTransform;

                buffer.RemoveAt(0);
            }
        }
    }
}
