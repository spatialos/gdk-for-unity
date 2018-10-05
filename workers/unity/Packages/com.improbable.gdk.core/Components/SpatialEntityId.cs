using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     ECS component which contains the SpatialOS Entity ID.
    /// </summary>
    public struct SpatialEntityId : IComponentData
    {
        public EntityId EntityId;
    }
}
