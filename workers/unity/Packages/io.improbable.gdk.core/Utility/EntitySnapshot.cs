using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A snapshot of a SpatialOS entity.
    /// </summary>
    /// <remarks>
    ///     This copies entity components from <see cref="Improbable.Worker.CInterop.Entity" /> for long term storage.
    ///     This may only be a partial snapshot of an entity.
    ///     The components present depend on the component filter used when making the entity query.
    /// </remarks>
    public struct EntitySnapshot
    {
        private readonly Dictionary<uint, ISpatialComponentSnapshot> components;

        /// <summary>
        ///     Gets the SpatialOS component snapshot if present.
        /// </summary>
        /// <returns> The component snapshot, if it exists, or null otherwise.</returns>
        /// <typeparam name="T">The component type.</typeparam>
        public T? GetComponentSnapshot<T>() where T : struct, ISpatialComponentSnapshot
        {
            var id = Dynamic.GetSnapshotComponentId<T>();
            if (components.TryGetValue(id, out var data))
            {
                return (T) data;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to get the SpatialOS component if present.
        /// </summary>
        /// <param name="snapshot">
        ///     When this method returns, this will be the component if it exists, default constructed otherwise.
        /// </param>
        /// <typeparam name="T">The component type.</typeparam>
        /// <returns>True, if the component exists; false otherwise.</returns>
        public bool TryGetComponentSnapshot<T>(out T snapshot) where T : struct, ISpatialComponentSnapshot
        {
            var maybeSnapshot = GetComponentSnapshot<T>();
            snapshot = maybeSnapshot ?? default(T);
            return maybeSnapshot.HasValue;
        }

        /// <summary>
        ///     Adds a component to this snapshot.
        /// </summary>
        /// <remarks>
        ///     Will override any pre-existing component in the snapshot.
        /// </remarks>
        /// <param name="component">The component to add.</param>
        /// <typeparam name="T">The component type.</typeparam>
        public void AddComponentSnapshot<T>(T component) where T : struct, ISpatialComponentSnapshot
        {
            components[component.ComponentId] = component;
        }

        internal EntitySnapshot(Entity entitySnapshot)
        {
            components = new Dictionary<uint, ISpatialComponentSnapshot>();

            var componentDataHandler = new QueryComponentDataHandler
            {
                EntitySnapshot = entitySnapshot,
                Components = components
            };

            foreach (var componentId in entitySnapshot.GetComponentIds())
            {
                Dynamic.ForComponent(componentId, componentDataHandler);
            }
        }

        internal EntitySnapshot(SchemaObject entityObject)
        {
            var handler = new SchemaEntityDeserializer(entityObject);
            var ids = entityObject.GetUniqueFieldIds();

            foreach (var id in ids)
            {
                Dynamic.ForComponent(id, handler);
            }

            components = handler.Components;
        }

        internal void SerializeToSchemaObject(SchemaObject inObj)
        {
            var handler = new SchemaEntitySerializer(this, inObj);

            foreach (var componentId in components.Keys)
            {
                Dynamic.ForComponent(componentId, handler);
            }
        }
    }

    internal struct QueryComponentDataHandler : Dynamic.IHandler
    {
        public Entity EntitySnapshot;
        public Dictionary<uint, ISpatialComponentSnapshot> Components;

        public void Accept<TData, TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TData, TUpdate, TSnapshot> vtable)
            where TData : struct, ISpatialComponentData
            where TUpdate : struct, ISpatialComponentUpdate
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            var schemaObject = EntitySnapshot.Get(componentId).Value;
            Components.Add(componentId, vtable.DeserializeSnapshot(schemaObject));
        }
    }

    internal struct SchemaEntityDeserializer : Dynamic.IHandler
    {
        public readonly Dictionary<uint, ISpatialComponentSnapshot> Components;

        private SchemaObject entityObject;

        public SchemaEntityDeserializer(SchemaObject entityObject)
        {
            Components = new Dictionary<uint, ISpatialComponentSnapshot>();
            this.entityObject = entityObject;
        }

        public void Accept<TData, TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TData, TUpdate, TSnapshot> vtable)
            where TData : struct, ISpatialComponentData
            where TUpdate : struct, ISpatialComponentUpdate
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            var componentObject = entityObject.GetObject(componentId);
            Components[componentId] = vtable.DeserializeSnapshotRaw(componentObject);
        }
    }

    internal struct SchemaEntitySerializer : Dynamic.IHandler
    {
        private EntitySnapshot entitySnapshot;
        private SchemaObject targetObject;

        public SchemaEntitySerializer(EntitySnapshot entitySnapshot, SchemaObject targetObject)
        {
            this.entitySnapshot = entitySnapshot;
            this.targetObject = targetObject;
        }

        public void Accept<TData, TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TData, TUpdate, TSnapshot> vtable)
            where TData : struct, ISpatialComponentData
            where TUpdate : struct, ISpatialComponentUpdate
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            // Okay to grab the value directly, we only call this for snapshots that actually exist.
            var data = entitySnapshot.GetComponentSnapshot<TSnapshot>().Value;
            vtable.SerializeSnapshotRaw(data, targetObject.AddObject(componentId));
        }
    }
}
