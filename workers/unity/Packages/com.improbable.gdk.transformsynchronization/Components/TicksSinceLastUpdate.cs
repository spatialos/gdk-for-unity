using Unity.Entities;

namespace Improbable.Gdk.TransformSynchronization
{
    public struct TicksSinceLastUpdate : IComponentData
    {
        public uint NumberOfTicks;
    }
}
