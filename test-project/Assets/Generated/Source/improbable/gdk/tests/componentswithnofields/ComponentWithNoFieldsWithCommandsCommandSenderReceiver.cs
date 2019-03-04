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

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    [AutoRegisterSubscriptionManager]
    public class ComponentWithNoFieldsWithCommandsCommandSenderSubscriptionManager : SubscriptionManager<ComponentWithNoFieldsWithCommandsCommandSender>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;

        private Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithCommandsCommandSender>>>
            entityIdToSenderSubscriptions =
                new Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithCommandsCommandSender>>>();

        public ComponentWithNoFieldsWithCommandsCommandSenderSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingManager<WorkerSystem>();

            var constraintSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();

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
                        subscription.SetAvailable(new ComponentWithNoFieldsWithCommandsCommandSender(entity, world));
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

        public override Subscription<ComponentWithNoFieldsWithCommandsCommandSender> Subscribe(EntityId entityId)
        {
            if (entityIdToSenderSubscriptions == null)
            {
                entityIdToSenderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithCommandsCommandSender>>>();
            }

            if (entityId.Id < 0)
            {
                throw new ArgumentException("EntityId can not be < 0");
            }

            var subscription = new Subscription<ComponentWithNoFieldsWithCommandsCommandSender>(this, entityId);

            if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ComponentWithNoFieldsWithCommandsCommandSender>>();
                entityIdToSenderSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity))
            {
                subscription.SetAvailable(new ComponentWithNoFieldsWithCommandsCommandSender(entity, world));
            }
            else if (entityId.Id == 0)
            {
                subscription.SetAvailable(new ComponentWithNoFieldsWithCommandsCommandSender(Entity.Null, world));
            }

            subscriptions.Add(subscription);
            return subscription;
        }

        public override void Cancel(ISubscription subscription)
        {
            var sub = ((Subscription<ComponentWithNoFieldsWithCommandsCommandSender>) subscription);
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
            var sub = ((Subscription<ComponentWithNoFieldsWithCommandsCommandSender>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    [AutoRegisterSubscriptionManager]
    public class ComponentWithNoFieldsWithCommandsCommandReceiverSubscriptionManager : SubscriptionManager<ComponentWithNoFieldsWithCommandsCommandReceiver>
    {
        private readonly World world;
        private readonly WorkerSystem workerSystem;
        private readonly ComponentUpdateSystem componentUpdateSystem;

        private Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithCommandsCommandReceiver>>> entityIdToReceiveSubscriptions;

        private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
        private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

        public ComponentWithNoFieldsWithCommandsCommandReceiverSubscriptionManager(World world)
        {
            this.world = world;

            // Check that these are there
            workerSystem = world.GetExistingManager<WorkerSystem>();
            componentUpdateSystem = world.GetExistingManager<ComponentUpdateSystem>();

            var constraintSystem = world.GetExistingManager<ComponentConstraintsCallbackSystem>();

            constraintSystem.RegisterAuthorityCallback(ComponentWithNoFieldsWithCommands.ComponentId, authorityChange =>
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
                        subscription.SetAvailable(new ComponentWithNoFieldsWithCommandsCommandReceiver(world, entity, authorityChange.EntityId));
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

        public override Subscription<ComponentWithNoFieldsWithCommandsCommandReceiver> Subscribe(EntityId entityId)
        {
            if (entityIdToReceiveSubscriptions == null)
            {
                entityIdToReceiveSubscriptions = new Dictionary<EntityId, HashSet<Subscription<ComponentWithNoFieldsWithCommandsCommandReceiver>>>();
            }

            var subscription = new Subscription<ComponentWithNoFieldsWithCommandsCommandReceiver>(this, entityId);

            if (!entityIdToReceiveSubscriptions.TryGetValue(entityId, out var subscriptions))
            {
                subscriptions = new HashSet<Subscription<ComponentWithNoFieldsWithCommandsCommandReceiver>>();
                entityIdToReceiveSubscriptions.Add(entityId, subscriptions);
            }

            if (workerSystem.TryGetEntity(entityId, out var entity)
                && componentUpdateSystem.HasComponent(ComponentWithNoFieldsWithCommands.ComponentId, entityId)
                && componentUpdateSystem.GetAuthority(entityId, ComponentWithNoFieldsWithCommands.ComponentId) != Authority.NotAuthoritative)
            {
                entitiesMatchingRequirements.Add(entityId);
                subscription.SetAvailable(new ComponentWithNoFieldsWithCommandsCommandReceiver(world, entity, entityId));
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
            var sub = ((Subscription<ComponentWithNoFieldsWithCommandsCommandReceiver>) subscription);
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
            var sub = ((Subscription<ComponentWithNoFieldsWithCommandsCommandReceiver>) subscription);
            if (sub.HasValue)
            {
                sub.Value.RemoveAllCallbacks();
            }
        }
    }

    public class ComponentWithNoFieldsWithCommandsCommandSender
    {
        public bool IsValid;

        private readonly Entity entity;
        private readonly CommandSystem commandSender;
        private readonly CommandCallbackSystem callbackSystem;

        private int callbackEpoch;

        internal ComponentWithNoFieldsWithCommandsCommandSender(Entity entity, World world)
        {
            this.entity = entity;
            callbackSystem = world.GetOrCreateManager<CommandCallbackSystem>();
            // todo check that this exists
            commandSender = world.GetExistingManager<CommandSystem>();

            IsValid = true;
        }

        public void SendCmdCommand(EntityId targetEntityId, global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty request, Action<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse> callback = null)
        {
            var commandRequest = new ComponentWithNoFieldsWithCommands.Cmd.Request(targetEntityId, request);
            SendCmdCommand(commandRequest, callback);
        }

        public void SendCmdCommand(ComponentWithNoFieldsWithCommands.Cmd.Request request, Action<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse> callback = null)
        {
            int validCallbackEpoch = callbackEpoch;
            var requestId = commandSender.SendCommand(request, entity);
            if (callback != null)
            {
                Action<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse> wrappedCallback = response =>
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

    public class ComponentWithNoFieldsWithCommandsCommandReceiver
    {
        public bool IsValid;

        private readonly EntityId entityId;
        private readonly CommandCallbackSystem callbackSystem;
        private readonly CommandSystem commandSystem;

        private Dictionary<Action<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest>, ulong> cmdCallbackToCallbackKey;

        public event Action<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest> OnCmdRequestReceived
        {
            add
            {
                if (cmdCallbackToCallbackKey == null)
                {
                    cmdCallbackToCallbackKey = new Dictionary<Action<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest>, ulong>();
                }

                var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
                cmdCallbackToCallbackKey.Add(value, key);
            }
            remove
            {
                if (!cmdCallbackToCallbackKey.TryGetValue(value, out var key))
                {
                    return;
                }

                callbackSystem.UnregisterCommandRequestCallback(key);
                cmdCallbackToCallbackKey.Remove(value);
            }
        }

        internal ComponentWithNoFieldsWithCommandsCommandReceiver(World world, Entity entity, EntityId entityId)
        {
            this.entityId = entityId;
            callbackSystem = world.GetOrCreateManager<CommandCallbackSystem>();
            commandSystem = world.GetExistingManager<CommandSystem>();
            // should check the system actually exists

            IsValid = true;
        }

        public void SendCmdResponse(Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Response response)
        {
            commandSystem.SendResponse(response);
        }

        public void SendCmdResponse(long requestId, global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty response)
        {
            commandSystem.SendResponse(new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Response(requestId, response));
        }

        public void SendCmdFailure(long requestId, string failureMessage)
        {
            commandSystem.SendResponse(new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Response(requestId, failureMessage));
        }

        public void RemoveAllCallbacks()
        {
            if (cmdCallbackToCallbackKey != null)
            {
                foreach (var callbackToKey in cmdCallbackToCallbackKey)
                {
                    callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
                }

                cmdCallbackToCallbackKey.Clear();
            }

        }
    }
}
