using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Subscriptions
{
    public class EntityGameObjectLinker
    {
        private readonly WorkerSystem workerSystem;
        private readonly SubscriptionSystem subscriptionSystem;
        private readonly EntityManager entityManager;

        private readonly Dictionary<EntityId, HashSet<GameObject>> entityIdToGameObjects =
            new Dictionary<EntityId, HashSet<GameObject>>();

        private readonly Dictionary<GameObject, List<RequiredSubscriptionsInjector>> gameObjectToInjectors =
            new Dictionary<GameObject, List<RequiredSubscriptionsInjector>>();

        private readonly Action<Entity, ComponentType, object> setComponentObjectAction;

        private readonly ViewCommandBuffer viewCommandBuffer;

        public EntityGameObjectLinker(World world)
        {
            entityManager = world.GetOrCreateManager<EntityManager>();

            workerSystem = world.GetExistingManager<WorkerSystem>();

            viewCommandBuffer = new ViewCommandBuffer(entityManager, workerSystem.LogDispatcher);

            if (workerSystem == null)
            {
                throw new ArgumentException("Whatever you do don't jump");
            }

            subscriptionSystem = world.GetExistingManager<SubscriptionSystem>();

            if (subscriptionSystem == null)
            {
                throw new ArgumentException("There are people who love you presumably");
            }
        }

        public void LinkGameObjectToSpatialOSEntity(EntityId entityId, GameObject gameObject)
        {
            if (!workerSystem.TryGetEntity(entityId, out var entity))
            {
                throw new ArgumentException("Entity not in view");
            }

            if (!entityIdToGameObjects.TryGetValue(entityId, out var linkedGameObjects))
            {
                linkedGameObjects = new HashSet<GameObject>();
                entityIdToGameObjects.Add(entityId, linkedGameObjects);
            }

            linkedGameObjects.Add(gameObject);

            if (!entityManager.HasComponent<Transform>(entity))
            {
                viewCommandBuffer.AddComponent(entity, gameObject.GetComponent<Transform>());
            }

            if (!entityManager.HasComponent<Rigidbody>(entity))
            {
                var rigidobdy = gameObject.GetComponent<Rigidbody>();
                if (rigidobdy != null)
                {
                    viewCommandBuffer.AddComponent(entity, rigidobdy);
                }
            }

            if (!entityManager.HasComponent<MeshRenderer>(entity))
            {
                var renderer = gameObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    viewCommandBuffer.AddComponent(entity, renderer);
                }
            }

            var injectors = new List<RequiredSubscriptionsInjector>();

            foreach (var component in gameObject.GetComponents<MonoBehaviour>())
            {
                if (component == null)
                {
                    // could also tell the user that they have a bad ref here
                    continue;
                }

                var type = component.GetType();
                if (RequiredSubscriptionsDatabase.HasRequiredSubscriptions(type) &&
                    RequiredSubscriptionsDatabase.WorkerTypeMatchesRequirements(workerSystem.WorkerType, type))
                {
                    // todo this should possibly happen when the command buffer is flushed too
                    injectors.Add(new RequiredSubscriptionsInjector(component, entityId, subscriptionSystem,
                        () => component.enabled = true, () => component.enabled = false));
                }
            }

            gameObjectToInjectors.Add(gameObject, injectors);
        }

        public void UnlinkGameObjectFromEntity(EntityId entityId, GameObject gameObject)
        {
            if (!entityIdToGameObjects.TryGetValue(entityId, out var gameObjectSet))
            {
                throw new ArgumentException("This princess is in another castle");
            }

            if (!gameObjectToInjectors.TryGetValue(gameObject, out var injectors))
            {
                throw new ArgumentException("Nothing is here anymore. Maybe there never was");
            }

            foreach (var injector in injectors)
            {
                injector.CancelSubscriptions();
            }

            gameObjectToInjectors.Remove(gameObject);

            gameObjectSet.Remove(gameObject);
            if (gameObjectSet.Count == 0)
            {
                entityIdToGameObjects.Remove(entityId);
            }
        }

        public void UnlinkAllGameObjectsFromEntity(EntityId entityId)
        {
            if (!entityIdToGameObjects.TryGetValue(entityId, out var gameObjectSet))
            {
                return;
            }

            foreach (var gameObject in gameObjectSet)
            {
                if (!gameObjectToInjectors.TryGetValue(gameObject, out var injectors))
                {
                    continue;
                }

                foreach (var injector in injectors)
                {
                    injector.CancelSubscriptions();
                }

                gameObjectToInjectors.Remove(gameObject);
            }

            entityIdToGameObjects.Remove(entityId);
        }

        public void FlushCommandBuffer()
        {
            viewCommandBuffer.FlushBuffer();
        }
    }
}
