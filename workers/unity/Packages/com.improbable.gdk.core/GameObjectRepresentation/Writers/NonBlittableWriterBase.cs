using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public abstract class NonBlittableWriterBase<TSpatialComponentData, TComponentUpdate>
        : WriterBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : Component, ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate
    {
        protected NonBlittableWriterBase(Entity entity, EntityManager entityManager,
            IReader<TSpatialComponentData, TComponentUpdate> reader)
            : base(entity, entityManager, reader)
        {
        }

        protected sealed override void SetDataInEntity(TSpatialComponentData newData)
        {
            EntityManager.SetComponentObject(Entity, newData);
        }

        public sealed override void CopyAndSend(TComponentUpdate update)
        {
            // TODO make a deep copy of the update?
            throw new System.NotImplementedException();
        }
    }
}
