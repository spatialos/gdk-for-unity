using Improbable.Gdk.TransformSynchronization;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct DeferredUpdateTransform : IComponentData
    {
        public TransformInternal.Component Transform;
    }
}
