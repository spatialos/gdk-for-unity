using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public interface ISpatialComponentUpdate
    {
    }

    public interface ISpatialComponentUpdate<TSpatialComponentData> : ISpatialComponentUpdate
        where TSpatialComponentData : ISpatialComponentData
    {
    }
}
