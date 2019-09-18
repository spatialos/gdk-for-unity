using System.Collections.Generic;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     A snapshot of a SpatialOS entity.
    /// </summary>
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

            var componentDataHandler = new QueryComponentDataHandler(entitySnapshot);

            foreach (var componentId in entitySnapshot.GetComponentIds())
            {
                Dynamic.ForComponent(componentId, componentDataHandler);
            }

            components = componentDataHandler.Components;
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
            var handler = new SchemaEntitySerializer(inObj, components);

            foreach (var pair in components)
            {
                Dynamic.ForComponent(pair.Key, handler);
            }
        }
    }

    internal struct QueryComponentDataHandler : Dynamic.IHandler
    {
        public Dictionary<uint, ISpatialComponentSnapshot> Components;

        private readonly Entity entitySnapshot;

        public QueryComponentDataHandler(Entity entitySnapshot)
        {
            Components = new Dictionary<uint, ISpatialComponentSnapshot>();
            this.entitySnapshot = entitySnapshot;
        }

        public void Accept<TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TUpdate, TSnapshot> vtable)
            where TUpdate : struct, ISpatialComponentUpdate
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            var schemaObject = entitySnapshot.Get(componentId).Value;
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

        public void Accept<TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TUpdate, TSnapshot> vtable)
            where TUpdate : struct, ISpatialComponentUpdate
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            var componentObject = entityObject.GetObject(componentId);
            Components[componentId] = vtable.DeserializeSnapshotRaw(componentObject);
        }
    }

    internal struct SchemaEntitySerializer : Dynamic.IHandler
    {
        private Dictionary<uint, ISpatialComponentSnapshot> componentSnapshots;
        private SchemaObject targetObject;

        public SchemaEntitySerializer(SchemaObject targetObject, Dictionary<uint, ISpatialComponentSnapshot> componentSnapshots)
        {
            this.targetObject = targetObject;
            this.componentSnapshots = componentSnapshots;
        }

        public void Accept<TUpdate, TSnapshot>(uint componentId, Dynamic.VTable<TUpdate, TSnapshot> vtable)
            where TUpdate : struct, ISpatialComponentUpdate
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            // Okay to grab the value directly, we only call this for snapshots that actually exist.
            var data = componentSnapshots[componentId];
            vtable.SerializeSnapshotRaw((TSnapshot) data, targetObject.AddObject(componentId));
        }
    }
}
