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

        public EntityGameObjectCreator(World world, Dictionary<string, GameObject> initialCachedPrefabs)
        {
            this.world = world;
            cachedPrefabs = initialCachedPrefabs;
        }

        public GameObject CreateEntityGameObject(Entity entity, string prefabName, Vector3 position,
            Quaternion rotation, long spatialEntityId)
        {
            GameObject prefab;
            if (!cachedPrefabs.TryGetValue(prefabName, out prefab))
            {
                prefab = Resources.Load<GameObject>(prefabName);
                if (prefab == null)
                {
                    throw new PrefabNotFoundException(string.Format(Errors.PrefabNotFound, prefabName));
                }

                cachedPrefabs[prefabName] = prefab;
            }

            var gameObject = GameObject.Instantiate(prefab, position, rotation);
            gameObject.name = $"{prefab.name}(SpatialOS: {spatialEntityId}, Unity: {entity.Index}/{world.Name})";
            return gameObject;
        }

        internal static class Errors
        {
            public const string PrefabNotFound = "Prefab for prefabPath {0} not found.";
        }

        internal class PrefabNotFoundException : Exception
        {
            public PrefabNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
