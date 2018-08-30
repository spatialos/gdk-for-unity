using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    public struct SpatialOSEntity
    {
        private readonly EntityManager entityManager;
        private readonly Entity entity;
        public readonly EntityId SpatialEntityId;
        public readonly int UnityEntityIndex;

        internal SpatialOSEntity(Entity entity, EntityManager entityManager)
        {
            this.entity = entity;
            this.entityManager = entityManager;
            SpatialEntityId = entityManager.GetComponentData<SpatialEntityId>(entity).EntityId;
            UnityEntityIndex = entity.Index;
        }

        public bool HasComponent<T>() where T : struct, ISpatialComponentData, IComponentData
        {
            return entityManager.HasComponent<T>(entity);
        }

        public T GetComponent<T>() where T : struct, ISpatialComponentData, IComponentData
        {
            return entityManager.GetComponentData<T>(entity);
        }
    }
}
