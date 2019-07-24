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
        ///     Utility method for creating a TransformInternal Snapshot.
        /// </summary>
        /// <param name="location">
        ///     The location of an entity, given as Improbable Coordinates.
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
            Coordinates location = default,
            Quaternion rotation = default,
            Vector3 velocity = default)
        {
            return new TransformInternal.Snapshot
            {
                Location = FixedPointVector3.FromCoordinates(location),
                Rotation = CompressedQuaternion.FromUnityQuaternion(rotation),
                Velocity = FixedPointVector3.FromUnityVector(velocity),
                TicksPerSecond = 1f / Time.fixedDeltaTime
            };
        }

        /// <summary>
        ///     Extension method for converting a Unity Vector to a Coordinates value.
        /// </summary>
        /// <remarks>
        ///     This method wraps Coordinates.FromUnityVector.
        /// </remarks>
        public static Coordinates ToCoordinates(this Vector3 unityVector)
        {
            return Coordinates.FromUnityVector(unityVector);
        }

        /// <summary>
        ///     Extension method for converting a Unity Vector to a FixedPointVector3.
        /// </summary>
        /// <remarks>
        ///     This method wraps FixedPointVector3.FromUnityVector.
        /// </remarks>
        public static FixedPointVector3 ToFixedPointVector3(this Vector3 unityVector)
        {
            return FixedPointVector3.FromUnityVector(unityVector);
        }

        /// <summary>
        ///     Extension method for converting a Coordinates value to a FixedPointVector3.
        /// </summary>
        /// <remarks>
        ///     This method wraps FixedPointVector3.FromCoordinates.
        /// </remarks>
        public static FixedPointVector3 ToFixedPointVector3(this Coordinates coordinates)
        {
            return FixedPointVector3.FromCoordinates(coordinates);
        }

        /// <summary>
        ///     Extension method for converting a Quaternion to a CompressedQuaternion.
        /// </summary>
        /// <remarks>
        ///     This method wraps CompressedQuaternion.FromUnityQuaternion.
        /// </remarks>
        public static CompressedQuaternion ToCompressedQuaternion(this Quaternion quaternion)
        {
            return CompressedQuaternion.FromUnityQuaternion(quaternion);
        }
    }
}
