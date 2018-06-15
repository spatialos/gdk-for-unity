using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public struct HeartbeatData : IComponentData
    {
        public int NumFailedHeartbeats;
    }
}
