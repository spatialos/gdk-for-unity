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

            linkedGameObject = entityManager.GetComponentObject<GameObjectReference>(entity).GameObject;
            return true;
        }
    }
}
