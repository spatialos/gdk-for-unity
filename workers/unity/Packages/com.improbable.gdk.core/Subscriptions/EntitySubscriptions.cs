using System;
using Improbable.Worker;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    public static class EntitySubscriptions
    {
        public static Subscription<T> Subscribe<T>(EntityId entityId, World world)
        {
            var subscriptionSystem = world.GetExistingManager<SubscriptionSystem>();
            if (subscriptionSystem == null)
            {
                throw new ArgumentException("No subscription system");
            }

            return (Subscription<T>) subscriptionSystem.Subscribe(entityId, typeof(T));
        }

        // public static Subscription<T> Subscribe<T>(Entity entity, World world)
        // {
        //     var workerSystem = world.GetExistingManager<WorkerSystem>();
        //     if (workerSystem == null)
        //     {
        //         throw new ArgumentException("No worker");
        //     }
        //
        //     if (!workerSystem.TryGetEntity(entityId, out var entity))
        //     {
        //         throw new ArgumentException("Couldn't find entity. Not sure this should be an exception. In fact I'm pretty sure it shouldn't be an exception but this will require a bit of a refactor." +
        //             "EntityId subscriptions should hang around until needed. In fact EntityId should be the standard way of making a subscription. I think the only reason I made it entity was that I did most of this later at night");
        //     }
        //
        //     return Subscribe<T>(entity, world);
        // }

        public static void TryRegisterManager<T>(SubscriptionManagerBase manager, World world)
        {
            TryRegisterManager(typeof(T), manager, world);
        }

        public static void TryRegisterManager(Type type, SubscriptionManagerBase manager, World world)
        {
            var subscriptionSystem = world.GetExistingManager<SubscriptionSystem>();
            if (subscriptionSystem == null)
            {
                throw new ArgumentException("No subscription system");
            }

            subscriptionSystem.RegisterSubscriptionManager(type, manager);
        }
    }
}
