using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public struct CreatePlayerRequestTrigger : ISharedComponentData
    {
        public Vector3f SpawnPosition;
        public byte[] SerializedArguments;
    }
}
