using System.Collections.Generic;
using System.IO;
using Generated.Improbable;
using Generated.Improbable.Transform;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using Transform = Generated.Improbable.Transform.Transform;

namespace Playground
{
    public class EntityGameObjectCreator : IEntityGameObjectCreator
    {
        private readonly Dictionary<string, Dictionary<string, GameObject>> cachedPrefabsPerWorkerType
            = new Dictionary<string, Dictionary<string, GameObject>>();

        public GameObject CreateGameObjectForEntity(SpatialOSEntity entity, WorkerSystem worker)
        {
            if (!cachedPrefabsPerWorkerType.TryGetValue(worker.WorkerType, out var cachedPrefabs))
            {
                cachedPrefabsPerWorkerType[worker.WorkerType] = cachedPrefabs = new Dictionary<string, GameObject>();
            }

            if (!entity.HasComponent<Metadata.Component>())
            {
                return null;
            }

            var prefabName = entity.GetComponent<Metadata.Component>().EntityType;
            if (!entity.HasComponent<Transform.Component>())
            {
                return cachedPrefabs[prefabName] = null;
            }

            var transform = entity.GetComponent<Transform.Component>();
            var position = new Vector3(transform.Location.X, transform.Location.Y, transform.Location.Z) +
                worker.Origin;
            var rotation = new UnityEngine.Quaternion(transform.Rotation.X, transform.Rotation.Y,
                transform.Rotation.Z, transform.Rotation.W);
            var workerSpecificPath = Path.Combine("Prefabs", worker.WorkerType, prefabName);
            var commonPath = Path.Combine("Prefabs", "Common", prefabName);

            if (!cachedPrefabs.TryGetValue(workerSpecificPath, out var prefab)
                && !cachedPrefabs.TryGetValue(commonPath, out prefab))
            {
                prefab = Resources.Load<GameObject>(workerSpecificPath);
                if (prefab == null)
                {
                    prefab = Resources.Load<GameObject>(commonPath);
                }

                // Cache even if null
                cachedPrefabs[prefabName] = prefab;
            }

            if (prefab == null)
            {
                return null;
            }

            var gameObject = GameObject.Instantiate(prefab, position, rotation);
            gameObject.name = $"{prefab.name}(SpatialOS: {entity.SpatialEntityId}";
            return gameObject;
        }
    }
}
