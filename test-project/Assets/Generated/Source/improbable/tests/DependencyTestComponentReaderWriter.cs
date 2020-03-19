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
    public class DependencyTestReaderSubscriptionManager : ReaderSubscriptionManager<DependencyTest.Component, DependencyTestReader>
    {
        public DependencyTestReaderSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependencyTestReader CreateReader(Entity entity, EntityId entityId)
        {
            return new DependencyTestReader(World, entity, entityId);
        }
    }

    [AutoRegisterSubscriptionManager]
    public class DependencyTestWriterSubscriptionManager : WriterSubscriptionManager<DependencyTest.Component, DependencyTestWriter>
    {
        public DependencyTestWriterSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependencyTestWriter CreateWriter(Entity entity, EntityId entityId)
        {
            return new DependencyTestWriter(World, entity, entityId);
        }
    }

    public class DependencyTestReader : Reader<DependencyTest.Component, DependencyTest.Update>
    {
        private Dictionary<Action<uint>, ulong> rootUpdateCallbackToCallbackKey;

        internal DependencyTestReader(World world, Entity entity, EntityId entityId) : base(world, entity, entityId)
        {
        }

        public event Action<uint> OnRootUpdate
        {
            add
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot add field update callback when Reader is not valid.");
                }

                if (rootUpdateCallbackToCallbackKey == null)
                {
                    rootUpdateCallbackToCallbackKey = new Dictionary<Action<uint>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependencyTest.Update>(EntityId, update =>
                {
                    if (update.Root.HasValue)
                    {
                        value(update.Root.Value);
                    }
                });
                rootUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!rootUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                rootUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        protected override void RemoveFieldCallbacks()
        {
            if (rootUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in rootUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                rootUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class DependencyTestWriter : DependencyTestReader
    {
        internal DependencyTestWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(DependencyTest.Update update)
        {
            var component = EntityManager.GetComponentData<DependencyTest.Component>(Entity);

            if (update.Root.HasValue)
            {
                component.Root = update.Root.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, DependencyTest.ComponentId);
        }
    }
}
