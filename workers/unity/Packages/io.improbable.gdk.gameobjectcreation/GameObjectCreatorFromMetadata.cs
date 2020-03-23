using System;
using System.Collections.Generic;
using System.IO;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.GameObjectCreation
{
    public class GameObjectCreatorFromMetadata : IEntityGameObjectCreator
    {
        private readonly Dictionary<string, GameObject> cachedPrefabs
            = new Dictionary<string, GameObject>();

        private readonly string workerType;
        private readonly Vector3 workerOrigin;


        private readonly Dictionary<EntityId, GameObject> entityIdToGameObject = new Dictionary<EntityId, GameObject>();

        private readonly Type[] componentsToAdd =
        {
            typeof(Transform), typeof(Rigidbody), typeof(MeshRenderer)
        };

        public ComponentType[] MinimumComponentTypes { get; } =
        {
            ComponentType.ReadOnly<Metadata.Component>(), ComponentType.ReadOnly<Position.Component>()
        };

        public GameObjectCreatorFromMetadata(string workerType, Vector3 workerOrigin, ILogDispatcher logger)
        {
            this.workerType = workerType;
            this.workerOrigin = workerOrigin;
        }

        public void Register(Dictionary<string, EntityTypeRegistration> entityTypeRegistrations)
        {
        }

        public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
        {
            if (!entity.TryGetComponent<Metadata.Component>(out var metadata) ||
                !entity.TryGetComponent<Position.Component>(out var spatialOSPosition))
            {
                return;
            }

            var prefabName = metadata.EntityType;
            var position = spatialOSPosition.Coords.ToUnityVector() + workerOrigin;

            if (!cachedPrefabs.TryGetValue(prefabName, out var prefab))
            {
                var workerSpecificPath = Path.Combine("Prefabs", workerType, prefabName);
                var commonPath = Path.Combine("Prefabs", "Common", prefabName);

                prefab = Resources.Load<GameObject>(workerSpecificPath) ?? Resources.Load<GameObject>(commonPath);
                cachedPrefabs[prefabName] = prefab;
            }

            if (prefab == null)
            {
                return;
            }

            var gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObject.name = $"{prefab.name}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";

            entityIdToGameObject.Add(entity.SpatialOSEntityId, gameObject);
            linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, gameObject, componentsToAdd);
        }

        public void OnEntityRemoved(EntityId entityId)
        {
            if (!entityIdToGameObject.TryGetValue(entityId, out var go))
            {
                return;
            }

            Object.Destroy(go);
            entityIdToGameObject.Remove(entityId);
        }
    }
}
