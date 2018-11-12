using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Worker;
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

            receiveSystem.Dispatcher.OnAddEntity(op =>
            {
                if (!entityIdToSubscriptions.TryGetValue(op.EntityId, out var subscriptions))
                {
                    return;
                }

                workerSystem.TryGetEntity(op.EntityId, out var entity);
                foreach (var subscription in subscriptions)
                {
                    subscription.SetAvailable(entity);
                }
            });

            receiveSystem.Dispatcher.OnRemoveEntity(op =>
            {
                if (!entityIdToSubscriptions.TryGetValue(op.EntityId, out var subscriptions))
                {
                    return;
                }

                foreach (var subscription in subscriptions)
                {
                    subscription.SetUnavailable();
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

        public override void Cancel(ITypeErasedSubscription subscription)
        {
            if (!entityIdToSubscriptions.TryGetValue(subscription.EntityId, out var subscriptions))
            {
                throw new ArgumentException("Go away");
            }

            var x = subscriptions.Remove((Subscription<Entity>) subscription);
            if (x == false)
            {
                int y = 9;
            }
        }

        public override void ResetValue(ITypeErasedSubscription subscription)
        {
        }
    }
}
