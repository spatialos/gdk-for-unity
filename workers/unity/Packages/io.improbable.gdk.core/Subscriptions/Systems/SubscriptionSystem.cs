using System;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.Subscriptions
{
    [DisableAutoCreation]
    public class SubscriptionSystem : ComponentSystem
    {
        private readonly Dictionary<Type, SubscriptionManagerBase> typeToSubscriptionManager =
            new Dictionary<Type, SubscriptionManagerBase>();

        public void RegisterSubscriptionManager(Type type, SubscriptionManagerBase manager)
        {
            if (typeToSubscriptionManager.ContainsKey(type))
            {
                throw new InvalidOperationException("Already a manager registered");
            }

            typeToSubscriptionManager[type] = manager;
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

        protected override void OnCreate()
        {
            base.OnCreate();
            Enabled = false;

            AutoRegisterManagers();
        }

        protected override void OnUpdate()
        {
        }

        private void AutoRegisterManagers()
        {
            var types = ReflectionUtility.GetNonAbstractTypes(typeof(SubscriptionManagerBase),
                typeof(AutoRegisterSubscriptionManagerAttribute));

            foreach (var type in types)
            {
                var instance = (SubscriptionManagerBase) Activator.CreateInstance(type, World);
                RegisterSubscriptionManager(instance.SubscriptionType, instance);
            }
        }
    }
}
