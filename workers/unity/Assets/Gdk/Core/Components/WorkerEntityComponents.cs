using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public struct WorkerEntityTag : IComponentData
    {
    }

    public struct IsConnected : IComponentData
    {
    }

    public struct OnConnected : IComponentData
    {
    }

    public struct OnDisconnected : ISharedComponentData
    {
        public string ReasonForDisconnect;
    }
}
