using System;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;

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
                // delete this once I work out how it should actually work
                //return;
                throw new InvalidOperationException("Already a manager registered");
            }

            typeToSubscriptionManager[type] = manager;
        }

        public Subscription<T> Subscribe<T>(EntityId entity)
        {
            if (!typeToSubscriptionManager.TryGetValue(typeof(T), out var manager))
            {
                throw new ArgumentException($"no manager for {typeof(T).Name}");
            }

            return ((SubscriptionManager<T>) manager).Subscribe(entity);
        }

        public ITypeErasedSubscription Subscribe(EntityId entity, Type type)
        {
            if (!typeToSubscriptionManager.TryGetValue(type, out var manager))
            {
                throw new ArgumentException($"no manager for {type.Name}");
            }

            return manager.SubscribeTypeErased(entity);
        }

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            Enabled = false;

            AutoRegisterManagers();
        }

        protected override void OnUpdate()
        {
        }

        private void AutoRegisterManagers()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(SubscriptionManagerBase).IsAssignableFrom(type) || type.IsAbstract)
                    {
                        continue;
                    }

                    if (type.GetCustomAttribute<AutoRegisterSubscriptionManagerAttribute>() == null)
                    {
                        continue;
                    }

                    var instance = (SubscriptionManagerBase) Activator.CreateInstance(type, World);
                    RegisterSubscriptionManager(instance.SubscriptionType, instance);
                }
            }
        }
    }
}
