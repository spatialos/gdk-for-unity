using Unity.Entities;

namespace Playground
{
    public struct GameObjectReferenceHandle : ISystemStateComponentData
    {
        public uint GameObjectHandle;
    }
}
