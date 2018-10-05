using Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct DeferredUpdateTransform : IComponentData
    {
        public TransformInternal.Component Transform;
    }
}
