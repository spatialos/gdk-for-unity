using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public interface IReader<TComponent, out TComponentUpdate>
        where TComponent : ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TComponent>
    {
        Authority Authority { get; }

        event AuthorityChangedDelegate AuthorityChanged;
        event ComponentUpdateDelegate<TComponentUpdate> ComponentUpdated;
    }
}