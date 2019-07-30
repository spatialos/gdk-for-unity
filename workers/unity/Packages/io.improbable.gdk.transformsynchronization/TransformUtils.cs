using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    /// <summary>
    ///     A collection of utility functions for use with the Transform Synchronization Feature Module.
    /// </summary>
    public static class TransformUtils
    {
        /// <summary>
        ///     Utility method for creating a TransformInternal Snapshot.
        /// </summary>
        /// <param name="location">
        ///     The location of an entity, given as a Unity Vector3.
        /// </param>
        /// <param name="rotation">
        ///     The rotation of an entity, given as a Unity Quaternion.
        /// </param>
        /// <param name="velocity">
        ///     The velocity of an entity, given as a Unity Vector3.
        /// </param>
        /// <remarks>
        ///     This method populates a TransformInternal with compressed representations of the given arguments.
        /// </remarks>
        public static TransformInternal.Snapshot CreateTransformSnapshot(
            Vector3 location = default,
            Quaternion rotation = default,
            Vector3 velocity = default)
        {
            return new TransformInternal.Snapshot
            {
                Location = FixedPointVector3.FromUnityVector(location),
                Rotation = CompressedQuaternion.FromUnityQuaternion(rotation),
                Velocity = FixedPointVector3.FromUnityVector(velocity),
                TicksPerSecond = 1f / Time.fixedDeltaTime
            };
        }

        /// <summary>
        ///     Extension method for converting a Unity Vector to a Coordinates value.
        /// </summary>
        public static Coordinates ToCoordinates(this Vector3 unityVector)
        {
            return Coordinates.FromUnityVector(unityVector);
        }

        /// <summary>
        ///     Extension method for converting a Unity Vector to a FixedPointVector3.
        /// </summary>
        public static FixedPointVector3 ToFixedPointVector3(this Vector3 unityVector)
        {
            return FixedPointVector3.FromUnityVector(unityVector);
        }

        /// <summary>
        ///     Extension method for converting a Quaternion to a CompressedQuaternion.
        /// </summary>
        public static CompressedQuaternion ToCompressedQuaternion(this Quaternion quaternion)
        {
            return CompressedQuaternion.FromUnityQuaternion(quaternion);
        }

        /// <summary>
        ///     Constructs an <see cref="EntityQueryDesc"/> given an array of base types and
        ///     a generic component type.
        /// </summary>
        /// <param name="baseTypes">The base set of types.</param>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <returns>An <see cref="EntityQueryDesc"/> that is the union of <see cref="baseTypes"/> and typeof(<see cref="T"/>)</returns>
        internal static EntityQueryDesc ConstructEntityQueryDesc<T>(params ComponentType[] baseTypes)
        {
            var componentType = ComponentType.ReadOnly<T>();

            var includedComponentTypes = baseTypes
                .Append(componentType)
                .ToArray();

            var componentQueryDesc = new EntityQueryDesc
            {
                All = includedComponentTypes
            };

            return componentQueryDesc;
        }
    }
}
