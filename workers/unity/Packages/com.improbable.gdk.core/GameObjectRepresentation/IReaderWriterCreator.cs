using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    internal interface IReaderWriterCreator
    {
        object CreateReaderWriter(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher);
    }
}
