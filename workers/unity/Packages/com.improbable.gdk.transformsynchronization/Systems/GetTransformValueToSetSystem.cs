﻿using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(FixedUpdateSystemGroup))]
    public class GetTransformValueToSetSystem : ComponentSystem
    {
        private WorkerSystem worker;

        private EntityQuery transformGroup;

        protected override void OnCreate()
        {
            base.OnCreate();

            worker = World.GetExistingSystem<WorkerSystem>();

            transformGroup = GetEntityQuery(
                ComponentType.ReadWrite<BufferedTransform>(),
                ComponentType.ReadWrite<TransformToSet>(),
                ComponentType.ReadOnly<TransformInternal.ComponentAuthority>()
            );
            transformGroup.SetFilter(TransformInternal.ComponentAuthority.NotAuthoritative);
        }

        protected override void OnUpdate()
        {
            Entities.With(transformGroup).ForEach(
                (DynamicBuffer<BufferedTransform> buffer, ref TransformToSet transformToSet) =>
                {
                    if (buffer.Length == 0)
                    {
                        return;
                    }

                    var bufferHead = buffer[0];

                    transformToSet.Position = bufferHead.Position + worker.Origin;
                    transformToSet.Orientation = bufferHead.Orientation;
                    transformToSet.Velocity = bufferHead.Velocity;
                    transformToSet.ApproximateRemoteTick = bufferHead.PhysicsTick;

                    buffer.RemoveAt(0);
                });
        }
    }
}
