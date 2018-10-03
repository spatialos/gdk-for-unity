using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     ECS Component denoting a worker entity
    /// </summary>
    public struct WorkerEntityTag : IComponentData
    {
    }

    /// <summary>
    ///     ECS Component added to the worker entity immediately after establishing a connection to a SpatialOS deployment.
    /// </summary>
    /// <remarks>
    ///    This is a reactive component and the <see cref="Improbable.Gdk.Core.CleanReactiveComponentsSystem"/> will
    ///    remove it at the end of the frame.
    /// </remarks>
    [RemoveAtEndOfTick]
    public struct OnConnected : IComponentData
    {
    }

    /// <summary>
    ///     ECS Component added to the worker entity immediately after disconnecting from SpatialOS
    /// </summary>
    /// <remarks>
    ///    This is a reactive component and the <see cref="Improbable.Gdk.Core.CleanReactiveComponentsSystem"/> will
    ///    remove it at the end of the frame.
    /// </remarks>
    [RemoveAtEndOfTick]
    public struct OnDisconnected : ISharedComponentData
    {
        /// <summary>
        ///     The reported reason for disconnecting
        /// </summary>
        public string ReasonForDisconnect;
    }
}
