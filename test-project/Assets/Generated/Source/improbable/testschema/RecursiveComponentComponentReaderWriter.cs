// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.TestSchema
{
    [AutoRegisterSubscriptionManager]
    public class RecursiveComponentReaderSubscriptionManager : ReaderSubscriptionManager<RecursiveComponent.Component, RecursiveComponentReader>
    {
        public RecursiveComponentReaderSubscriptionManager(World world) : base(world)
        {
        }

        protected override RecursiveComponentReader CreateReader(Entity entity, EntityId entityId)
        {
            return new RecursiveComponentReader(World, entity, entityId);
        }
    }

    [AutoRegisterSubscriptionManager]
    public class RecursiveComponentWriterSubscriptionManager : WriterSubscriptionManager<RecursiveComponent.Component, RecursiveComponentWriter>
    {
        public RecursiveComponentWriterSubscriptionManager(World world) : base(world)
        {
        }

        protected override RecursiveComponentWriter CreateWriter(Entity entity, EntityId entityId)
        {
            return new RecursiveComponentWriter(World, entity, entityId);
        }
    }

    public class RecursiveComponentReader : Reader<RecursiveComponent.Component, RecursiveComponent.Update>
    {
        private Dictionary<Action<global::Improbable.TestSchema.TypeA>, ulong> aUpdateCallbackToCallbackKey;

        private Dictionary<Action<global::Improbable.TestSchema.TypeB>, ulong> bUpdateCallbackToCallbackKey;

        private Dictionary<Action<global::Improbable.TestSchema.TypeC>, ulong> cUpdateCallbackToCallbackKey;

        internal RecursiveComponentReader(World world, Entity entity, EntityId entityId) : base(world, entity, entityId)
        {
        }

        public event Action<global::Improbable.TestSchema.TypeA> OnAUpdate
        {
            add
            {
                if (aUpdateCallbackToCallbackKey == null)
                {
                    aUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.TypeA>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<RecursiveComponent.Update>(EntityId, update =>
                {
                    if (update.A.HasValue)
                    {
                        value(update.A.Value);
                    }
                });
                aUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!aUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                aUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::Improbable.TestSchema.TypeB> OnBUpdate
        {
            add
            {
                if (bUpdateCallbackToCallbackKey == null)
                {
                    bUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.TypeB>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<RecursiveComponent.Update>(EntityId, update =>
                {
                    if (update.B.HasValue)
                    {
                        value(update.B.Value);
                    }
                });
                bUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!bUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                bUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::Improbable.TestSchema.TypeC> OnCUpdate
        {
            add
            {
                if (cUpdateCallbackToCallbackKey == null)
                {
                    cUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.TypeC>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<RecursiveComponent.Update>(EntityId, update =>
                {
                    if (update.C.HasValue)
                    {
                        value(update.C.Value);
                    }
                });
                cUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!cUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                cUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        protected override void RemoveFieldCallbacks()
        {
            if (aUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in aUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                aUpdateCallbackToCallbackKey.Clear();
            }

            if (bUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in bUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                bUpdateCallbackToCallbackKey.Clear();
            }

            if (cUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in cUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                cUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class RecursiveComponentWriter : RecursiveComponentReader
    {
        internal RecursiveComponentWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(RecursiveComponent.Update update)
        {
            var component = EntityManager.GetComponentData<RecursiveComponent.Component>(Entity);

            if (update.A.HasValue)
            {
                component.A = update.A.Value;
            }

            if (update.B.HasValue)
            {
                component.B = update.B.Value;
            }

            if (update.C.HasValue)
            {
                component.C = update.C.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, RecursiveComponent.ComponentId);
        }
    }
}
