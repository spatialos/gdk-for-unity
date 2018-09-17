using Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct LastTransformSentData : IComponentData
    {
        public TransformInternal.Component Transform;
        public float TimeSinceLastUpdate;
    }
}
