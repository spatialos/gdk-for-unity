using System.Collections.Generic;
using System.IO;
using Improbable.Gdk.Core;
using Improbable.Worker;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation
{
    public class GameObjectCreatorFromMetadata : IEntityGameObjectCreator
    {
        private readonly Dictionary<string, GameObject> cachedPrefabs
            = new Dictionary<string, GameObject>();

        private readonly string workerType;
        private readonly Vector3 workerOrigin;

        private readonly ILogDispatcher logger;

        public GameObjectCreatorFromMetadata(string workerType, Vector3 workerOrigin, ILogDispatcher logger)
        {
            this.workerType = workerType;
            this.workerOrigin = workerOrigin;
            this.logger = logger;
        }

        public GameObject OnEntityCreated(SpatialOSEntity entity)
        {
            if (!entity.HasComponent<Metadata.Component>())
            {
                return null;
            }

            var prefabName = entity.GetComponent<Metadata.Component>().EntityType;
            if (!entity.HasComponent<Position.Component>())
            {
                cachedPrefabs[prefabName] = null;
                return null;
            }

            var spatialOSPosition = entity.GetComponent<Position.Component>();
            var position = new Vector3((float) spatialOSPosition.Coords.X, (float) spatialOSPosition.Coords.Y, (float) spatialOSPosition.Coords.Z) +
                workerOrigin;
            var workerSpecificPath = Path.Combine("Prefabs", workerType, prefabName);
            var commonPath = Path.Combine("Prefabs", "Common", prefabName);

            if (!cachedPrefabs.TryGetValue(prefabName, out var prefab))
            {
                prefab = Resources.Load<GameObject>(workerSpecificPath);
                if (prefab == null)
                {
                    prefab = Resources.Load<GameObject>(commonPath);
                }

                cachedPrefabs[prefabName] = prefab;
            }

            if (prefab == null)
            {
                return null;
            }

            var gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObject.name = $"{prefab.name}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";
            return gameObject;
        }

        public void OnEntityRemoved(EntityId entityId, GameObject linkedGameObject)
        {
            if (linkedGameObject != null)
            {
                UnityObjectDestroyer.Destroy(linkedGameObject);
            }
        }
    }
}
