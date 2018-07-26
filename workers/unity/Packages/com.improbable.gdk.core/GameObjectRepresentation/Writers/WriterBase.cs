using Unity.Entities;

namespace Improbable.Gdk.Core
{
    public abstract class WriterBase<TSpatialComponentData, TComponentUpdate>
        : ReaderBase<TSpatialComponentData, TComponentUpdate> where TSpatialComponentData : ISpatialComponentData where TComponentUpdate : ISpatialComponentUpdate<TSpatialComponentData>
    {
        protected WriterBase(Entity entity, EntityManager entityManager) : base(entity, entityManager)
        {
        }
    }
}