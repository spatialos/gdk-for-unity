using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public struct GameObjectReferenceHandle : ISystemStateComponentData
    {
        public uint GameObjectHandle;
    }
}
