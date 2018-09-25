using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation
{
    public class SpatialOSComponent : MonoBehaviour
    {
        public EntityId SpatialEntityId;
        public Entity Entity;
        public World World;
        public WorkerSystem Worker;

        private EntityManager entityManager;

        public bool IsEntityOnThisWorker(EntityId entityId)
        {
            return Worker.EntityIdToEntity.ContainsKey(entityId);
        }

        public bool TryGetGameObjectForSpatialOSEntityId(EntityId entityId, out GameObject linkedGameObject)
        {
            linkedGameObject = default(GameObject);
            if (!Worker.TryGetEntity(entityId, out var entity))
            {
                return false;
            }

            entityManager = entityManager ?? World.GetOrCreateManager<EntityManager>();
            if (!entityManager.HasComponent<GameObjectReference>(entity))
            {
                return false;
            }

            var retrievedGameObject = entityManager.GetComponentObject<GameObjectReference>(entity).GameObject;
            var component = retrievedGameObject.GetComponent<SpatialOSComponent>();
            if (component == null || component.SpatialEntityId != entityId)
            {
                return false;
            }

            linkedGameObject = retrievedGameObject;
            return true;
        }

        public bool TryGetSpatialOSEntityIdForGameObject(GameObject linkedGameObject, out EntityId entityId)
        {
            entityId = default(EntityId);
            var component = linkedGameObject.GetComponent<SpatialOSComponent>();
            if (component == null || Worker != component.Worker)
            {
                return false;
            }

            entityManager = entityManager ?? World.GetOrCreateManager<EntityManager>();
            if (!entityManager.HasComponent<GameObjectReference>(component.Entity))
            {
                return false;
            }

            var retrievedGameObject = entityManager.GetComponentObject<GameObjectReference>(component.Entity).GameObject;
            if (linkedGameObject != retrievedGameObject)
            {
                return false;
            }

            entityId = component.SpatialEntityId;
            return true;

        }
    }
}
