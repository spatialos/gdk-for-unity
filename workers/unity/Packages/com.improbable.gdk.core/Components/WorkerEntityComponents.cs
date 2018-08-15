using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Component denoting a worker entity
    /// </summary>
    public struct WorkerEntityTag : IComponentData
    {
    }

    /// <summary>
    ///     Component added to the worker entity immediately after establishing a connection to a SpatialOS deployment
    ///     Removed at the end of the tick it was added
    /// </summary>
    [RemoveAtEndOfTick]
    public struct OnConnected : IComponentData
    {
    }

    /// <summary>
    ///     Component added to the worker entity immediately after disconnecting from SpatialOS
    ///     Removed at the end of the tick it was added
    /// </summary>
    [RemoveAtEndOfTick]
    public struct OnDisconnected : ISharedComponentData
    {
        /// <summary>
        ///     The reported reason for disconnecting
        /// </summary>
        public string ReasonForDisconnect;
    }
}
