using Unity.Entities;

namespace Improbable.Gdk.Core.Representation
{
    public readonly struct SpatialOSEntityInfo
    {
        public readonly string EntityType;
        public readonly Entity Entity;
        public readonly EntityId SpatialOSEntityId;

        public SpatialOSEntityInfo(string entityType, Entity entity, EntityId spatialOSEntityId)
        {
            EntityType = entityType;
            Entity = entity;
            SpatialOSEntityId = spatialOSEntityId;
        }
    }
}
