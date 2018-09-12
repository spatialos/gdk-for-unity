using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     Interface for listening for SpatialOS Entity check-out and check-in events for defining custom companion GameObjects creation/deletion logic.
    ///     Implementing classes can be passed to GameObjectCreationSystemHelper in order to be called.
    /// </summary>
    public interface IEntityGameObjectCreator
    {
        /// <summary>
        ///     Called when a new SpatialOS Entity is checked out by the worker.
        ///     Implement this method to define custom logic for creating a GameObject to represent an entity.
        /// </summary>
        /// <param name="gameObject">
        ///     The GameObject created to represent the entity.
        /// </param>
        /// <returns>
        ///     true, if a GameObject should be created to represent the entity and if said GameObject was successfully created.
        /// </returns>
        bool OnEntityAdded(SpatialOSEntity entity, out GameObject gameObject);

        /// <summary>
        ///     Called when a SpatialOS Entity is removed from the worker.
        ///     Implement this method to define custom logic for removing the GameObject representing an entity.
        /// </summary>
        /// <param name="linkedGameObject">
        ///     The GameObject linked to the entity, or null if no GameObject was linked.
        /// </param>
        void OnEntityRemoved(EntityId entityId, GameObject linkedGameObject);
    }
}
