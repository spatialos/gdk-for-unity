using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;

namespace Improbable.Gdk.GameObjectCreation
{
    /// <summary>
    ///     Interface for listening for SpatialOS Entity creation to be used for binding GameObjects.
    ///     Implementing classes can be passed to GameObjectCreationSystemHelper in order to be called.
    /// </summary>
    public interface IEntityGameObjectCreator
    {
        /// <summary>
        ///     Called to register the components expected on an entity to create a GameObject for a given entity type.
        /// </summary>
        void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations);

        /// <summary>
        ///     Called when a new SpatialOS Entity is checked out by the worker.
        /// </summary>
        /// <returns>
        ///     A GameObject to be linked to the entity, or null if no GameObject should be linked.
        /// </returns>
        void OnEntityCreated(string entityType, SpatialOSEntity entity, EntityGameObjectLinker linker);

        /// <summary>
        ///     Called when a SpatialOS Entity is removed from the worker's view.
        /// </summary>
        void OnEntityRemoved(EntityId entityId);
    }
}
