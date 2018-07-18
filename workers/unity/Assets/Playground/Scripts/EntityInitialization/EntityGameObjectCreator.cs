using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Playground
{
    public class EntityGameObjectCreator
    {
        private readonly World world;
        private readonly Dictionary<string, GameObject> cachedPrefabs;

        public EntityGameObjectCreator(World world, Dictionary<string, GameObject> initialCachedPrefabs = null)
        {
            this.world = world;
            cachedPrefabs = initialCachedPrefabs ?? new Dictionary<string, GameObject>();
        }

        public GameObject CreateEntityGameObject(Entity entity, string prefabPath, Vector3 position,
            Quaternion rotation, long spatialEntityId)
        {
            GameObject prefab;
            if (!cachedPrefabs.TryGetValue(prefabPath, out prefab))
            {
                prefab = Resources.Load<GameObject>(prefabPath);
                if (prefab == null)
                {
                    throw new PrefabNotFoundException($"Prefab for prefabPath {prefabPath} not found.");
                }

                cachedPrefabs[prefabPath] = prefab;
            }

            var gameObject = GameObject.Instantiate(prefab, position, rotation);
            gameObject.name = $"{prefab.name}(SpatialOS: {spatialEntityId}, Unity: {entity.Index}/{world.Name})";
            return gameObject;
        }
    }

    public class PrefabNotFoundException : Exception
    {
        public PrefabNotFoundException(string message) : base(message)
        {
        }
    }
}
