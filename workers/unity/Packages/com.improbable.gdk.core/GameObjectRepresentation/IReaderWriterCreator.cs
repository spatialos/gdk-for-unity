using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    internal interface IReaderWriterCreator
    {
        IReaderWriterInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher);
    }
}
