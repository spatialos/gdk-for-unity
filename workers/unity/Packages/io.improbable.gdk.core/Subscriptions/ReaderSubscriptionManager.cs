using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public abstract class ReaderSubscriptionManager<TComponent, TReader> : SubscriptionManager<TReader>
        where TComponent : struct, ISpatialComponentData
        where TReader : IRequireable
    {
        private readonly EntityManager entityManager;

        private Dictionary<EntityId, HashSet<Subscription<TReader>>> entityIdToReaderSubscriptions;

        private readonly HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private readonly HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();
        
        private static readonly uint ComponentId = ComponentDatabase.GetComponentId<TComponent>();

        protected ReaderSubscriptionManager(World world) : base(world)
        {
            entityManager = world.EntityManager;

            RegisterComponentCallbacks();
        }

        protected abstract TReader CreateReader(Entity entity, EntityId entityId);

        private void RegisterComponentCallbacks()
        {
            var constraintCallbackSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintCallbackSystem.RegisterComponentAddedCallback(ComponentId, entityId =>
            {
                if (!entitiesNotMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    subscription.SetAvailable(CreateReader(entity, entityId));
                }

                entitiesMatchingRequirements.Add(entityId);
                entitiesNotMatchingRequirements.Remove(entityId);
            });

            constraintCallbackSystem.RegisterComponentRemovedCallback(ComponentId, entityId =>
            {
                if (!entitiesMatchingRequirements.Contains(entityId))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out _);

                foreach (var subscription in entityIdToReaderSubscriptions[entityId])
                {
                    ResetValue(subscription);
                    subscription.SetUnavailable();
                }

                entitiesNotMatchingRequirements.Add(entityId);
                entitiesMatchingRequirements.Remove(entityId);
            });
        }

        public override Subscription<TReader> Subscribe(EntityId entityId)
        {
            if (entityIdToReaderSubscriptions == null)
            {
                entityIdToReaderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<TReader>>>();
            }

            var subscription = new Subscription<TReader>(this, entityId);

            if (!entityIdToReaderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<TReader>>();
                entityIdToReaderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && entityManager.HasComponent<TComponent>(entity))
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(CreateReader(entity, entityId));
            }
            else
            {
                entitiesNotMatchingRequirements.Add(entityId);
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<TReader>) subscription);
            ResetValue(sub);

            var subscriptions = entityIdToReaderSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToReaderSubscriptions.Remove(sub.EntityId);
                entitiesMatchingRequirements.Remove(sub.EntityId);
                entitiesNotMatchingRequirements.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<TReader>) subscription);
            if (sub.HasValue)
            {
                var reader = sub.Value;
                reader.IsValid = false;
                reader.RemoveAllCallbacks();
            }
        }
    }
}
