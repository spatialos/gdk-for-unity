// =====================================================
// DO NOT EDIT - this file is automatically regenerated.
// =====================================================

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Improbable.DependentSchema
{
    [AutoRegisterSubscriptionManager]
    public class DependentDataComponentCommandSenderSubscriptionManager : SubscriptionManager<DependentDataComponentCommandSender>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<DependentDataComponentCommandSender>>>
            entityIdToSenderSubscriptions =
                new Dictionary<EntityId, HashSet<Subscription<DependentDataComponentCommandSender>>>();

        public DependentDataComponentCommandSenderSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();

            var constraintSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintSystem.RegisterEntityAddedCallback(entityId =>
            {
                if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
                {
                    return;
                }

                workerSystem.TryGetEntity(entityId, out var entity);
                foreach (var subscription in subscriptions)
                {
                    if (!subscription.HasValue)
                    {
                        subscription.SetAvailable(new DependentDataComponentCommandSender(entity, world));
                    }
                }
            });

            constraintSystem.RegisterEntityRemovedCallback(entityId =>
            {
                if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
                {
                    return;
                }

                foreach (var subscription in subscriptions)
                {
                    if (subscription.HasValue)
                    {
                        ResetValue(subscription);
                        subscription.SetUnavailable();
                    }
                }
            });
        }

        public override Subscription<DependentDataComponentCommandSender> Subscribe(EntityId entityId)
        {
            if (entityIdToSenderSubscriptions == null)
            {
                entityIdToSenderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DependentDataComponentCommandSender>>>();
            }

            if (entityId.Id < 0)
            {
                throw new ArgumentException("EntityId can not be < 0");
            }

            var subscription = new Subscription<DependentDataComponentCommandSender>(this, entityId);

            if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DependentDataComponentCommandSender>>();
                entityIdToSenderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity))
            {
                subscription.SetAvailable(new DependentDataComponentCommandSender(entity, world));
            }
            else if (entityId.Id == 0)
            {
                subscription.SetAvailable(new DependentDataComponentCommandSender(Entity.Null, world));
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<DependentDataComponentCommandSender>) subscription);
            if (sub.HasValue)
            {
                var sender = sub.Value;
                sender.IsValid = false;
            }

            var subscriptions = entityIdToSenderSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToSenderSubscriptions.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<DependentDataComponentCommandSender>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    [AutoRegisterSubscriptionManager]
    public class DependentDataComponentCommandReceiverSubscriptionManager : SubscriptionManager<DependentDataComponentCommandReceiver>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<DependentDataComponentCommandReceiver>>> entityIdToReceiveSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public DependentDataComponentCommandReceiverSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintSystem.RegisterAuthorityCallback(DependentDataComponent.ComponentId, authorityChange =>
            {
                if (authorityChange.Authority == Authority.Authoritative)
                {
                    if (!entitiesNotMatchingRequirements.Contains(authorityChange.EntityId))
                    {
                        return;
                    }

                    workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

                    foreach (var subscription in entityIdToReceiveSubscriptions[authorityChange.EntityId])
                    {
                        subscription.SetAvailable(new DependentDataComponentCommandReceiver(world, entity, authorityChange.EntityId));
                    }

                    entitiesMatchingRequirements.Add(authorityChange.EntityId);
                    entitiesNotMatchingRequirements.Remove(authorityChange.EntityId);
                }
                else if (authorityChange.Authority == Authority.NotAuthoritative)
                {
                    if (!entitiesMatchingRequirements.Contains(authorityChange.EntityId))
                    {
                        return;
                    }

                    workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

                    foreach (var subscription in entityIdToReceiveSubscriptions[authorityChange.EntityId])
                    {
                        ResetValue(subscription);
                        subscription.SetUnavailable();
                    }

                    entitiesNotMatchingRequirements.Add(authorityChange.EntityId);
                    entitiesMatchingRequirements.Remove(authorityChange.EntityId);
                }
            });
        }

        public override Subscription<DependentDataComponentCommandReceiver> Subscribe(EntityId entityId)
        {
            if (entityIdToReceiveSubscriptions == null)
            {
                entityIdToReceiveSubscriptions = new Dictionary<EntityId, HashSet<Subscription<DependentDataComponentCommandReceiver>>>();
            }

            var subscription = new Subscription<DependentDataComponentCommandReceiver>(this, entityId);

            if (!entityIdToReceiveSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<DependentDataComponentCommandReceiver>>();
                entityIdToReceiveSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(DependentDataComponent.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, DependentDataComponent.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new DependentDataComponentCommandReceiver(world, entity, entityId));
            }
            else
            {
                entitiesNotMatchingRequirements.Add(entityId);
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<DependentDataComponentCommandReceiver>) subscription);
            if (sub.HasValue)
            {
                var receiver = sub.Value;
                receiver.IsValid = false;
                receiver.RemoveAllCallbacks();
            }

            var subscriptions = entityIdToReceiveSubscriptions[sub.EntityId];
            subscriptions.Remove(sub);
            if (subscriptions.Count == 0)
            {
                entityIdToReceiveSubscriptions.Remove(sub.EntityId);
                entitiesMatchingRequirements.Remove(sub.EntityId);
                entitiesNotMatchingRequirements.Remove(sub.EntityId);
            }
        }

        public override void ResetValue(ISubscription subscription)
        {
            var sub = ((Subscription<DependentDataComponentCommandReceiver>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class DependentDataComponentCommandSender
    {
        public bool IsValid;

        private readonly Entity entity;
        private readonly CommandSystem commandSender;
        private readonly CommandCallbackSystem callbackSystem;

        private int callbackEpoch;

        internal DependentDataComponentCommandSender(Entity entity, World world)
        {
            this.entity = entity;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            // todo check that this exists
            commandSender = world.GetExistingSystem<CommandSystem>();

            IsValid = true;
        }

        public void SendBarCommandCommand(EntityId targetEntityId, global::Improbable.TestSchema.SomeType request, Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedResponse> callback = null)
        {
            var commandRequest = new DependentDataComponent.BarCommand.Request(targetEntityId, request);
            SendBarCommandCommand(commandRequest, callback);
        }

        public void SendBarCommandCommand(global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Request request, Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedResponse> wrappedCallback = response =>
                {
                    if (!this.IsValid || validCallbackEpoch != this.callbackEpoch)
                    {
                        return;
                    }

                    callback(response);
                };
                callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
            }
        }

        public void RemoveAllCallbacks()
        {
            ++callbackEpoch;
        }
    }

    public class DependentDataComponentCommandReceiver
    {
        public bool IsValid;

        private readonly EntityId entityId;
        private readonly CommandCallbackSystem callbackSystem;
        private readonly CommandSystem commandSystem;

        private Dictionary<Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedRequest>, ulong> barCommandCallbackToCallbackKey;

        public event Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedRequest> OnBarCommandRequestReceived
        {
            add
            {
                if (barCommandCallbackToCallbackKey == null)
                {
                    barCommandCallbackToCallbackKey = new Dictionary<Action<global::Improbable.DependentSchema.DependentDataComponent.BarCommand.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                barCommandCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!barCommandCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                barCommandCallbackToCallbackKey.Remove(value);
            }
        }

        internal DependentDataComponentCommandReceiver(World world, Entity entity, EntityId entityId)
        {
            this.entityId = entityId;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            commandSystem = world.GetExistingSystem<CommandSystem>();
            // should check the system actually exists

            IsValid = true;
        }

        public void SendBarCommandResponse(global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendBarCommandResponse(long requestId, global::Improbable.TestSchema.SomeType response)
        {
            commandSystem.SendResponse(new global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Response(requestId, response));
        }

        public void SendBarCommandFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Improbable.DependentSchema.DependentDataComponent.BarCommand.Response(requestId, failureMessage));
        }

        public void RemoveAllCallbacks()
        {
            if (barCommandCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in barCommandCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                barCommandCallbackToCallbackKey.Clear();
            }

        }
    }
}
