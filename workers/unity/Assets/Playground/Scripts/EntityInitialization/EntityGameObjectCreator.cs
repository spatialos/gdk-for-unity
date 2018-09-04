using System.Collections.Generic;
using System.IO;
using Generated.Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using UnityEngine;
using Transform = Generated.Improbable.Transform.Transform;

namespace Playground
{
    public class EntityGameObjectCreator : IEntityGameObjectCreator
    {
        private readonly Dictionary<string, GameObject> cachedPrefabs
            = new Dictionary<string, GameObject>();

        private readonly string workerType;

        public EntityGameObjectCreator(string workerType)
        {
            this.workerType = workerType;
        }

        public GameObject GetGameObjectForEntityAdded(SpatialOSEntity entity, WorkerSystem worker)
        {
            if (!entity.HasComponent<Metadata.Component>())
            {
                return null;
            }

            var prefabName = entity.GetComponent<Metadata.Component>().EntityType;
            if (!entity.HasComponent<Transform.Component>())
            {
                cachedPrefabs[prefabName] = null;
                return null;
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

                if (prefab == null)
                {
                    worker.LogDispatcher.HandleLog(LogType.Log, new LogEvent(
                        $"Prefab not found for SpatialOS Entity in either {workerSpecificPath} or {commonPath}," +
                        "not going to associate a GameObject with it."));
                }

                cachedPrefabs[prefabName] = prefab;
            }

            if (prefab == null)
            {
                return null;
            }

            var gameObject = Object.Instantiate(prefab, position, rotation);
            gameObject.name = $"{prefab.name}(SpatialOS: {entity.SpatialOSEntityId}";
            return gameObject;
        }

        public void OnEntityGameObjectRemoved(SpatialOSEntity entity, WorkerSystem worker, GameObject linkedGameObject)
        {
            UnityObjectDestroyer.Destroy(linkedGameObject);
        }
    }
}
