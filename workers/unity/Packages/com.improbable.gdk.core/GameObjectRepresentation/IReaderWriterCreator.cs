using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    internal interface IReaderWriterCreator
    {
        IReaderInternal CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher);
    }
}
