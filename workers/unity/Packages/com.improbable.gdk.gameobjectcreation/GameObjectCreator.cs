using System;
using Improbable.Worker;
using Improbable.Gdk.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.GameObjectCreation
{
    public delegate GameObject PrefabProvider(SpatialOSEntity entity);

    public delegate Vector3 EntityPositionProvider(SpatialOSEntity entity);

    public delegate Quaternion EntityRotationProvider(SpatialOSEntity entity);

    public delegate string GameObjectNameProvider(SpatialOSEntity entity, GameObject prefab);

    /// <inheritdoc />
    /// <summary>
    /// Base class for creating game objects with a position and rotation
    /// </summary>
    public class GameObjectCreator : IEntityGameObjectCreator
    {
        private PrefabProvider prefabProvider;
        private EntityPositionProvider positionProvider;
        private EntityRotationProvider rotationProvider;

        // Prefab provider is (sort of) required so we specify a constructor with it.
        public GameObjectCreator(PrefabProvider prefabProvider)
        {
            this.prefabProvider = prefabProvider;
        }

        /// <summary>
        /// Convenience constructor if you're using all providers
        /// </summary>
        /// <param name="prefabProvider"></param>
        /// <param name="positionProvider"></param>
        /// <param name="rotationProvider"></param>
        public GameObjectCreator(
            PrefabProvider prefabProvider,
            EntityPositionProvider positionProvider,
            EntityRotationProvider rotationProvider)
        {
            this.prefabProvider = prefabProvider;
            this.positionProvider = positionProvider;
            this.rotationProvider = rotationProvider;
        }

        public GameObjectCreator()
        {
        }

        // Convenience builder syntax for creating different combinations of providers without complicated constructors
        public GameObjectCreator PrefabsFrom(PrefabProvider prefabProvider)
        {
            this.prefabProvider = prefabProvider;
            return this;
        }

        public GameObjectCreator PositionFrom(EntityPositionProvider positionProvider)
        {
            this.positionProvider = positionProvider;
            return this;
        }

        public GameObjectCreator RotationFrom(EntityRotationProvider rotationProvider)
        {
            this.rotationProvider = rotationProvider;
            return this;
        }

        public GameObject OnEntityCreated(SpatialOSEntity entity)
        {
            var gameObject = prefabProvider?.Invoke(entity);
            if (gameObject == null)
            {
                return null;
            }

            var position = positionProvider?.Invoke(entity) ?? gameObject.transform.position;
            var rotation = rotationProvider?.Invoke(entity) ?? gameObject.transform.rotation;

            // todo (@tencho): consider if we need to support other instantiation overloads such as parent transform
            return Object.Instantiate(gameObject, position, rotation);
        }

        public void OnEntityRemoved(EntityId entityId, GameObject linkedGameObject)
        {
            if (linkedGameObject != null)
            {
                UnityObjectDestroyer.Destroy(linkedGameObject);
            }
        }
    }

    public class NamedGameObjectCreator : IEntityGameObjectCreator
    {
        private readonly IEntityGameObjectCreator creator;
        private readonly GameObjectNameProvider nameProvider;

        public NamedGameObjectCreator(IEntityGameObjectCreator creator, GameObjectNameProvider nameProvider)
        {
            this.creator = creator;
            this.nameProvider = nameProvider;
        }

        public GameObject OnEntityCreated(SpatialOSEntity entity)
        {
            var gameObject = creator.OnEntityCreated(entity);
            if (gameObject == null)
            {
                return null;
            }

            gameObject.name = nameProvider?.Invoke(entity, gameObject) ?? gameObject.name;
            return gameObject;
        }

        public void OnEntityRemoved(EntityId entityId, GameObject linkedGameObject)
        {
            creator.OnEntityRemoved(entityId, linkedGameObject);
        }
    }
}
