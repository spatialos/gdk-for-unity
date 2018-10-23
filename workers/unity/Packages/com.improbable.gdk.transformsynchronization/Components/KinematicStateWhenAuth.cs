using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct KinematicStateWhenAuth : IComponentData
    {
        public BlittableBool KinematicWhenAuthoritative;
    }
}
