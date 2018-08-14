using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    internal interface IReaderWriterCreator
    {
        IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher);
    }
}
