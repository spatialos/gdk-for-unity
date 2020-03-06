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
    public class DependencyTestGrandchildReaderSubscriptionManager : ReaderSubscriptionManager<DependencyTestGrandchild.Component, DependencyTestGrandchildReader>
    {
        public DependencyTestGrandchildReaderSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependencyTestGrandchildReader CreateReader(Entity entity, EntityId entityId)
        {
            return new DependencyTestGrandchildReader(World, entity, entityId);
        }
    }

    [AutoRegisterSubscriptionManager]
    public class DependencyTestGrandchildWriterSubscriptionManager : WriterSubscriptionManager<DependencyTestGrandchild.Component, DependencyTestGrandchildWriter>
    {
        public DependencyTestGrandchildWriterSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependencyTestGrandchildWriter CreateWriter(Entity entity, EntityId entityId)
        {
            return new DependencyTestGrandchildWriter(World, entity, entityId);
        }
    }

    public class DependencyTestGrandchildReader : Reader<DependencyTestGrandchild.Component, DependencyTestGrandchild.Update>
    {
        private Dictionary<Action<uint>, ulong> grandchildUpdateCallbackToCallbackKey;

        internal DependencyTestGrandchildReader(World world, Entity entity, EntityId entityId) : base(world, entity, entityId)
        {
        }

        public event Action<uint> OnGrandchildUpdate
        {
            add
            {
                if (grandchildUpdateCallbackToCallbackKey == null)
                {
                    grandchildUpdateCallbackToCallbackKey = new Dictionary<Action<uint>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependencyTestGrandchild.Update>(EntityId, update =>
                {
                    if (update.Grandchild.HasValue)
                    {
                        value(update.Grandchild.Value);
                    }
                });
                grandchildUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!grandchildUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                grandchildUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        protected override void RemoveFieldCallbacks()
        {
            if (grandchildUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in grandchildUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                grandchildUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class DependencyTestGrandchildWriter : DependencyTestGrandchildReader
    {
        internal DependencyTestGrandchildWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(DependencyTestGrandchild.Update update)
        {
            var component = EntityManager.GetComponentData<DependencyTestGrandchild.Component>(Entity);

            if (update.Grandchild.HasValue)
            {
                component.Grandchild = update.Grandchild.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, DependencyTestGrandchild.ComponentId);
        }
    }
}
