using System;
using System.Collections.Generic;
using Improbable.Worker;
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

        public GameObject CreateEntityGameObject(Entity entity, string prefabName, string workerType, Vector3 position,
            Quaternion rotation, EntityId spatialEntityId)
        {
            string workerSpecificPath = "Prefabs\\" + workerType + "\\" + prefabName;
            string commonPath = "Prefabs\\Common\\" + prefabName;
            if (!cachedPrefabs.TryGetValue(workerSpecificPath, out var prefab)
                && !cachedPrefabs.TryGetValue(commonPath, out prefab))
            {
                prefab = Resources.Load<GameObject>(workerSpecificPath);
                if (prefab == null)
                {
                    prefab = Resources.Load<GameObject>(commonPath);
                }

                if (prefab == null)
                {
                    throw new PrefabNotFoundException($"Prefab for prefabPaths {workerSpecificPath} or {commonPath} not found.");
                }

                cachedPrefabs[prefabName] = prefab;
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
