using Improbable.Gdk.Core;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     The Writer interface is used by MonoBehaviours to send SpatialOS component updates. Also includes the
    ///     functionality of the <see cref="IReader{TSpatialComponentData,TComponentUpdate}"/> interface.
    /// </summary>
    /// <typeparam name="TSpatialComponentData">The data type for the SpatialOS component.</typeparam>
    /// <typeparam name="TComponentUpdate">The update type for the SpatialOS component.</typeparam>
    public interface IWriter<TSpatialComponentData, TComponentUpdate>
        : IReader<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        void Send(TComponentUpdate update);
    }
}
