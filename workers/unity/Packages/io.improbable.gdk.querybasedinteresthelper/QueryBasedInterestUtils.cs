using UnityEngine;

namespace Improbable.Gdk.QueryBasedInterest
{
    public static class QueryBasedInterestUtils
    {
        /// <summary>
        ///     Extension method for converting a Unity Vector to an EdgeLength.
        /// </summary>
        /// <remarks>
        ///     This method wraps EdgeLength.FromUnityVector.
        /// </remarks>
        public static EdgeLength ToEdgeLength(this Vector3 unityVector)
        {
            return EdgeLength.FromUnityVector(unityVector);
        }
    }
}
