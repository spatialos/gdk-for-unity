using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    /// <summary>
    ///     The Reader interface is used by MonoBehaviours to query information about a SpatialOS component and register
    ///     callbacks for updates, events and commands for component instances.
    /// </summary>
    /// <typeparam name="TSpatialComponentData">The data type for the SpatialOS component.</typeparam>
    public interface IReader<TSpatialComponentData>
        where TSpatialComponentData : ISpatialComponentData
    {
        Authority Authority { get; }
        TSpatialComponentData Data { get; }

        event GameObjectDelegates.AuthorityChanged AuthorityChanged;
        event GameObjectDelegates.ComponentUpdated<TSpatialComponentData> ComponentUpdated;
    }
}
