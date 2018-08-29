using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.TransformSynchronization
{
    public class BufferedTransform : Component
    {
        public System.Collections.Generic.List<SpatialOSTransform> TransformUpdates;
        public SpatialOSTransform LastTransformSnapshot;
        public BlittableBool IsInitialised;

        public BufferedTransform()
        {
            TransformUpdates = new System.Collections.Generic.List<SpatialOSTransform>();
        }
    }
}
