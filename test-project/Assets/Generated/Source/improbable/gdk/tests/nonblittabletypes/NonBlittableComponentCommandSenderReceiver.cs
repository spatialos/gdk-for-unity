// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    [AutoRegisterSubscriptionManager]
    public class NonBlittableComponentCommandSenderSubscriptionManager : SubscriptionManager<NonBlittableComponentCommandSender>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentCommandSender>>>
            entityIdToSenderSubscriptions =
                new Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentCommandSender>>>();

        public NonBlittableComponentCommandSenderSubscriptionManager(World world)
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
                        subscription.SetAvailable(new NonBlittableComponentCommandSender(entity, world));
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

        public override Subscription<NonBlittableComponentCommandSender> Subscribe(EntityId entityId)
        {
            if (entityIdToSenderSubscriptions == null)
            {
                entityIdToSenderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentCommandSender>>>();
            }

            if (entityId.Id < 0)
            {
                throw new ArgumentException("EntityId can not be < 0");
            }

            var subscription = new Subscription<NonBlittableComponentCommandSender>(this, entityId);

            if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<NonBlittableComponentCommandSender>>();
                entityIdToSenderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity))
            {
                subscription.SetAvailable(new NonBlittableComponentCommandSender(entity, world));
            }
            else if (entityId.Id == 0)
            {
                subscription.SetAvailable(new NonBlittableComponentCommandSender(Entity.Null, world));
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<NonBlittableComponentCommandSender>) subscription);
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
            var sub = ((Subscription<NonBlittableComponentCommandSender>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    [AutoRegisterSubscriptionManager]
    public class NonBlittableComponentCommandReceiverSubscriptionManager : SubscriptionManager<NonBlittableComponentCommandReceiver>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentCommandReceiver>>> entityIdToReceiveSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public NonBlittableComponentCommandReceiverSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingSystem<WorkerSystem>();
            componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

            var constraintSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

            constraintSystem.RegisterAuthorityCallback(NonBlittableComponent.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new NonBlittableComponentCommandReceiver(world, entity, authorityChange.EntityId));
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

        public override Subscription<NonBlittableComponentCommandReceiver> Subscribe(EntityId entityId)
        {
            if (entityIdToReceiveSubscriptions == null)
            {
                entityIdToReceiveSubscriptions = new Dictionary<EntityId, HashSet<Subscription<NonBlittableComponentCommandReceiver>>>();
            }

            var subscription = new Subscription<NonBlittableComponentCommandReceiver>(this, entityId);

            if (!entityIdToReceiveSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<NonBlittableComponentCommandReceiver>>();
                entityIdToReceiveSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(NonBlittableComponent.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, NonBlittableComponent.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new NonBlittableComponentCommandReceiver(world, entity, entityId));
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
            var sub = ((Subscription<NonBlittableComponentCommandReceiver>) subscription);
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
            var sub = ((Subscription<NonBlittableComponentCommandReceiver>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class NonBlittableComponentCommandSender
    {
        public bool IsValid;

        private readonly Entity entity;
        private readonly CommandSystem commandSender;
        private readonly CommandCallbackSystem callbackSystem;

        private int callbackEpoch;

        internal NonBlittableComponentCommandSender(Entity entity, World world)
        {
            this.entity = entity;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            // todo check that this exists
            commandSender = world.GetExistingSystem<CommandSystem>();

            IsValid = true;
        }

        public void SendFirstCommandCommand(EntityId targetEntityId, global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest request, Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse> callback = null)
        {
            var commandRequest = new NonBlittableComponent.FirstCommand.Request(targetEntityId, request);
            SendFirstCommandCommand(commandRequest, callback);
        }

        public void SendFirstCommandCommand(global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request request, Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse> wrappedCallback = response =>
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
        public void SendSecondCommandCommand(EntityId targetEntityId, global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest request, Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse> callback = null)
        {
            var commandRequest = new NonBlittableComponent.SecondCommand.Request(targetEntityId, request);
            SendSecondCommandCommand(commandRequest, callback);
        }

        public void SendSecondCommandCommand(global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request request, Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse> wrappedCallback = response =>
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

    public class NonBlittableComponentCommandReceiver
    {
        public bool IsValid;

        private readonly EntityId entityId;
        private readonly CommandCallbackSystem callbackSystem;
        private readonly CommandSystem commandSystem;

        private Dictionary<Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest>, ulong> firstCommandCallbackToCallbackKey;

        public event Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest> OnFirstCommandRequestReceived
        {
            add
            {
                if (firstCommandCallbackToCallbackKey == null)
                {
                    firstCommandCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                firstCommandCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!firstCommandCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                firstCommandCallbackToCallbackKey.Remove(value);
            }
        }
        private Dictionary<Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest>, ulong> secondCommandCallbackToCallbackKey;

        public event Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest> OnSecondCommandRequestReceived
        {
            add
            {
                if (secondCommandCallbackToCallbackKey == null)
                {
                    secondCommandCallbackToCallbackKey = new Dictionary<Action<global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                secondCommandCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!secondCommandCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                secondCommandCallbackToCallbackKey.Remove(value);
            }
        }

        internal NonBlittableComponentCommandReceiver(World world, Entity entity, EntityId entityId)
        {
            this.entityId = entityId;
            callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
            commandSystem = world.GetExistingSystem<CommandSystem>();
            // should check the system actually exists

            IsValid = true;
        }

        public void SendFirstCommandResponse(global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendFirstCommandResponse(long requestId, global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse response)
        {
            commandSystem.SendResponse(new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response(requestId, response));
        }

        public void SendFirstCommandFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response(requestId, failureMessage));
        }

        public void SendSecondCommandResponse(global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendSecondCommandResponse(long requestId, global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse response)
        {
            commandSystem.SendResponse(new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response(requestId, response));
        }

        public void SendSecondCommandFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response(requestId, failureMessage));
        }

        public void RemoveAllCallbacks()
        {
            if (firstCommandCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in firstCommandCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                firstCommandCallbackToCallbackKey.Clear();
            }

            if (secondCommandCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in secondCommandCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                secondCommandCallbackToCallbackKey.Clear();
            }

        }
    }
}
