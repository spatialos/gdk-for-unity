using Generated.Improbable.Transform;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public class BufferedTransform : Component
    {
        public System.Collections.Generic.List<SpatialOSTransform> TransformUpdates;

        public BufferedTransform()
        {
            TransformUpdates = new System.Collections.Generic.List<SpatialOSTransform>();
        }
    }
}
