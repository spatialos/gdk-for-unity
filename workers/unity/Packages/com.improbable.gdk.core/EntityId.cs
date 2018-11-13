using System;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A unique identifier used to look up an entity in SpatialOS.
    /// </summary>
    /// <remarks>
    ///     Instances of this type should be treated as transient identifiers that will not be
    ///     consistent between different runs of the same simulation.
    /// </remarks>
    public struct EntityId : IEquatable<EntityId>
    {
        /// <summary>
        ///     The value of the EntityId.
        /// </summary>
        /// <remarks>
        ///     Though this value is numeric, you should not perform any mathematical operations on it.
        /// </remarks>
        public readonly long Id;

        /// <summary>
        ///     Constructs a new instance of an EntityId.
        /// </summary>
        public EntityId(long id)
        {
            Id = id;
        }

        /// <summary>
        ///     Whether this represents a valid SpatialOS entity ID. Specifically, <code>Id > 0</code>.
        /// </summary>
        /// <returns>True iff valid.</returns>
        public bool IsValid()
        {
            return Id > 0;
        }

        /// <inheritdoc cref="IEquatable{T}" />
        public override bool Equals(object obj)
        {
            if (!(obj is EntityId))
            {
                return false;
            }

            return Equals((EntityId) obj);
        }

        /// <inheritdoc cref="IEquatable{T}" />
        public bool Equals(EntityId obj)
        {
            return Id.Equals(obj.Id);
        }

        /// <summary>
        ///     Returns true if entityId1 is exactly equal to entityId2.
        /// </summary>
        public static bool operator ==(EntityId entityId1, EntityId entityId2)
        {
            return entityId1.Equals(entityId2);
        }

        /// <summary>
        ///     Returns true if entityId1 is not exactly equal to entityId2.
        /// </summary>
        public static bool operator !=(EntityId entityId1, EntityId entityId2)
        {
            return !entityId1.Equals(entityId2);
        }

        /// <summary>
        ///     Implicit cast operator from EntityId to long.
        /// </summary>
        /// <param name="entityId">The EntityId to cast.</param>
        /// <returns>A long that represents that EntityId.</returns>
        public static implicit operator long(EntityId entityId)
        {
            return entityId.Id;
        }

        /// <summary>
        ///     Implicit cast operator from long to EntityId
        /// </summary>
        /// <param name="entityId">The long to cast.</param>
        /// <returns>A EntityId that represents that long.</returns>
        public static implicit operator EntityId(long entityId)
        {
            return new EntityId(entityId);
        }

        /// <inheritdoc cref="object" />
        public override int GetHashCode()
        {
            var res = 1327;
            res = res * 977 + Id.GetHashCode();
            return res;
        }

        /// <inheritdoc cref="object" />
        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
