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
    ///     Removed immediately after disconnecting
    /// </summary>
    public struct IsConnected : IComponentData
    {
    }

    /// <summary>
    ///     Component added to the worker entity immediately after establishing a connection to a SpatialOS deployment
    ///     Removed at the end of the tick it was added
    /// </summary>
    public struct OnConnected : IComponentData, IRemoveableComponent
    {
        public void RemoveComponent(EntityCommandBuffer commands, Entity entity)
        {
            commands.RemoveComponent<OnConnected>(entity);
        }
    }

    /// <summary>
    ///     Component added to the worker entity immediately after disconnecting from SpatialOS
    ///     Removed at the end of the tick it was added
    /// </summary>
    public struct OnDisconnected : ISharedComponentData, IRemoveableComponent
    {
        /// <summary>
        ///     The reported reason for disconnecting
        /// </summary>
        public string ReasonForDisconnect;
        public void RemoveComponent(EntityCommandBuffer commands, Entity entity)
        {
            commands.RemoveComponent<OnDisconnected>(entity);
        }
    }
}
