using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class GameObjectManager
    {
        private readonly Dictionary<Entity, GameObject> entityGameObjectMapping = new Dictionary<Entity, GameObject>();

        public void AddGameObjectEntity(Entity entity, GameObject gameObject)
        {
            entityGameObjectMapping[entity] = gameObject;
        }

        public void TryRemoveGameObjectEntity(Entity entity)
        {
            GameObject gameObject;
            if (!entityGameObjectMapping.TryGetValue(entity, out gameObject))
            {
                return;
            }

            entityGameObjectMapping.Remove(entity);
            Object.Destroy(gameObject);
        }
    }
}
