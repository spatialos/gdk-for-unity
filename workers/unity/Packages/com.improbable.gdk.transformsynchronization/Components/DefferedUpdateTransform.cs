using Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct DefferedUpdateTransform : IComponentData
    {
        public TransformInternal.Component Transform;
    }
}
