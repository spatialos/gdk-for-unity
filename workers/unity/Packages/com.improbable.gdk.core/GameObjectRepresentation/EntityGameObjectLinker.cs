using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Links GameObjects and ECS Entities.
    /// </summary>
    public class EntityGameObjectLinker
    {
        private static readonly EntityId WorkerEntityId = new EntityId(0);

        private readonly World world;
        private readonly WorkerSystem worker;
        private readonly EntityManager entityManager;
        private readonly HashSet<Type> gameObjectComponentTypes = new HashSet<Type>();

        public EntityGameObjectLinker(World world, WorkerSystem worker)
        {
            this.world = world;
            this.worker = worker;
            entityManager = world.GetExistingManager<EntityManager>();
        }

        /// <summary>
        ///     Links a GameObject to an ECS Entity.
        /// </summary>
        /// <remarks>
        ///     All <see cref="UnityEngine.Component" />s on the GameObject will be inserted onto the ECS Entity. Note
        ///     that children and parent components are not added.
        /// </remarks>
        /// <remarks>
        ///     A <see cref="SpatialOSComponent" /> will be added to the GameObject.
        /// </remarks>
        /// <remarks>
        ///     If a Monobehaviour exists multiple times on the GameObject, only the first occurence is added
        ///     to the ECS entity.
        /// </remarks>
        /// <param name="gameObject">The GameObject to link.</param>
        /// <param name="entity">The entity to link.</param>
        /// <param name="viewCommandBuffer">
        ///     An instance of the ViewCommandBuffer. Must be flushed to apply the changes to the ECS Entity.
        /// </param>
        public void LinkGameObjectToEntity(GameObject gameObject, Entity entity, ViewCommandBuffer viewCommandBuffer)
        {
            var hasSpatialEntityId = entityManager.HasComponent<SpatialEntityId>(entity);
            var isWorkerEntity = entityManager.HasComponent<WorkerEntityTag>(entity);
            if (!hasSpatialEntityId && !isWorkerEntity)
            {
                worker.LogDispatcher.HandleLog(LogType.Warning, new LogEvent(
                        "Attempted to link GameObject to an entity that is not a SpatialOS entity or the worker entity")
                    .WithField(LoggingUtils.LoggerName, nameof(EntityGameObjectLinker)));
                return;
            }

            EntityId spatialEntityId;
            if (hasSpatialEntityId)
            {
                spatialEntityId = entityManager.GetComponentData<SpatialEntityId>(entity).EntityId;
            }
            else // worker entity
            {
                spatialEntityId = WorkerEntityId;
            }

            gameObjectComponentTypes.Clear();
            foreach (var component in gameObject.GetComponents<Component>())
            {
                if (ReferenceEquals(component, null))
                {
                    continue;
                }

                var componentType = component.GetType();
                if (gameObjectComponentTypes.Contains(componentType))
                {
                    worker.LogDispatcher.HandleLog(LogType.Warning, new LogEvent(
                            "GameObject contains multiple instances of the same component type. Only one instance of each component type will be added to the corresponding ECS entity.")
                        .WithField("EntityId", spatialEntityId)
                        .WithField("ComponentType", componentType));
                    continue;
                }

                gameObjectComponentTypes.Add(componentType);
                viewCommandBuffer.AddComponent(entity, component.GetType(), component);
            }

            viewCommandBuffer.AddComponent(entity, new GameObjectReference { GameObject = gameObject });

            var spatialOSComponent = gameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.World = world;
            spatialOSComponent.Worker = worker;
            spatialOSComponent.Entity = entity;
            spatialOSComponent.SpatialEntityId = spatialEntityId;
        }

        /// <summary>
        ///     Unlinks a GameObject and ECS Entity.
        /// </summary>
        /// <remarks>
        ///     The GameObject and ECS Entity should be linked before this method call.
        /// </remarks>
        /// <param name="gameObject">The GameObject to unlink.</param>
        /// <param name="entity">The ECS Entity to unlink.</param>
        /// <param name="viewCommandBuffer">
        ///     An instance of the ViewCommandBuffer. Must be flushed to apply the changes to the ECS Entity.
        /// </param>
        public void UnlinkGameObjectFromEntity(GameObject gameObject, Entity entity,
            ViewCommandBuffer viewCommandBuffer)
        {
            if (!entityManager.Exists(entity))
            {
                return;
            }

            if (gameObject != null)
            {
                foreach (var component in gameObject.GetComponents<Component>())
                {
                    if (ReferenceEquals(component, null))
                    {
                        continue;
                    }

                    var componentType = component.GetType();
                    if (entityManager.HasComponent(entity, componentType))
                    {
                        viewCommandBuffer.RemoveComponent(entity, componentType);
                    }
                }

                var spatialOSComponent = gameObject.GetComponent<SpatialOSComponent>();
                if (spatialOSComponent != null)
                {
                    UnityObjectDestroyer.Destroy(spatialOSComponent);
                }
            }

            if (entityManager.HasComponent<GameObjectReference>(entity))
            {
                viewCommandBuffer.RemoveComponent(entity, typeof(GameObjectReference));
            }
        }
    }
}
