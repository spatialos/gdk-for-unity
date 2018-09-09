using Generated.Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct LastTransformSentData : IComponentData
    {
        public Transform.Component Transform;
        public float TimeSinceLastUpdate;
    }
}
