using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Denotes that a GameObject has been linked to an SpatialOS entity and exposes SpatialOS functionality.
    /// </summary>
    public class SpatialOSComponent : MonoBehaviour
    {
        /// <summary>
        ///     The entity ID of the linked SpatialOS entity.
        /// </summary>
        public EntityId SpatialEntityId;

        /// <summary>
        ///     The ECS entity that represents the SpatialOS entity that this GameObject is linked to.
        /// </summary>
        public Entity Entity;

        /// <summary>
        ///     A reference to the ECS world that the SpatialOS entity exists in.
        /// </summary>
        public World World;

        /// <summary>
        ///     A reference to the Worker that the SpatialOS entity exists in.
        /// </summary>
        public WorkerSystem Worker;

        private EntityManager entityManager;

        /// <summary>
        ///     Checks whether a SpatialOS entity is in this worker's view.
        /// </summary>
        /// <param name="entityId">The entity ID to check.</param>
        /// <returns>True if the entity is in this worker's view, false otherwise.</returns>
        public bool IsEntityOnThisWorker(EntityId entityId)
        {
            return Worker.EntityIdToEntity.ContainsKey(entityId);
        }

        /// <summary>
        ///     Attempts to get a GameObject linked to a SpatialOS entity.
        /// </summary>
        /// <param name="entityId">The entity ID of the SpatialOS entity.</param>
        /// <param name="linkedGameObject">
        ///     When this method returns, this will be a reference to the linked GameObject if it is found or null
        ///     otherwise.
        /// </param>
        /// <returns>True, if a linked GameObject was found, false otherwise.</returns>
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

        /// <summary>
        ///     Attempts to get an entity ID for a GameObject linked to a SpatialOS entity.
        /// </summary>
        /// <param name="linkedGameObject">The GameObject to get an entity ID from.</param>
        /// <param name="entityId">
        ///     When this method returns, contains a valid entity ID for the GameObject if it is linked and is
        ///     valid or default constructed otherwise.
        /// </param>
        /// <returns>True, if the GameObject is linked to a SpatialOS entity and is valid, false otherwise.</returns>
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
