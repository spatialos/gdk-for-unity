using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    /// <summary>
    ///     The Reader interface is used by MonoBehaviours to query information about a SpatialOS component and register
    ///     callbacks for updates, events and commands for component instances.
    /// </summary>
    /// <typeparam name="TComponent">The data type for the SpatialOS component.</typeparam>
    public interface IReader<TComponent>
        where TComponent : ISpatialComponentData, IComponentData
    {
        Authority Authority { get; }

        event GameObjectDelegates.AuthorityChanged AuthorityChanged;
        event GameObjectDelegates.ComponentUpdated<TComponent> ComponentUpdated;
    }
}
