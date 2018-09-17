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
    [UpdateAfter(typeof(SpatialOSReceiveGroup))]
    public class DefaultTransformInitializationSystem : ComponentSystem
    {
        private struct Data
        {
            public readonly int Length;
            [ReadOnly] public EntityArray Entity;
            [ReadOnly] public ComponentDataArray<TransformInternal.Component> Transform;

            // This can't work once we put proper interest in. Will need to completely decouple transform and position
            [ReadOnly] public ComponentDataArray<Position.Component> Position;
            [ReadOnly] public ComponentDataArray<NewlyAddedSpatialOSEntity> DenotesNewEntity;
        }

        [Inject] private Data data;
        [Inject] private WorkerSystem worker;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; ++i)
            {
                // Need to split this out into server / client / whatever
                var transform = data.Transform[i];
                var defaultToSet = new TransformToSet
                {
                    Position = transform.Location.ToUnityVector3() + worker.Origin,
                    Velocity = transform.Velocity.ToUnityVector3(),
                    Orientation = transform.Rotation.ToUnityQuaternion()
                };
                var defaultToSend = new TransformToSend
                {
                    Position = transform.Location.ToUnityVector3() - worker.Origin,
                    Velocity = transform.Velocity.ToUnityVector3(),
                    Orientation = transform.Rotation.ToUnityQuaternion()
                };
                var previousTransform = new DefferedUpdateTransform
                {
                    Transform = transform
                };
                var ticksSinceLastUpdate = new TicksSinceLastTransformUpdate
                {
                    NumberOfTicks = 0
                };
                var lastTransform = new LastTransformSentData
                {
                    // could set this to the max time if we want to immediately send something
                    TimeSinceLastUpdate = 0.0f,
                    Transform = transform
                };
                var lastPosition = new LastPositionSentData
                {
                    // could set this to the max time if we want to immediately send something
                    TimeSinceLastUpdate = 0.0f,
                    Position = data.Position[i]
                };
                PostUpdateCommands.AddComponent(data.Entity[i], defaultToSet);
                PostUpdateCommands.AddComponent(data.Entity[i], defaultToSend);
                PostUpdateCommands.AddComponent(data.Entity[i], previousTransform);
                PostUpdateCommands.AddComponent(data.Entity[i], ticksSinceLastUpdate);
                PostUpdateCommands.AddComponent(data.Entity[i], lastPosition);
                PostUpdateCommands.AddComponent(data.Entity[i], lastTransform);
                PostUpdateCommands.AddBuffer<BufferedTransform>(data.Entity[i]);
            }
        }
    }
}
