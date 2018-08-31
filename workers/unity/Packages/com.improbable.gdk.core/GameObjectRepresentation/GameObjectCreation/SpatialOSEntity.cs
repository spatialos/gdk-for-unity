using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Core.GameObjectRepresentation
{
    /// <summary>
    ///     Wrapper around an ECS Entity instance representing a SpatialOS Entity used for interacting with the the
    ///     entity without touching ECS code like EntityManagers.
    /// </summary>
    public struct SpatialOSEntity
    {
        private readonly EntityManager entityManager;
        private readonly Entity entity;
        public readonly EntityId SpatialEntityId;

        internal SpatialOSEntity(Entity entity, EntityManager entityManager)
        {
            this.entity = entity;
            this.entityManager = entityManager;
            SpatialEntityId = entityManager.GetComponentData<SpatialEntityId>(entity).EntityId;
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
