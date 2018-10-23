using Improbable.Gdk.Core;
using Improbable.Transform;
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
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(DefaultUpdateLatestTransformSystem))]
    public class DirectTransformUpdateSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [WriteOnly] public ComponentDataArray<TransformToSet> TransfomToSet;
            [ReadOnly] public ComponentDataArray<TransformInternal.Component> Transform;

            [ReadOnly] public ComponentDataArray<TransformInternal.ReceivedUpdates> DenotesReceivedUpdate;
            [ReadOnly] public ComponentDataArray<NotAuthoritative<TransformInternal.Component>> DenotesNotAuthoritative;
            [ReadOnly] public ComponentDataArray<DirectReceiveTag> DenotesShouldUseDirectReceive;
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
