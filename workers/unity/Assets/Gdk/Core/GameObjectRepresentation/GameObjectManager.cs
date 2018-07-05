using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class GameObjectManager
    {
        private readonly Dictionary<Entity, GameObject> entityGameObjectMapping = new Dictionary<Entity, GameObject>();

        public bool HasGameObjectEntity(Entity entity)
        {
            return entityGameObjectMapping.ContainsKey(entity);
        }

        public void AddGameObjectEntity(Entity entity, GameObject gameObject)
        {
            entityGameObjectMapping[entity] = gameObject;
        }

        public void RemoveGameObjectEntity(Entity entity)
        {
            GameObject gameObject;
            if (!entityGameObjectMapping.TryGetValue(entity, out gameObject))
            {
                Debug.LogErrorFormat(Errors.EntityNotFound, entity.Index);
                return;
            }

            entityGameObjectMapping.Remove(entity);
            Object.Destroy(gameObject);
        }

        internal static class Errors
        {
            public const string EntityNotFound = "Entity {0} not found.";
        }
    }
}
