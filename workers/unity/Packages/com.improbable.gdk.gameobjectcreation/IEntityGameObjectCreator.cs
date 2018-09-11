using Improbable.Gdk.Core;
using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     Interface for listening for SpatialOS Entity creation to be used for binding GameObjects.
    ///     Implementing classes can be passed to GameObjectCreationSystemHelper in order to be called.
    /// </summary>
    public interface IEntityGameObjectCreator
    {
        /// <summary>
        ///     Called when a new SpatialOS Entity is checked out by the worker.
        /// </summary>
        /// <returns>
        ///     A GameObject to be linked to the entity, or null if no GameObject should be linked.
        /// </returns>
        GameObject OnEntityCreated(SpatialOSEntity entity);

        /// <summary>
        ///     Called when a SpatialOS Entity is removed from the worker's view.
        /// </summary>
        /// <param name="linkedGameObject">
        ///     The GameObject linked to the entity, or null if no GameObject is linked.
        /// </param>
        void OnEntityRemoved(EntityId entityId, GameObject linkedGameObject);
    }
}
