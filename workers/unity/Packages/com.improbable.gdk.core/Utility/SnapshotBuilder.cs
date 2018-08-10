using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class SnapshotBuilder
    {
        private readonly Dictionary<EntityId, Entity> entities = new Dictionary<EntityId, Entity>();

        public int Count => entities.Count;

        public void AddEntity(Entity entity)
        {
            entities[new EntityId(entities.Count + 1)] = entity;
        }

        public void Serialize(string path)
        {
            var parameters = new SnapshotParameters
            {
                DefaultComponentVtable = new PassthroughComponentVtable()
            };

            using (var outputStream = new SnapshotOutputStream(path, parameters))
            {
                foreach (var entry in entities)
                {
                    var error = outputStream.WriteEntity(entry.Key, entry.Value);
                    if (error.HasValue)
                    {
                        Debug.LogError(error.Value);
                    }
                }
            }
        }
    }
}
