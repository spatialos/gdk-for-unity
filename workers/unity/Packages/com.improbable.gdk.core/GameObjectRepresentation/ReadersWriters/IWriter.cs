using Improbable.Gdk.Core;

namespace Improbable.Gdk.GameObjectRepresentation
{
    public interface IWriter<TSpatialComponentData, TComponentUpdate>
        : IReader<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        void Send(TComponentUpdate update);
    }
}
