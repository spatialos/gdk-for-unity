using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SpatialOSUpdateGroup))]
    [UpdateBefore(typeof(PartitionManagementSystem))]
    public class ClassifyWorkersSystem : ComponentSystem
    {
        private EntityQuery newWorkers;

        protected override void OnCreate()
        {
            newWorkers = GetEntityQuery(ComponentType.ReadOnly<Improbable.Restricted.Worker.Component>(),
                ComponentType.Exclude<WorkerClassification>());
        }

        protected override void OnUpdate()
        {
            Entities.With(newWorkers).ForEach((Entity entity, ref Improbable.Restricted.Worker.Component worker) =>
            {
                PostUpdateCommands.AddSharedComponent(entity, new WorkerClassification(worker.WorkerType));
            });
        }
    }

    public struct WorkerClassification : ISharedComponentData, IEquatable<WorkerClassification>
    {
        public string WorkerType;

        public WorkerClassification(string workerType)
        {
            WorkerType = workerType;
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
            return (WorkerType != null ? WorkerType.GetHashCode() : 0);
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
