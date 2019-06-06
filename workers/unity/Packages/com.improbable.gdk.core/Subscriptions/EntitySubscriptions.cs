using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public static class EntitySubscriptions
    {
        public static Subscription<T> Subscribe<T>(EntityId entityId, World world)
        {
            var subscriptionSystem = world.GetExistingSystem<SubscriptionSystem>();
            if (subscriptionSystem == null)
            {
                throw new ArgumentException("No subscription system");
            }

            return (Subscription<T>) subscriptionSystem.Subscribe<T>(entityId);
        }

        public static void TryRegisterManager<T>(SubscriptionManagerBase manager, World world)
        {
            TryRegisterManager(typeof(T), manager, world);
        }

        public static void TryRegisterManager(Type type, SubscriptionManagerBase manager, World world)
        {
            var subscriptionSystem = world.GetExistingSystem<SubscriptionSystem>();
            if (subscriptionSystem == null)
            {
                throw new ArgumentException("No subscription system");
            }

            subscriptionSystem.RegisterSubscriptionManager(type, manager);
        }
    }
}
