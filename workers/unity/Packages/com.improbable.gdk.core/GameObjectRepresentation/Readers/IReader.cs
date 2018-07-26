using Improbable.Worker;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     The Reader interface is used by MonoBehaviours to query information about a SpatialOS component and register
    ///     callbacks for updates, events and commands for component instances.
    /// </summary>
    /// <typeparam name="TSpatialComponentData">The data type for the SpatialOS component.</typeparam>
    /// <typeparam name="TComponentUpdate">The update data type for the SpatialOS component.</typeparam>
    public interface IReader<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        Authority Authority { get; }
        TSpatialComponentData Data { get; }

        event GameObjectDelegates.AuthorityChanged AuthorityChanged;
        event GameObjectDelegates.ComponentUpdated<TComponentUpdate> ComponentUpdated;
    }
}
