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
        private readonly Dictionary<EntityId, Entity> entities = new Dictionary<EntityId, Entity>();

        public int Count => entities.Count;

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
            var entityId = new EntityId(entities.Count + 1);
            entities[entityId] = entityTemplate.GetEntity();
            return entityId;
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
                    catch (System.IO.InvalidDataException e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
        }
    }
}
