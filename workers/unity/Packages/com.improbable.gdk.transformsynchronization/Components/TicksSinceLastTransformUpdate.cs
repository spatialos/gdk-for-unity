using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct TicksSinceLastTransformUpdate : IComponentData
    {
        public uint NumberOfTicks;
    }
}
