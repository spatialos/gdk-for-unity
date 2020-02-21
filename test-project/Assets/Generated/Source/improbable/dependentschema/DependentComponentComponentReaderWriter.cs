// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using Entity = Unity.Entities.Entity;

namespace Improbable.DependentSchema
{
    [AutoRegisterSubscriptionManager]
    public class DependentComponentReaderSubscriptionManager : ReaderSubscriptionManager<DependentComponent.Component, DependentComponentReader>
    {
        public DependentComponentReaderSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependentComponentReader CreateReader(Entity entity, EntityId entityId)
        {
            return new DependentComponentReader(world, entity, entityId);
        }
    }

    [AutoRegisterSubscriptionManager]
    public class DependentComponentWriterSubscriptionManager : WriterSubscriptionManager<DependentComponent.Component, DependentComponentWriter>
    {
        public DependentComponentWriterSubscriptionManager(World world) : base(world)
        {
        }

        protected override DependentComponentWriter CreateWriter(Entity entity, EntityId entityId)
        {
            return new DependentComponentWriter(world, entity, entityId);
        }
    }

    public class DependentComponentReader : Reader<DependentComponent.Component, DependentComponent.Update>
    {
        private Dictionary<Action<global::Improbable.TestSchema.ExhaustiveRepeatedData>, ulong> aUpdateCallbackToCallbackKey;

        private Dictionary<Action<global::Improbable.TestSchema.SomeEnum>, ulong> bUpdateCallbackToCallbackKey;

        private Dictionary<Action<global::Improbable.TestSchema.SomeEnum?>, ulong> cUpdateCallbackToCallbackKey;

        private Dictionary<Action<global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>>, ulong> dUpdateCallbackToCallbackKey;

        private Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>>, ulong> eUpdateCallbackToCallbackKey;

        internal DependentComponentReader(World world, Entity entity, EntityId entityId) : base(world, entity, entityId)
        {
        }

        public event Action<global::Improbable.TestSchema.ExhaustiveRepeatedData> OnAUpdate
        {
            add
            {
                if (aUpdateCallbackToCallbackKey == null)
                {
                    aUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.ExhaustiveRepeatedData>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
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

        public event Action<global::Improbable.TestSchema.SomeEnum> OnBUpdate
        {
            add
            {
                if (bUpdateCallbackToCallbackKey == null)
                {
                    bUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.SomeEnum>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
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

        public event Action<global::Improbable.TestSchema.SomeEnum?> OnCUpdate
        {
            add
            {
                if (cUpdateCallbackToCallbackKey == null)
                {
                    cUpdateCallbackToCallbackKey = new Dictionary<Action<global::Improbable.TestSchema.SomeEnum?>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
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

        public event Action<global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>> OnDUpdate
        {
            add
            {
                if (dUpdateCallbackToCallbackKey == null)
                {
                    dUpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.List<global::Improbable.TestSchema.SomeType>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
                {
                    if (update.D.HasValue)
                    {
                        value(update.D.Value);
                    }
                });
                dUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!dUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                dUpdateCallbackToCallbackKey.Remove(value);
            }
        }

        public event Action<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>> OnEUpdate
        {
            add
            {
                if (eUpdateCallbackToCallbackKey == null)
                {
                    eUpdateCallbackToCallbackKey = new Dictionary<Action<global::System.Collections.Generic.Dictionary<global::Improbable.TestSchema.SomeEnum, global::Improbable.TestSchema.SomeType>>, ulong>();
                }

                var key = CallbackSystem.RegisterComponentUpdateCallback<DependentComponent.Update>(EntityId, update =>
                {
                    if (update.E.HasValue)
                    {
                        value(update.E.Value);
                    }
                });
                eUpdateCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!eUpdateCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                CallbackSystem.UnregisterCallback(key);
                eUpdateCallbackToCallbackKey.Remove(value);
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

            if (dUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in dUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                dUpdateCallbackToCallbackKey.Clear();
            }

            if (eUpdateCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in eUpdateCallbackToCallbackKey)
                {
                    CallbackSystem.UnregisterCallback(callbackToKey.Value);
                }

                eUpdateCallbackToCallbackKey.Clear();
            }
        }
    }

    public class DependentComponentWriter : DependentComponentReader
    {
        internal DependentComponentWriter(World world, Entity entity, EntityId entityId)
            : base(world, entity, entityId)
        {
        }

        public void SendUpdate(DependentComponent.Update update)
        {
            var component = EntityManager.GetComponentData<DependentComponent.Component>(Entity);

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

            if (update.D.HasValue)
            {
                component.D = update.D.Value;
            }

            if (update.E.HasValue)
            {
                component.E = update.E.Value;
            }

            EntityManager.SetComponentData(Entity, component);
        }

        public void AcknowledgeAuthorityLoss()
        {
            ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, DependentComponent.ComponentId);
        }
    }
}
