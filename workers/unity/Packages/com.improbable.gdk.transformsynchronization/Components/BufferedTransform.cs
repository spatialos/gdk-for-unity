using Improbable.Gdk.Core;
using UnityEngine;
using Transform = Generated.Improbable.Transform.Transform;

namespace Improbable.Gdk.TransformSynchronization
{
    public class BufferedTransform : Component
    {
        public System.Collections.Generic.List<Transform.Component> TransformUpdates;
        public Transform.Component LastTransformSnapshot;
        public BlittableBool IsInitialised;

        public BufferedTransform()
        {
            TransformUpdates = new System.Collections.Generic.List<Transform.Component>();
        }
    }
}
