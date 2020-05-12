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
        private const uint EntityAclComponentId = 50;
        private const uint PositionComponentId = 54;

        private readonly Dictionary<uint, ISpatialComponentSnapshot> entityData =
            new Dictionary<uint, ISpatialComponentSnapshot>();

        private readonly Acl acl = new Acl();

        /// <summary>
        ///     Constructs a worker access attribute, given a worker ID.
        /// </summary>
        /// <param name="workerId">An ID of a worker.</param>
        /// <returns>A string representing the worker access attribute.</returns>
        public static string GetWorkerAccessAttribute(string workerId)
        {
            return $"workerId:{workerId}";
        }

        /// <summary>
        ///     Adds a SpatialOS component to the EntityTemplate.
        /// </summary>
        /// <param name="snapshot">The component snapshot to add.</param>
        /// <typeparam name="TSnapshot">The type of the component snapshot.</typeparam>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the EntityTemplate already contains a component snapshot of type <see cref="TSnapshot" />.
        /// </exception>
        /// <remarks>
        ///     EntityACL is handled automatically by the EntityTemplate, so a EntityACL snapshot will be ignored.
        /// </remarks>
        public void AddComponent<TSnapshot>(TSnapshot snapshot)
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            AddComponent((ISpatialComponentSnapshot) snapshot);
        }

        /// <summary>
        ///     Adds a SpatialOS component to the EntityTemplate with write permissions specified.
        /// </summary>
        /// <param name="snapshot">The component snapshot to add.</param>
        /// <param name="writeAccess">
        ///     The worker attribute that should be granted write access over the <see cref="TSnapshot" /> component.
        /// </param>
        /// <typeparam name="TSnapshot">The type of the component snapshot.</typeparam>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the EntityTemplate already contains a component snapshot of type <see cref="TSnapshot" />.
        /// </exception>
        /// <remarks>
        ///     EntityACL is handled automatically by the EntityTemplate, so a EntityACL snapshot will be ignored.
        /// </remarks>
        public void AddComponent<TSnapshot>(TSnapshot snapshot, string writeAccess)
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            AddComponent((ISpatialComponentSnapshot) snapshot, writeAccess);
        }

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
            if (snapshot.ComponentId == EntityAclComponentId)
            {
                // ACL handled automatically.
                return;
            }

            if (entityData.ContainsKey(snapshot.ComponentId))
            {
                throw new InvalidOperationException(
                    "Cannot add multiple components of the same type to the same entity. " +
                    $"Attempted to add componentId: {snapshot.ComponentId} more than once.");
            }

            entityData.Add(snapshot.ComponentId, snapshot);
        }

        /// <summary>
        ///     Adds a SpatialOS component to the EntityTemplate with write permissions specified.
        /// </summary>
        /// <param name="snapshot">The component snapshot to add.</param>
        /// <param name="writeAccess">
        ///     The worker attribute that should be granted write access over the given component.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the EntityTemplate already contains a component snapshot with the same component ID.
        /// </exception>
        /// <remarks>
        ///     EntityACL is handled automatically by the EntityTemplate, so a EntityACL snapshot will be ignored.
        /// </remarks>
        public void AddComponent(ISpatialComponentSnapshot snapshot, string writeAccess)
        {
            AddComponent(snapshot);

            if (snapshot.ComponentId != EntityAclComponentId)
            {
                acl.SetComponentWriteAccess(snapshot.ComponentId, writeAccess);
            }
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
        /// <typeparam name="TSnapshot">The type of the component snapshot.</typeparam>
        /// <remarks>
        ///     This will override a snapshot of type <see cref="TSnapshot" /> in the EntityTemplate if one already exists.
        /// </remarks>
        public void SetComponent<TSnapshot>(TSnapshot snapshot) where TSnapshot : struct, ISpatialComponentSnapshot
        {
            entityData[snapshot.ComponentId] = snapshot;
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
            acl.RemoveComponentWriteAccess(id);
        }

        /// <summary>
        ///     Removes a component snapshot from the EntityTemplate, if it exists.
        /// </summary>
        /// <param name="componentId">The component that will be removed from the EntityTemplate.</param>
        public void RemoveComponent(uint componentId)
        {
            entityData.Remove(componentId);
            acl.RemoveComponentWriteAccess(componentId);
        }

        /// <summary>
        ///     Retrieves the write access worker attribute for a given component.
        /// </summary>
        /// <param name="componentId">The component id for that component.</param>
        /// <returns>The write access worker attribute, if it exists, null otherwise.</returns>
        public string GetComponentWriteAccess(uint componentId)
        {
            return acl.GetComponentStringAccess(componentId);
        }

        /// <summary>
        ///     Retrieves the write access worker attribute for a given component.
        /// </summary>
        /// <typeparam name="TSnapshot">The type of the component.</typeparam>
        /// <returns>The write access worker attribute, if it exists, null otherwise.</returns>
        public string GetComponentWriteAccess<TSnapshot>() where TSnapshot : struct, ISpatialComponentSnapshot
        {
            return GetComponentWriteAccess(ComponentDatabase.GetSnapshotComponentId<TSnapshot>());
        }

        /// <summary>
        ///     Sets the write access worker attribute for a given component.
        /// </summary>
        /// <param name="componentId">The component id for that component.</param>
        /// <param name="writeAccess">The write access worker attribute.</param>
        public void SetComponentWriteAccess(uint componentId, string writeAccess)
        {
            acl.SetComponentWriteAccess(componentId, writeAccess);
        }

        /// <summary>
        ///     Sets the write access worker attribute for a given component.
        /// </summary>
        /// <param name="writeAccess">The write access worker attribute.</param>
        /// <typeparam name="TSnapshot">The type of the component.</typeparam>
        public void SetComponentWriteAccess<TSnapshot>(string writeAccess)
            where TSnapshot : struct, ISpatialComponentSnapshot
        {
            SetComponentWriteAccess(ComponentDatabase.GetSnapshotComponentId<TSnapshot>(), writeAccess);
        }

        /// <summary>
        ///     Sets the worker attributes which should have read access over this entity.
        /// </summary>
        /// <param name="attributes">The worker attributes which should have read access.</param>
        public void SetReadAccess(params string[] attributes)
        {
            foreach (var attr in attributes)
            {
                acl.AddReadAccess(attr);
            }
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
            entity.Add(acl.Build());
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
            if (!entityData.ContainsKey(PositionComponentId))
            {
                throw new InvalidOperationException("Entity is invalid. No Position component was found");
            }
        }

        private class Acl
        {
            private readonly Dictionary<uint, string> writePermissions = new Dictionary<uint, string>();
            private readonly List<string> readPermissions = new List<string>();

            public string GetComponentStringAccess(uint componentId)
            {
                writePermissions.TryGetValue(componentId, out var writeAccess);
                return writeAccess;
            }

            public void SetComponentWriteAccess(uint componentId, string attribute)
            {
                writePermissions[componentId] = attribute;
            }

            public void AddReadAccess(string attribute)
            {
                readPermissions.Add(attribute);
            }

            public void RemoveComponentWriteAccess(uint componentId)
            {
                writePermissions.Remove(componentId);
            }

            public ComponentData Build()
            {
                var schemaComponentData = SchemaComponentData.Create();
                var fields = schemaComponentData.GetFields();

                // Write the read acl
                var workerRequirementSet = fields.AddObject(1);
                foreach (var attr in readPermissions)
                {
                    // Add another WorkerAttributeSet to the list
                    var set = workerRequirementSet.AddObject(1);
                    set.AddString(1, attr);
                }

                // Write the component write acl
                foreach (var writePermission in writePermissions)
                {
                    var keyValuePair = fields.AddObject(2);
                    keyValuePair.AddUint32(1, writePermission.Key);
                    var containedRequirementSet = keyValuePair.AddObject(2);
                    var containedAttributeSet = containedRequirementSet.AddObject(1);
                    containedAttributeSet.AddString(1, writePermission.Value);
                }

                return new ComponentData(EntityAclComponentId, schemaComponentData);
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
