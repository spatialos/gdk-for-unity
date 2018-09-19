using UnityEngine;

namespace Improbable
{
    public static class Vector3fExtensions
    {
        /// <summary>
        ///     Conversion method from Unity Vector3 to Spatial Vector3f.
        /// </summary>
        /// <param name="vector">The Vector3 to convert.</param>
        /// <returns>The converted Vector3f.</returns>
        public static Vector3f ToSpatialVector3f(this Vector3 vector)
        {
            return new Vector3f(vector.x, vector.y, vector.z);
        }

        /// <summary>
        ///     Conversion method from Spatial Vector3f to Unity Vector3
        /// </summary>
        /// <param name="vector">The Vector3f to convert.</param>
        /// <returns>The converted Vector3.</returns>
        public static Vector3 ToUnityVector(this Vector3f vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}
