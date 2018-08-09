namespace Improbable.Gdk.Core.MonoBehaviours
{
    public interface IWriter<TSpatialComponentData>
        : IReader<TSpatialComponentData>
        where TSpatialComponentData : ISpatialComponentData
    {
    }

    public interface IWriter<TSpatialComponentData, TComponentUpdate>
        : IReader<TSpatialComponentData, TComponentUpdate>, IWriter<TSpatialComponentData>
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        void Send(TComponentUpdate update);
    }
}
