using System;
using Improbable.Gdk.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(PartitionManagementSystem))]
    public class ClassifyWorkersSystem : SystemBase
    {
        protected override void OnCreate()
        {
        }

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            Entities
                .WithoutBurst()
                .WithNone<WorkerClassification>()
                .ForEach((Entity entity, in Improbable.Restricted.Worker.Component worker) =>
            {
                ecb.AddSharedComponent(entity, new WorkerClassification(worker.WorkerType));
            }).Run();
            
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }

    public readonly struct WorkerClassification : ISharedComponentData, IEquatable<WorkerClassification>
    {
        public readonly FixedString64 WorkerType;

        public WorkerClassification(string workerType)
        {
            WorkerType = new FixedString64(workerType);
        }

        public bool Equals(WorkerClassification other)
        {
            return WorkerType == other.WorkerType;
        }

        public override bool Equals(object obj)
        {
            return obj is WorkerClassification other && Equals(other);
        }

        public override int GetHashCode()
        {
            return WorkerType.GetHashCode();
        }

        public static bool operator ==(WorkerClassification left, WorkerClassification right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WorkerClassification left, WorkerClassification right)
        {
            return !left.Equals(right);
        }
    }
}
