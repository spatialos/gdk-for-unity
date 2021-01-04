using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Utility class to help build SpatialOS entities. An <see cref="EntityTemplate" /> can be mutated be used
    ///     multiple times.
    /// </summary>
    public class EntityTemplate
    {
        private const uint PositionComponentId = 54;

        private readonly Dictionary<uint, ISpatialComponentSnapshot> entityData =
            new Dictionary<uint, ISpatialComponentSnapshot>();

        /// <summary>
        ///     Adds a SpatialOS component to the Entity Template.
        /// </summary>
        /// <param name="snapshot">The component snapshot to add.</param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the EntityTemplate already contains a component snapshot with the same component ID.
        /// </exception>
        /// <remarks>
        ///     EntityACL is handled automatically by the EntityTemplate, so a EntityACL snapshot will be ignored.
        /// </remarks>
        public void AddComponent(ISpatialComponentSnapshot snapshot)
        {
            if (entityData.ContainsKey(snapshot.ComponentId))
            {
                throw new InvalidOperationException(
                    "Cannot add multiple components of the same type to the same entity. " +
                    $"Attempted to add componentId: {snapshot.ComponentId} more than once.");
            }

            entityData.Add(snapshot.ComponentId, snapshot);
        }

        /// <summary>
        ///     Attempts to get a component snapshot stored in the EntityTemplate.
        /// </summary>
        /// <typeparam name="TSnapshot">The type of the component snapshot.</typeparam>
        /// <returns>The component snapshot, if the component snapshot exists, null otherwise.</returns>
        public TSnapshot? GetComponent<TSnapshot>() where TSnapshot : struct, ISpatialComponentSnapshot
        {
            if (entityData.TryGetValue(ComponentDatabase.GetSnapshotComponentId<TSnapshot>(), out var snapshot))
            {
                return (TSnapshot) snapshot;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to get a component snapshot stored in the EntityTemplate.
        /// </summary>
        /// <param name="componentId">The ID of the component to fetch.</param>
        /// <returns>The component snapshot, if the component snapshot exists, null otherwise.</returns>
        public ISpatialComponentSnapshot GetComponent(uint componentId)
        {
            entityData.TryGetValue(componentId, out var snapshot);
            return snapshot;
        }

        /// <summary>Gets the component of the associated type.</summary>
        /// <param name="component">
        ///     When this method returns, contains the component, if the component is found; otherwise, the default value <see cref="TSnapshot"/>. This parameter is passed uninitialized.
        /// </param>
        /// <typeparam name="TSnapshot">The type of the component snapshot.</typeparam>
        /// <returns>
        ///     True if this contains a component of type <see cref="TSnapshot"/>; otherwise, false.
        /// </returns>
        public bool TryGetComponent<TSnapshot>(out TSnapshot component)
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            if (entityData.TryGetValue(ComponentDatabase.GetSnapshotComponentId<TSnapshot>(), out var boxedComponent))
            {
                component = (TSnapshot) boxedComponent;
                return true;
            }

            component = default;
            return false;
        }

        /// <summary>Gets the component with the associated component ID.</summary>
        /// <param name="componentId">The ID of the component to get.</param>
        /// <param name="component">
        ///     When this method returns, contains the component, if the component is found; otherwise null. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     True if this contains a component with the associated component ID; otherwise, false.
        /// </returns>
        public bool TryGetComponent(uint componentId, out ISpatialComponentSnapshot component)
        {
            return entityData.TryGetValue(componentId, out component);
        }

        /// <summary>
        ///     Checks if a component snapshot is stored in the EntityTemplate.
        /// </summary>
        /// <param name="componentId">The component id to check.</param>
        /// <returns>True, if the component snapshot exists, false otherwise.</returns>
        public bool HasComponent(uint componentId)
        {
            return entityData.ContainsKey(componentId);
        }

        /// <summary>
        ///     Checks if a component snapshot is stored in the EntityTemplate.
        /// </summary>
        /// <typeparam name="TSnapshot">The type of the component snapshot.</typeparam>
        /// <returns>True, if the component snapshot exists, false otherwise.</returns>
        public bool HasComponent<TSnapshot>() where TSnapshot : struct, ISpatialComponentSnapshot
        {
            return HasComponent(ComponentDatabase.GetSnapshotComponentId<TSnapshot>());
        }


        /// <summary>
        ///     Sets a component snapshot in the EntityTemplate.
        /// </summary>
        /// <param name="snapshot">The component snapshot that will be inserted into the EntityTemplate.</param>
        /// <remarks>
        ///     This will override the component snapshot in the EntityTemplate if one already exists.
        /// </remarks>
        public void SetComponent(ISpatialComponentSnapshot snapshot)
        {
            entityData[snapshot.ComponentId] = snapshot;
        }

        /// <summary>
        ///     Removes a component snapshot from the EntityTemplate, if it exists.
        /// </summary>
        /// <typeparam name="TSnapshot">The type of the component snapshot.</typeparam>
        public void RemoveComponent<TSnapshot>() where TSnapshot : struct, ISpatialComponentSnapshot
        {
            var id = ComponentDatabase.GetSnapshotComponentId<TSnapshot>();
            entityData.Remove(id);
        }

        /// <summary>
        ///     Removes a component snapshot from the EntityTemplate, if it exists.
        /// </summary>
        /// <param name="componentId">The component that will be removed from the EntityTemplate.</param>
        public void RemoveComponent(uint componentId)
        {
            entityData.Remove(componentId);
        }

        /// <summary>
        ///     Creates an <see cref="Entity" /> instance from this template.
        /// </summary>
        /// <remarks>
        ///     This function allocates native memory. The <see cref="Entity" /> returned from this function should
        ///     be handed to a GDK API, which will take ownership, or otherwise must be disposed of manually.
        /// </remarks>
        /// <returns>The Entity object.</returns>
        public Entity GetEntity()
        {
            ValidateEntity();
            var handler = new EntityTemplateDynamicHandler(entityData);
            Dynamic.ForEachComponent(handler);
            var entity = handler.Entity;
            return entity;
        }

        /// <summary>
        ///     Creates an <see cref="EntitySnapshot"/> from this template.
        /// </summary>
        /// <returns>The EntitySnapshot object.</returns>
        public EntitySnapshot GetEntitySnapshot()
        {
            var entity = GetEntity();
            var snapshot = new EntitySnapshot(entity);

            foreach (var id in entity.GetComponentIds())
            {
                entity.Get(id).Value.SchemaData.Value.Destroy();
            }

            return snapshot;
        }

        private void ValidateEntity()
        {
            // TODO: Ensure this has AuthorityDelegation component on it.
            if (!entityData.ContainsKey(PositionComponentId))
            {
                throw new InvalidOperationException("Entity is invalid. No Position component was found");
            }
        }

        private class EntityTemplateDynamicHandler : Dynamic.IHandler
        {
            public Entity Entity;
            private readonly Dictionary<uint, ISpatialComponentSnapshot> data;

            public EntityTemplateDynamicHandler(Dictionary<uint, ISpatialComponentSnapshot> data)
            {
                this.data = data;
                Entity = new Entity();
            }

            public void Accept<TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TUpdate, TSnapshot> vtable)
                where TUpdate : struct, ISpatialComponentUpdate
                where TSnapshot : struct, ISpatialComponentSnapshot
            {
                if (!data.ContainsKey(componentId))
                {
                    return;
                }

                var componentData = new ComponentData(componentId, SchemaComponentData.Create());
                vtable.SerializeSnapshot((TSnapshot) data[componentId], componentData);
                Entity.Add(componentData);
            }
        }
    }
}
