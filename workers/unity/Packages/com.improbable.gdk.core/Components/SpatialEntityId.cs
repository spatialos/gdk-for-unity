using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public struct SpatialEntityId : IComponentData
    {
        public EntityId EntityId;
    }
}
