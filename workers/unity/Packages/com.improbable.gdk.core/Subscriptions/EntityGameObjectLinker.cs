using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Subscriptions
{
    public class EntityGameObjectLinker
    {
        private readonly WorkerSystem workerSystem;
        private readonly SubscriptionSystem subscriptionSystem;
        private readonly RequireLifecycleSystem lifecycleSystem;
        private readonly EntityManager entityManager;
        private readonly World world;

        private readonly Dictionary<EntityId, List<GameObject>> entityIdToGameObjects =
            new Dictionary<EntityId, List<GameObject>>();

        private readonly Dictionary<GameObject, List<RequiredSubscriptionsInjector>> gameObjectToInjectors =
            new Dictionary<GameObject, List<RequiredSubscriptionsInjector>>();

        private readonly Dictionary<GameObject, List<ComponentType>> gameObjectToComponentsAdded =
            new Dictionary<GameObject, List<ComponentType>>();

        private readonly ViewCommandBuffer viewCommandBuffer;

        public EntityGameObjectLinker(World world)
        {
            this.world = world;
            entityManager = world.GetOrCreateManager<EntityManager>();

            workerSystem = world.GetExistingManager<WorkerSystem>();
            lifecycleSystem = world.GetExistingManager<RequireLifecycleSystem>();

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

        public void LinkGameObjectToSpatialOSEntity(EntityId entityId, GameObject gameObject,
            params Type[] componentTypesToAdd)
        {
            if (!workerSystem.TryGetEntity(entityId, out var entity))
            {
                throw new ArgumentException("Entity not in view");
            }

            if (gameObjectToComponentsAdded.TryGetValue(gameObject, out var added))
            {
                throw new InvalidOperationException("GameObject already linked to an entity");
            }

            var componentTypes = new List<ComponentType>(componentTypesToAdd.Length);
            gameObjectToComponentsAdded.Add(gameObject, componentTypes);

            if (!entityIdToGameObjects.TryGetValue(entityId, out var linkedGameObjects))
            {
                linkedGameObjects = new List<GameObject>();
                entityIdToGameObjects.Add(entityId, linkedGameObjects);
            }

            linkedGameObjects.Add(gameObject);

            foreach (var type in componentTypesToAdd)
            {
                if (!type.IsSubclassOf(typeof(Component)))
                {
                    throw new InvalidOperationException("Types must be derived from Component or MonoBehaviour");
                }

                var c = gameObject.GetComponent(type);
                if (c != null)
                {
                    var componentType = new ComponentType(type);
                    componentTypes.Add(componentType);
                    viewCommandBuffer.AddComponent(entity, componentType, c);
                }
            }

            var linkedComponent = gameObject.GetComponent<LinkedEntityComponent>();
            if (linkedComponent == null)
            {
                linkedComponent = gameObject.AddComponent<LinkedEntityComponent>();
            }
            else if (linkedComponent.IsValid)
            {
                // todo already being linked is checked above so this shouldn't happen - reorg to make contracts more clear
                throw new InvalidOperationException("GameObject is already linked to an entity");
            }

            linkedComponent.IsValid = true;
            linkedComponent.Linker = this;
            linkedComponent.EntityId = entityId;
            linkedComponent.World = world;
            linkedComponent.Entity = entity;

            var injectors = new List<RequiredSubscriptionsInjector>();

            foreach (var component in gameObject.GetComponents<MonoBehaviour>())
            {
                if (component == null)
                {
                    // todo consider - could also tell the user that they have a bad ref here
                    continue;
                }

                var type = component.GetType();
                if (RequiredSubscriptionsDatabase.HasRequiredSubscriptions(type) &&
                    RequiredSubscriptionsDatabase.WorkerTypeMatchesRequirements(workerSystem.WorkerType, type))
                {
                    // todo this should possibly happen when the command buffer is flushed too
                    injectors.Add(new RequiredSubscriptionsInjector(component, entityId, subscriptionSystem,
                        () => lifecycleSystem.EnableMonoBehaviour(component),
                        () => lifecycleSystem.DisableMonoBehaviour(component)));
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

            if (workerSystem.TryGetEntity(entityId, out var entity) && gameObject != null)
            {
                foreach (var componentType in gameObjectToComponentsAdded[gameObject])
                {
                    entityManager.RemoveComponent(entity, componentType);
                }

                gameObjectToComponentsAdded.Remove(gameObject);
            }

            var linkComponent = gameObject.GetComponent<LinkedEntityComponent>();
            // todo check if this can happen - right now I think it can on application quit
            if (linkComponent != null)
            {
                linkComponent.Invalidate();
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

        public void UnlinkAllGameObjectsFromEntityId(EntityId entityId)
        {
            if (!entityIdToGameObjects.TryGetValue(entityId, out var gameObjectSet))
            {
                return;
            }

            workerSystem.TryGetEntity(entityId, out var entity);

            while (gameObjectSet.Count > 0)
            {
                UnlinkGameObjectFromEntity(entityId, gameObjectSet[gameObjectSet.Count - 1]);
            }
        }

        // todo this is slow and crap - work out if it needs to not be
        public List<EntityId> GetLinkedEntityIds()
        {
            List<EntityId> entitiesToRemove = new List<EntityId>(entityIdToGameObjects.Count);
            foreach (var entityIdAndGameObjects in entityIdToGameObjects)
            {
                entitiesToRemove.Add(entityIdAndGameObjects.Key);
            }

            return entitiesToRemove;
        }

        public void FlushCommandBuffer()
        {
            viewCommandBuffer.FlushBuffer();
        }
    }
}
