using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     Used to easily retrieve information about a SpatialOS Entity instance from a Unity ECS Entity instance.
    /// </summary>
    public struct SpatialOSEntity
    {
        public readonly EntityId SpatialOSEntityId;
        private readonly EntityManager entityManager;
        private readonly Entity entity;

        internal SpatialOSEntity(Entity entity, EntityManager entityManager)
        {
            this.entity = entity;
            this.entityManager = entityManager;
            SpatialOSEntityId = entityManager.GetComponentData<SpatialEntityId>(entity).EntityId;
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
