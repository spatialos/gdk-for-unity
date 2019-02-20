using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [AutoRegisterSubscriptionManager]
    public class EntitySubscriptionManager : SubscriptionManager<Entity>
    {
        private readonly Dictionary<EntityId, HashSet<Subscription<Entity>>> entityIdToSubscriptions =
            new Dictionary<EntityId, HashSet<Subscription<Entity>>>();

        private readonly WorkerSystem workerSystem;

        public EntitySubscriptionManager(World world)
        {
            workerSystem = world.GetExistingManager<WorkerSystem>();
            if (workerSystem == null)
            {
                throw new ArgumentException("No worker");
            }

            var receiveSystem = world.GetExistingManager<SpatialOSReceiveSystem>();
            if (receiveSystem == null)
            {
                throw new ArgumentException("No worker");
            }

            var constraintsSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();
            if (constraintsSystem == null)
            {
                // todo real error messages
                throw new ArgumentException("Subscriptions systems missing");
            }

            constraintsSystem.RegisterEntityAddedCallback(entityId =>
            {
                if (!entityIdToSubscriptions.TryGetValue(entityId, out var subscriptions))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);
                foreach (var subscription in subscriptions)
                {
                    if (!subscription.HasValue)
                    {
                        subscription.SetAvailable(entity);
                    }
                }
            });

            constraintsSystem.RegisterEntityRemovedCallback(entityId =>
            {
                if (!entityIdToSubscriptions.TryGetValue(entityId, out var subscriptions))
                {
                    return;
                }

                foreach (var subscription in subscriptions)
                {
                    if (subscription.HasValue)
                    {
                        subscription.SetUnavailable();
                    }
                }
            });
        }

        public override Subscription<Entity> Subscribe(EntityId entityId)
        {
            if (!entityIdToSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<Entity>>();
                entityIdToSubscriptions.Add(entityId, subscriptions);
            }

            var subscription = new Subscription<Entity>(this, entityId);
            subscriptions.Add(subscription);

            if (workerSystem.TryGetEntity(entityId, out var entity))
            {
                subscription.SetAvailable(entity);
            }

            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            if (!entityIdToSubscriptions.TryGetValue(subscription.EntityId, out var subscriptions))
            {
                throw new ArgumentException("Go away");
            }

            subscriptions.Remove((Subscription<Entity>) subscription);
        }

        public override void ResetValue(ISubscription subscription)
        {
        }
    }
}
