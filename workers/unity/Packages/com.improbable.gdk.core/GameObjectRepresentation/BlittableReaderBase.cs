using Unity.Entities;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public abstract class BlittableReaderBase<TComponent, TComponentUpdate>
        : ReaderBase<TComponent, TComponentUpdate>
        where TComponent : struct, ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TComponent>
    {
        protected BlittableReaderBase(Entity entity, EntityManager entityManager) : base(entity, entityManager)
        {
        }

        public override TComponent Data => EntityManager.GetComponentData<TComponent>(Entity);
    }
}
