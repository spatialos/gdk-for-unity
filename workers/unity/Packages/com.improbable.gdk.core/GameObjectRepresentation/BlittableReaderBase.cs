using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public abstract class BlittableReaderBase<TSpatialComponentData, TComponentUpdate>
        : ReaderBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : struct, ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TSpatialComponentData>
    {
        protected BlittableReaderBase(Entity entity, EntityManager entityManager) : base(entity, entityManager)
        {
        }

        public override TSpatialComponentData Data => EntityManager.GetComponentData<TSpatialComponentData>(Entity);
    }
}
