using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.MonoBehaviours
{
    public abstract class NonBlittableReaderBase<TComponent, TComponentUpdate>
        : ReaderBase<TComponent, TComponentUpdate>
        where TComponent : Component, ISpatialComponentData
        where TComponentUpdate : ISpatialComponentUpdate<TComponent>
    {
        protected NonBlittableReaderBase(Entity entity, EntityManager entityManager) : base(entity, entityManager)
        {
        }

        public override TComponent Data => EntityManager.GetComponentObject<TComponent>(Entity);
    }
}
