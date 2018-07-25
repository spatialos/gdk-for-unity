using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public abstract class NonBlittableReaderBase<TSpatialComponentData, TComponentUpdate>
        : ReaderBase<TSpatialComponentData, TComponentUpdate>
        where TSpatialComponentData : Component, ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TSpatialComponentData>
    {
        protected NonBlittableReaderBase(Entity entity, EntityManager entityManager) : base(entity, entityManager)
        {
        }

        public override TSpatialComponentData Data => EntityManager.GetComponentObject<TSpatialComponentData>(Entity);
    }
}
