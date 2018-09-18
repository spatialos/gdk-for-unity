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

        public bool IsEntityOnThisWorker(EntityId entityId)
        {
            return Worker.EntityIdToEntity.ContainsKey(entityId);
        }

        public bool TryGetGameObjectForEntity(EntityId entityId, out GameObject linkedGameObject)
        {
            linkedGameObject = default(GameObject);
            if (!Worker.TryGetEntity(entityId, out var entity))
            {
                Worker.LogDispatcher.HandleLog(LogType.Warning, new LogEvent("Could not find ECS entity for given SpatialOS entity id")
                    .WithField("EntityId", entityId.Id));
                return false;
            }

            var entityManager = World.GetOrCreateManager<EntityManager>();
            if (!entityManager.HasComponent<GameObjectReference>(entity))
            {
                Worker.LogDispatcher.HandleLog(LogType.Warning, new LogEvent("Given entity is not linked to a game object")
                    .WithField("Entity", entity.Index));
                return false;
            }

            linkedGameObject = entityManager.GetComponentObject<GameObjectReference>(entity).GameObject;
            return true;
        }
    }
}
