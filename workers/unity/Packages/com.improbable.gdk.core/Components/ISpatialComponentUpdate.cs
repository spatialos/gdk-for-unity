using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface ISpatialComponentUpdate
    {
    }

    public interface ISpatialComponentUpdate<TComponent> : ISpatialComponentUpdate
        where TComponent : ISpatialComponentData, IComponentData
    {
    }
}
