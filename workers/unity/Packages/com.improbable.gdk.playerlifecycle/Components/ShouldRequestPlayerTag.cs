using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public struct ShouldRequestPlayerTag : ISharedComponentData
    {
        public Vector3f SpawnPosition;
        public byte[] SerializedArguments;
    }
}
