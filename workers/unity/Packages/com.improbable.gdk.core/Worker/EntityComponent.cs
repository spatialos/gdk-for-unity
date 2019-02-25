using System;

namespace Improbable.Gdk.Core
{
    public readonly struct EntityComponent : IEquatable<EntityComponent>
    {
        public readonly long EntityId;
        public readonly uint ComponentId;

        public EntityComponent(long entityId, uint componentId)
        {
            ComponentId = componentId;
            EntityId = entityId;
        }

        public bool Equals(EntityComponent other)
        {
            return EntityId == other.EntityId && ComponentId == other.ComponentId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is EntityComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EntityId.GetHashCode() * 397) ^ (int) ComponentId;
            }
        }
    }
}
