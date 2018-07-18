using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Provides a helper method for calling Object.DestroyImmediate() instead of Object.Destroy() in EditMode unit tests.
    /// </summary>
    public static class UnityObjectDestroyer
    {
        public static void Destroy(Object obj)
        {
            // This is false when running EditMode tests and true otherwise.
            if (Application.isPlaying)
            {
                Object.Destroy(obj);
            }
            else
            {
                Object.DestroyImmediate(obj);
            }
        }
    }
}
