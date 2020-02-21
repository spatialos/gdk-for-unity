// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.Tests
{
    [AutoRegisterSubscriptionManager]
    public class DependencyTestChildReaderSubscriptionManager : ReaderSubscriptionManager<DependencyTestChild.Component, DependencyTestChildReader>
    {
        public DependencyTestChildReaderSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependencyTestChildReader CreateReader(Entity entity, EntityId entityId)
        {
            return new DependencyTestChildReader(world, entity, entityId);
        }
    }

    [AutoRegisterSubscriptionManager]
    public class DependencyTestChildWriterSubscriptionManager : WriterSubscriptionManager<DependencyTestChild.Component, DependencyTestChildWriter>
    {
        public DependencyTestChildWriterSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependencyTestChildWriter CreateWriter(Entity entity, EntityId entityId)
        {
            return new DependencyTestChildWriter(world, entity, entityId);
        }
    }

    public class DependencyTestChildReader : Reader<DependencyTestChild.Component, DependencyTestChild.Update>
    {
        private Dictionary<Action<uint>, ulong> childUpdateCallbackToCallbackKey;

        internal DependencyTestChildReader(World world, Entity entity, EntityId entityId) : base(world, entity, entityId)
        {
        }

        public event Action<uint> OnChildUpdate
        {
            add
            {
                if (childUpdateCallbackToCallbackKey == null)
                {
                    childUpdateCallbackToCallbackKey = new Dictionary<Action<uint>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependencyTestChild.Update>(EntityId, update =>
                {
                    if (update.Child.HasValue)
                    {
                        value(update.Child.Value);
                    }
                });
                childUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!childUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                childUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        protected override void RemoveFieldCallbacks()
        {
            if (childUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in childUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                childUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class DependencyTestChildWriter : DependencyTestChildReader
    {
        internal DependencyTestChildWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(DependencyTestChild.Update update)
        {
            var component = EntityManager.GetComponentData<DependencyTestChild.Component>(Entity);

            if (update.Child.HasValue)
            {
                component.Child = update.Child.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, DependencyTestChild.ComponentId);
        }
    }
}
