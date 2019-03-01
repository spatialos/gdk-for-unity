using Improbable.Gdk.Core;
using Improbable.Transform;
using Unity.Entities;
using UnityEngine.Experimental.PlayerLoop;

#region Diagnostic control

// ReSharper disable ClassNeverInstantiated.Global

#endregion


namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateBefore(typeof(FixedUpdate.PhysicsFixedUpdate))]
    public class GetTransformValueToSetSystem : ComponentSystem
    {
        private WorkerSystem worker;

        private ComponentGroup transformGroup;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();

            worker = World.GetExistingManager<WorkerSystem>();

            transformGroup = GetComponentGroup(
                ComponentType.Create<BufferedTransform>(),
                ComponentType.Create<TransformToSet>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>());
        }

        protected override void OnUpdate()
        {
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);

            var transformToSetArray = transformGroup.GetComponentDataArray<TransformToSet>();
            var transformBufferArray = transformGroup.GetBufferArray<BufferedTransform>();

            for (int i = 0; i < transformToSetArray.Length; ++i)
            {
                var buffer = transformBufferArray[i];
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

                transformToSetArray[i] = currentTransform;
                buffer.RemoveAt(0);
            }
        }
    }
}
