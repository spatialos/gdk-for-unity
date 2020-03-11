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
    public class ComponentUsingNestedTypeSameNameReaderSubscriptionManager : ReaderSubscriptionManager<ComponentUsingNestedTypeSameName.Component, ComponentUsingNestedTypeSameNameReader>
    {
        public ComponentUsingNestedTypeSameNameReaderSubscriptionManager(World world) : base(world)
        {
        }

        protected override ComponentUsingNestedTypeSameNameReader CreateReader(Entity entity, EntityId entityId)
        {
            return new ComponentUsingNestedTypeSameNameReader(World, entity, entityId);
        }
    }

    [AutoRegisterSubscriptionManager]
    public class ComponentUsingNestedTypeSameNameWriterSubscriptionManager : WriterSubscriptionManager<ComponentUsingNestedTypeSameName.Component, ComponentUsingNestedTypeSameNameWriter>
    {
        public ComponentUsingNestedTypeSameNameWriterSubscriptionManager(World world) : base(world)
        {
        }

        protected override ComponentUsingNestedTypeSameNameWriter CreateWriter(Entity entity, EntityId entityId)
        {
            return new ComponentUsingNestedTypeSameNameWriter(World, entity, entityId);
        }
    }

    public class ComponentUsingNestedTypeSameNameReader : Reader<ComponentUsingNestedTypeSameName.Component, ComponentUsingNestedTypeSameName.Update>
    {
        private Dictionary<Action<int>, ulong> nestedFieldUpdateCallbackToCallbackKey;

        private Dictionary<Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName>, ulong> other0FieldUpdateCallbackToCallbackKey;

        private Dictionary<Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName>, ulong> other1FieldUpdateCallbackToCallbackKey;

        internal ComponentUsingNestedTypeSameNameReader(World world, Entity entity, EntityId entityId) : base(world, entity, entityId)
        {
        }

        public event Action<int> OnNestedFieldUpdate
        {
            add
            {
                if (nestedFieldUpdateCallbackToCallbackKey == null)
                {
                    nestedFieldUpdateCallbackToCallbackKey = new Dictionary<Action<int>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ComponentUsingNestedTypeSameName.Update>(EntityId, update =>
                {
                    if (update.NestedField.HasValue)
                    {
                        value(update.NestedField.Value);
                    }
                });
                nestedFieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!nestedFieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                nestedFieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName> OnOther0FieldUpdate
        {
            add
            {
                if (other0FieldUpdateCallbackToCallbackKey == null)
                {
                    other0FieldUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other0.NestedTypeSameName>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ComponentUsingNestedTypeSameName.Update>(EntityId, update =>
                {
                    if (update.Other0Field.HasValue)
                    {
                        value(update.Other0Field.Value);
                    }
                });
                other0FieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!other0FieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                other0FieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName> OnOther1FieldUpdate
        {
            add
            {
                if (other1FieldUpdateCallbackToCallbackKey == null)
                {
                    other1FieldUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.NestedTypeSameName.Other.NestedTypeSameName.Other1.NestedTypeSameName>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ComponentUsingNestedTypeSameName.Update>(EntityId, update =>
                {
                    if (update.Other1Field.HasValue)
                    {
                        value(update.Other1Field.Value);
                    }
                });
                other1FieldUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!other1FieldUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                other1FieldUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        protected override void RemoveFieldCallbacks()
        {
            if (nestedFieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in nestedFieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                nestedFieldUpdateCallbackToCallbackKey.Clear();
            }

            if (other0FieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in other0FieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                other0FieldUpdateCallbackToCallbackKey.Clear();
            }

            if (other1FieldUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in other1FieldUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                other1FieldUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class ComponentUsingNestedTypeSameNameWriter : ComponentUsingNestedTypeSameNameReader
    {
        internal ComponentUsingNestedTypeSameNameWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(ComponentUsingNestedTypeSameName.Update update)
        {
            var component = EntityManager.GetComponentData<ComponentUsingNestedTypeSameName.Component>(Entity);

            if (update.NestedField.HasValue)
            {
                component.NestedField = update.NestedField.Value;
            }

            if (update.Other0Field.HasValue)
            {
                component.Other0Field = update.Other0Field.Value;
            }

            if (update.Other1Field.HasValue)
            {
                component.Other1Field = update.Other1Field.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, ComponentUsingNestedTypeSameName.ComponentId);
        }
    }
}
