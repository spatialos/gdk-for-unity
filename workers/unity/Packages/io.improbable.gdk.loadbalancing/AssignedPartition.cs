using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.LoadBalancing
{
    public struct AssignedPartition : IComponentData
    {
        public EntityId Partition;

        public AssignedPartition(EntityId partition)
        {
            Partition = partition;
        }
    }
}