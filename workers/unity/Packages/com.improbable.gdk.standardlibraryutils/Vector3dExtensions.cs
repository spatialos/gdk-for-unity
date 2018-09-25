using UnityEngine;

namespace Improbable
{
    public static class Vector3dExtensions
    {
        /// <summary>
        ///     Conversion method from Unity Vector3 to Spatial Vector3d.
        /// </summary>
        /// <param name="vector">The Vector3 to convert.</param>
        /// <returns>The converted Vector3d.</returns>
        public static Vector3d ToSpatialVector3d(this Vector3 vector)
        {
            return new Vector3d(vector.x, vector.y, vector.z);
        }

        /// <summary>
        ///     Conversion method from Spatial Vector3d to Unity Vector3.
        ///     Be aware this involves a downcast from double to float and
        ///     overflow may occur.
        /// </summary>
        /// <param name="vector">The Vector3d to convert.</param>
        /// <returns>The converted Vector3.</returns>
        public static Vector3 NarrowToUnityVector(this Vector3d vector)
        {
            return new Vector3((float) vector.X, (float) vector.Y, (float) vector.Z);
        }
    }
}
