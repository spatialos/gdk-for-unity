using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct KinematicStateWhenAuth : IComponentData
    {
        public bool KinematicWhenAuthoritative;
    }
}
