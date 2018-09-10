using Improbable.Gdk.Core;
using Improbable.Worker;
using Improbable.Worker.Core;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     The Reader interface is used by MonoBehaviours to query information about a SpatialOS component and register
    ///     callbacks for updates, events and commands for component instances.
    /// </summary>
    /// <typeparam name="TSpatialComponentData">The data type for the SpatialOS component.</typeparam>
    /// <typeparam name="TComponentUpdate">The update type for the SpatialOS component.</typeparam>
    public interface IReader<TSpatialComponentData, TComponentUpdate> : IInjectable
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        TSpatialComponentData Data { get; }
        Authority Authority { get; }

        event GameObjectDelegates.ComponentUpdated<TComponentUpdate> ComponentUpdated;
        event GameObjectDelegates.AuthorityChanged AuthorityChanged;
    }
}
