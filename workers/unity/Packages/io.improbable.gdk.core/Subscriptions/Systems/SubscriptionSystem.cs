using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [DisableAutoCreation]
    public class SubscriptionSystem : ComponentSystem
    {
        private readonly Dictionary<Type, SubscriptionManagerBase> typeToSubscriptionManager =
            new Dictionary<Type, SubscriptionManagerBase>();

        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;

            AutoRegisterManagers();
        }

        public void RegisterSubscriptionManager(Type type, SubscriptionManagerBase manager)
        {
            if (typeToSubscriptionManager.ContainsKey(type))
            {
                throw new InvalidOperationException($"Duplicate manager for {type.Name}.");
            }

            typeToSubscriptionManager.Add(type, manager);
        }

        public Subscription<T> Subscribe<T>(EntityId entity)
        {
            if (!typeToSubscriptionManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException($"No manager for {typeof(T).Name}.");
            }

            return ((SubscriptionManager<T>) manager).Subscribe(entity);
        }

        public ISubscription Subscribe(EntityId entity, Type type)
        {
            if (!typeToSubscriptionManager.TryGetValue(type, out var manager))
            {
                throw new ArgumentException($"No manager for {type.Name}.");
            }

            return manager.SubscribeTypeErased(entity);
        }

        public SubscriptionAggregate Subscribe(EntityId entity, params Type[] types)
        {
            var subscriptions = new ISubscription[types.Length];
            for (var i = 0; i < types.Length; i++)
            {
                subscriptions[i] = Subscribe(entity, types[i]);
            }

            return new SubscriptionAggregate(types, subscriptions);
        }

        protected override void OnUpdate()
        {
        }

        private void AutoRegisterManagers()
        {
            var types = TypeCache.GetSubscriptionManagerTypes();

            foreach (var type in types)
            {
                var instance = (SubscriptionManagerBase) Activator.CreateInstance(type, World);
                RegisterSubscriptionManager(instance.SubscriptionType, instance);
            }
        }
    }
}
