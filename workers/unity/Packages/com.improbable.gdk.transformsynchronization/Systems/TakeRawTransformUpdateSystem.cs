using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(DefaultUpdateLatestTransformSystem))]
    public class TakeRawTransformUpdateSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [WriteOnly] public ComponentDataArray<TransformToSet> TransfomToSet;
            [ReadOnly] public ComponentDataArray<TransformInternal.Component> Transform;

            [ReadOnly] public ComponentDataArray<TransformInternal.ReceivedUpdates> DenotesReceivedUpdate;
        }

        [Inject] private Data data;
        [Inject] private WorkerSystem worker;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                var t = data.Transform[i];
                data.TransfomToSet[i] = new TransformToSet
                {
                    Position = t.Location.ToUnityVector3() + worker.Origin,
                    Velocity = t.Velocity.ToUnityVector3(),
                    Orientation = t.Rotation.ToUnityQuaternion()
                };
            }
        }
    }
}
