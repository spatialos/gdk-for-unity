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
    public class ExhaustiveEntityReaderSubscriptionManager : ReaderSubscriptionManager<ExhaustiveEntity.Component, ExhaustiveEntityReader>
    {
        public ExhaustiveEntityReaderSubscriptionManager(World world) : base(world)
        {
        }

        protected override ExhaustiveEntityReader CreateReader(Entity entity, EntityId entityId)
        {
            return new ExhaustiveEntityReader(World, entity, entityId);
        }
    }

    [AutoRegisterSubscriptionManager]
    public class ExhaustiveEntityWriterSubscriptionManager : WriterSubscriptionManager<ExhaustiveEntity.Component, ExhaustiveEntityWriter>
    {
        public ExhaustiveEntityWriterSubscriptionManager(World world) : base(world)
        {
        }

        protected override ExhaustiveEntityWriter CreateWriter(Entity entity, EntityId entityId)
        {
            return new ExhaustiveEntityWriter(World, entity, entityId);
        }
    }

    public class ExhaustiveEntityReader : Reader<ExhaustiveEntity.Component, ExhaustiveEntity.Update>
    {
        private Dictionary<Action<global::Improbable.Gdk.Core.EntitySnapshot>, ulong> field1UpdateCallbackToCallbackKey;

        private Dictionary<Action<global::Improbable.Gdk.Core.EntitySnapshot?>, ulong> field2UpdateCallbackToCallbackKey;

        private Dictionary<Action<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>, ulong> field3UpdateCallbackToCallbackKey;

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>>, ulong> field4UpdateCallbackToCallbackKey;

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>>, ulong> field5UpdateCallbackToCallbackKey;

        internal ExhaustiveEntityReader(World world, Entity entity, EntityId entityId) : base(world, entity, entityId)
        {
        }

        public event Action<global::Improbable.Gdk.Core.EntitySnapshot> OnField1Update
        {
            add
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot add field update callback when Reader is not valid.");
                }

                if (field1UpdateCallbackToCallbackKey == null)
                {
                    field1UpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Core.EntitySnapshot>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field1.HasValue)
                    {
                        value(update.Field1.Value);
                    }
                });
                field1UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field1UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field1UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::Improbable.Gdk.Core.EntitySnapshot?> OnField2Update
        {
            add
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot add field update callback when Reader is not valid.");
                }

                if (field2UpdateCallbackToCallbackKey == null)
                {
                    field2UpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Core.EntitySnapshot?>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field2.HasValue)
                    {
                        value(update.Field2.Value);
                    }
                });
                field2UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field2UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field2UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>> OnField3Update
        {
            add
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot add field update callback when Reader is not valid.");
                }

                if (field3UpdateCallbackToCallbackKey == null)
                {
                    field3UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.List<global::Improbable.Gdk.Core.EntitySnapshot>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field3.HasValue)
                    {
                        value(update.Field3.Value);
                    }
                });
                field3UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field3UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field3UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>> OnField4Update
        {
            add
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot add field update callback when Reader is not valid.");
                }

                if (field4UpdateCallbackToCallbackKey == null)
                {
                    field4UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.Gdk.Core.EntitySnapshot, string>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field4.HasValue)
                    {
                        value(update.Field4.Value);
                    }
                });
                field4UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field4UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field4UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>> OnField5Update
        {
            add
            {
                if (!IsValid)
                {
                    throw new InvalidOperationException("Cannot add field update callback when Reader is not valid.");
                }

                if (field5UpdateCallbackToCallbackKey == null)
                {
                    field5UpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<string, global::Improbable.Gdk.Core.EntitySnapshot>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<ExhaustiveEntity.Update>(EntityId, update =>
                {
                    if (update.Field5.HasValue)
                    {
                        value(update.Field5.Value);
                    }
                });
                field5UpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!field5UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                field5UpdateCallbackToCallbackKey.Remove(value);
            }
        }

        protected override void RemoveFieldCallbacks()
        {
            if (field1UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field1UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field1UpdateCallbackToCallbackKey.Clear();
            }

            if (field2UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field2UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field2UpdateCallbackToCallbackKey.Clear();
            }

            if (field3UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field3UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field3UpdateCallbackToCallbackKey.Clear();
            }

            if (field4UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field4UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field4UpdateCallbackToCallbackKey.Clear();
            }

            if (field5UpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in field5UpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                field5UpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class ExhaustiveEntityWriter : ExhaustiveEntityReader
    {
        internal ExhaustiveEntityWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(ExhaustiveEntity.Update update)
        {
            var component = EntityManager.GetComponentData<ExhaustiveEntity.Component>(Entity);

            if (update.Field1.HasValue)
            {
                component.Field1 = update.Field1.Value;
            }

            if (update.Field2.HasValue)
            {
                component.Field2 = update.Field2.Value;
            }

            if (update.Field3.HasValue)
            {
                component.Field3 = update.Field3.Value;
            }

            if (update.Field4.HasValue)
            {
                component.Field4 = update.Field4.Value;
            }

            if (update.Field5.HasValue)
            {
                component.Field5 = update.Field5.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, ExhaustiveEntity.ComponentId);
        }
    }
}
