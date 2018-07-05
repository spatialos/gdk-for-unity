using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Creates a companion gameobject for newly spawned entities according to a prefab definition.
    /// </summary>
    public class SpatialOSGameObjectCreator
    {
        private readonly MutableView mutableView;
        private readonly World world;
        private readonly Dictionary<string, GameObject> cachedPrefabs = new Dictionary<string, GameObject>();

        public SpatialOSGameObjectCreator(MutableView mutableView, World world)
        {
            this.mutableView = mutableView;
            this.world = world;
        }

        public GameObject CreateSpatialOSGameObject(Entity entity, string prefabName, Vector3 position, Quaternion rotation, ViewCommandBuffer viewCommandBuffer, long spatialEntityId)
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

            var spatialOSComponent = gameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.Entity = entity;
            spatialOSComponent.SpatialEntityId = spatialEntityId;
            spatialOSComponent.World = world;

            foreach (var component in gameObject.GetComponents<Component>())
            {
                viewCommandBuffer.AddComponent(entity, component.GetType(), component);
            }

            return gameObject;
        }

        internal static class Errors
        {
            public const string PrefabNotFound = "Prefab for prefabPath {0} not found.";
            public const string SpatialEntityIdNotFound = "SpatialOS EntityId not found for entity {0}/{1}.";
        }

        internal class PrefabNotFoundException : Exception
        {
            public PrefabNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
