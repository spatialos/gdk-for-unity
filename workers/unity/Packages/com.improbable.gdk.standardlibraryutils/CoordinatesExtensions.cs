using UnityEngine;

namespace Improbable
{
    public static class CoordinatesExtensions
    {
        /// <summary>
        ///     Conversion method from Unity Vector3 to Spatial Coordinates.
        /// </summary>
        /// <param name="vector">The Vector3 to convert.</param>
        /// <returns>The converted Coordinates.</returns>
        public static Coordinates ToSpatialCoordinates(this Vector3 vector)
        {
            return new Coordinates(vector.x, vector.y, vector.z);
        }

        /// <summary>
        ///     Conversion method from Spatial Coordinates to Unity Vector3.
        ///     Be aware this involves a downcast from double to float and
        ///     overflow may occur.
        /// </summary>
        /// <param name="coordinates">The Coordinates to convert.</param>
        /// <returns>The converted Vector3.</returns>
        public static Vector3 NarrowToUnityVector(this Coordinates coordinates)
        {
            return new Vector3((float) coordinates.X, (float) coordinates.Y, (float) coordinates.Z);
        }
    }
}
