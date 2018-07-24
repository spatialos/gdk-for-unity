using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    /// <summary>
    ///     The Reader interface is used by MonoBehaviours to query information about a SpatialOS component and register
    ///     callbacks for updates, events and commands for component instances.
    /// </summary>
    /// <typeparam name="TComponent">The data type for the SpatialOS component.</typeparam>
    /// <typeparam name="TComponentUpdate">The data type for the SpatialOS component's update.</typeparam>
    public interface IReader<TComponent, TComponentUpdate>
        where TComponent : ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TComponent>
    {
        Authority Authority { get; }

        event AuthorityChangedDelegate AuthorityChanged;
        event ComponentUpdateDelegate<TComponentUpdate> ComponentUpdated;
    }
}
