using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public abstract class BlittableWriterBase<TSpatialComponentData, TComponentUpdate>
        : WriterBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : struct, ISpatialComponentData, IComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        protected BlittableWriterBase(Entity entity, EntityManager entityManager,
            IReader<TSpatialComponentData, TComponentUpdate> reader)
            : base(entity, entityManager, reader)
        {
        }

        protected sealed override void SetDataInEntity(TSpatialComponentData newData)
        {
            EntityManager.SetComponentData(Entity, newData);
        }

        public sealed override void CopyAndSend(TComponentUpdate update)
        {
            // since the component is blittable, it does not need to do a deep copy.
            Send(update);
        }
    }
}
