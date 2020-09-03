using System.Collections.Generic;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Convenience wrapper around the WorkerSDK Snapshot API.
    /// </summary>
    public class Snapshot
    {
        private const int PersistenceComponentId = 55;

        private readonly Dictionary<EntityId, Entity> entities = new Dictionary<EntityId, Entity>();

        public int Count => entities.Count;

        private long nextEntityId = 1;

        /// <summary>
        ///     Returns the next available entity ID.
        /// </summary>
        /// <returns>The next available entity ID.</returns>
        public EntityId GetNextEntityId()
        {
            return new EntityId(nextEntityId++);
        }

        /// <summary>
        ///     Adds an entity to the snapshot
        /// </summary>
        /// <remarks>
        ///    The entity ID is automatically assigned.
        /// </remarks>
        /// <param name="entityTemplate">The entity to be added to the snapshot.</param>
        /// <returns>The entity ID assigned to the entity in the snapshot.</returns>
        public EntityId AddEntity(EntityTemplate entityTemplate)
        {
            var entityId = GetNextEntityId();
            AddEntity(entityId, entityTemplate);
            return entityId;
        }

        /// <summary>
        ///     Adds an entity to the snapshot
        /// </summary>
        /// <param name="entityId">The entity ID of the entity to be added to the snapshot</param>
        /// <param name="entityTemplate">The entity to be added to the snapshot.</param>
        /// <remarks>
        ///    You should obtain `entityId` using the `GetNextEntityId()` method, otherwise you could be given
        ///    invalid entity IDs.
        /// </remarks>
        public void AddEntity(EntityId entityId, EntityTemplate entityTemplate)
        {
            var entity = entityTemplate.GetEntity();
            // This is a no-op if the entity already has persistence.
            entity.Add(new ComponentData(PersistenceComponentId, SchemaComponentData.Create()));
            entities[entityId] = entity;
        }

        /// <summary>
        ///     Writes the snapshot out to a file.
        /// </summary>
        /// <param name="path">The file path.</param>
        public void WriteToFile(string path)
        {
            var parameters = new SnapshotParameters
            {
                DefaultComponentVtable = new ComponentVtable()
            };

            using (var outputStream = new SnapshotOutputStream(path, parameters))
            {
                foreach (var entry in entities)
                {
                    try
                    {
                        outputStream.WriteEntity(entry.Key.Id, entry.Value);
                    }
                    catch (StreamBadStateException e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
        }

        internal Entity this[EntityId entityId] => entities[entityId];
    }
}
