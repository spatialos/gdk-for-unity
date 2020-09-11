using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public readonly struct EntitySystemStateComponent : ISystemStateComponentData
    {
        public readonly EntityId EntityId;

        public EntitySystemStateComponent(in EntityId entityId)
        {
            EntityId = entityId;
        }

        public static explicit operator EntitySystemStateComponent(SpatialEntityId comp)
        {
            return new EntitySystemStateComponent(comp.EntityId);
        }
    }
}
