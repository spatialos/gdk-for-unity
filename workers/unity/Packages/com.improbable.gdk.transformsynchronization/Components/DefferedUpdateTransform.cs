using Generated.Improbable.Transform;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct DefferedUpdateTransform : IComponentData
    {
        public Transform.Component Transform;
    }
}
