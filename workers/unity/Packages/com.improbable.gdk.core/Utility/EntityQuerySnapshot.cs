using System.Collections.Generic;
using Unity.Entities;
using Entity = Improbable.Worker.CInterop.Entity;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A  snapshot of a SpatialOS entity, containing the result of a entity query.
    /// </summary>
    /// <remarks>
    ///     This copies entity components from <see cref="Improbable.Worker.CInterop.Entity" /> for long term storage.
    ///     This may only be a partial snapshot of an entity.
    ///     The components present depend on the component filter used when making the entity query.
    /// </remarks>
    public struct EntityQuerySnapshot
    {
        private struct ComponentDataHandler : DynamicSnapshot.ISnapshotHandler
        {
            public Entity EntitySnapshot;
            public Dictionary<uint, ISpatialComponentSnapshot> Components;

            public void Accept<T>(uint componentId, DynamicSnapshot.SnapshotDeserializer<T> deserializeSnapshot,
                DynamicSnapshot.SnapshotSerializer<T> serializeSnapshot) where T : struct, ISpatialComponentSnapshot
            {
                var schemaObject = EntitySnapshot.Get(componentId).Value;
                Components.Add(componentId, deserializeSnapshot(schemaObject));
            }
        }

        private readonly Dictionary<uint, ISpatialComponentSnapshot> components;

        internal EntityQuerySnapshot(Entity entitySnapshot)
        {
            components = new Dictionary<uint, ISpatialComponentSnapshot>();

            var componentDataHandler = new ComponentDataHandler
            {
                EntitySnapshot = entitySnapshot,
                Components = components,
            };

            foreach (var componentId in entitySnapshot.GetComponentIds())
            {
                DynamicSnapshot.ForSnapshotComponent(componentId, componentDataHandler);
            }
        }

        /// <summary>
        ///     Get the SpatialOS component snapshot if present.
        /// </summary>
        /// <returns> The component snapshot, if it exists, or null otherwise.</returns>
        /// <typeparam name="T">The component type.</typeparam>
        public T? GetComponentSnapshot<T>() where T : struct, ISpatialComponentSnapshot
        {
            var id = DynamicSnapshot.GetSnapshotComponentId<T>();
            if (components.TryGetValue(id, out var data))
            {
                return (T) data;
            }

            return null;
        }
    }
}
