// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Collections;
using Improbable.Worker.Core;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 1001;

            private readonly EntityManager entityManager;

            private const string LoggerName = "BlittableComponent.DispatcherHandler";

            private CommandStorages.FirstCommand firstCommandStorage;
            private CommandStorages.SecondCommand secondCommandStorage;

            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                firstCommandStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.FirstCommand>();
                secondCommandStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.SecondCommand>();
            }

            public override void Dispose()
            {
                BlittableComponent.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);

                BlittableComponent.ReferenceTypeProviders.FirstEventProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.SecondEventProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.CleanDataInWorld(World);
                BlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("BlittableComponent");
                var data = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>());

                var update = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update
                {
                    BoolField = data.BoolField,
                    IntField = data.IntField,
                    LongField = data.LongField,
                    FloatField = data.FloatField,
                    DoubleField = data.DoubleField,
                };

                var updates = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.BlittableTypes.BlittableComponent")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("BlittableComponent");

                entityManager.RemoveComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.BlittableTypes.BlittableComponent")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("BlittableComponent");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>(entity);
                    Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.DirtyBit = false;
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update> updates;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update.Pool.Count > 0 ? Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update.Pool.Pop() : new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update>();
                    var updatesComponent = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates
                    {
                        handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                    };
                    ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                    entityManager.AddComponentData(entity, updatesComponent);
                }

                updates.Add(update);

                var eventsObject = op.Update.SchemaData.Value.GetEvents();
                {
                    var eventCount = eventsObject.GetObjectCount(1);
                    if (eventCount > 0)
                    {
                        // Create component to hold received events
                        ReceivedEvents.FirstEvent eventsReceived;
                        List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload> eventList;
                        if (!entityManager.HasComponent<ReceivedEvents.FirstEvent>(entity))
                        {
                            eventsReceived = new ReceivedEvents.FirstEvent() {
                                handle = ReferenceTypeProviders.FirstEventProvider.Allocate(World)
                            };
                            eventList = new List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>((int) eventCount);
                            ReferenceTypeProviders.FirstEventProvider.Set(eventsReceived.handle, eventList);
                            entityManager.AddComponentData(entity, eventsReceived);
                        }
                        else
                        {
                            eventsReceived = entityManager.GetComponentData<ReceivedEvents.FirstEvent>(entity);
                            eventList = eventsReceived.Events;
                        }

                        // Deserialize events onto component
                        for (var i = 0; i < eventCount; i++)
                        {
                            var e = global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload.Serialization.Deserialize(eventsObject.GetObject(1));
                            eventList.Add(e);
                        }
                    }
                }

                {
                    var eventCount = eventsObject.GetObjectCount(2);
                    if (eventCount > 0)
                    {
                        // Create component to hold received events
                        ReceivedEvents.SecondEvent eventsReceived;
                        List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload> eventList;
                        if (!entityManager.HasComponent<ReceivedEvents.SecondEvent>(entity))
                        {
                            eventsReceived = new ReceivedEvents.SecondEvent() {
                                handle = ReferenceTypeProviders.SecondEventProvider.Allocate(World)
                            };
                            eventList = new List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>((int) eventCount);
                            ReferenceTypeProviders.SecondEventProvider.Set(eventsReceived.handle, eventList);
                            entityManager.AddComponentData(entity, eventsReceived);
                        }
                        else
                        {
                            eventsReceived = entityManager.GetComponentData<ReceivedEvents.SecondEvent>(entity);
                            eventList = eventsReceived.Events;
                        }

                        // Deserialize events onto component
                        for (var i = 0; i < eventCount; i++)
                        {
                            var e = global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload.Serialization.Deserialize(eventsObject.GetObject(2));
                            eventList.Add(e);
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("BlittableComponent");
                ApplyAuthorityChange(entity, op.Authority, op.EntityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();

                Profiler.BeginSample("BlittableComponent");
                switch (commandIndex)
                {
                    case 1:
                        OnFirstCommandRequest(op);
                        break;
                    case 2:
                        OnSecondCommandRequest(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "BlittableComponent");
                }

                Profiler.EndSample();
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.Response.CommandIndex;

                Profiler.BeginSample("BlittableComponent");
                switch (commandIndex)
                {
                    case 1:
                        OnFirstCommandResponse(op);
                        break;
                    case 2:
                        OnSecondCommandResponse(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "BlittableComponent");
                }

                Profiler.EndSample();
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
                {
                    var commandSender = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.FirstCommand();
                    commandSender.CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Allocate(World);
                    commandSender.RequestsToSend = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Request>();

                    entityManager.AddComponentData(entity, commandSender);

                    var commandResponder = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.FirstCommand();
                    commandResponder.CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Allocate(World);
                    commandResponder.ResponsesToSend = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.Response>();

                    entityManager.AddComponentData(entity, commandResponder);
                }
                {
                    var commandSender = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.SecondCommand();
                    commandSender.CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Allocate(World);
                    commandSender.RequestsToSend = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Request>();

                    entityManager.AddComponentData(entity, commandSender);

                    var commandResponder = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.SecondCommand();
                    commandResponder.CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Allocate(World);
                    commandResponder.ResponsesToSend = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.Response>();

                    entityManager.AddComponentData(entity, commandResponder);
                }
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Worker.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>());

                        // Add event senders
                        {
                            var eventSender = new EventSender.FirstEvent()
                            {
                                handle = ReferenceTypeProviders.FirstEventProvider.Allocate(World)
                            };
                            ReferenceTypeProviders.FirstEventProvider.Set(eventSender.handle, new List<global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload>());
                            entityManager.AddComponentData(entity, eventSender);
                        }
                        {
                            var eventSender = new EventSender.SecondEvent()
                            {
                                handle = ReferenceTypeProviders.SecondEventProvider.Allocate(World)
                            };
                            ReferenceTypeProviders.SecondEventProvider.Set(eventSender.handle, new List<global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload>());
                            entityManager.AddComponentData(entity, eventSender);
                        }
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>());

                        // Remove event senders
                        {
                            var eventSender = entityManager.GetComponentData<EventSender.FirstEvent>(entity);
                            ReferenceTypeProviders.FirstEventProvider.Free(eventSender.handle);
                            entityManager.RemoveComponent<EventSender.FirstEvent>(entity);
                        }
                        {
                            var eventSender = entityManager.GetComponentData<EventSender.SecondEvent>(entity);
                            ReferenceTypeProviders.SecondEventProvider.Free(eventSender.handle);
                            entityManager.RemoveComponent<EventSender.SecondEvent>(entity);
                        }
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>
                    {
                        Handle = AuthorityChangesProvider.Allocate(World)
                    };
                    AuthorityChangesProvider.Set(changes.Handle, new List<Authority>());
                    authorityChanges = changes.Changes;
                    entityManager.AddComponentData(entity, changes);
                }

                authorityChanges.Add(authority);
            }

            private void LogInvalidAuthorityTransition(Authority newAuthority, Authority expectedOldAuthority, global::Improbable.Worker.EntityId entityId)
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(InvalidAuthorityChange)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId.Id)
                    .WithField("New Authority", newAuthority)
                    .WithField("Expected Old Authority", expectedOldAuthority)
                    .WithField("Component", "Improbable.Gdk.Tests.BlittableTypes.BlittableComponent")
                );
            }

            private void OnFirstCommandRequest(CommandRequestOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                var deserializedRequest = global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest> requests;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.FirstCommand>(entity))
                {
                    requests = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.FirstCommand>(entity).Requests;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.FirstCommand
                    {
                        CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Allocate(World)
                    };
                    requests = data.Requests = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest>();
                    entityManager.AddComponentData(entity, data);
                }

                requests.Add(new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedRequest(op.RequestId.Id,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest));
            }

            private void OnFirstCommandResponse(CommandResponseOp op)
            {
                if (!firstCommandStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out var requestBundle))
                {
                    throw new InvalidOperationException($"Could not find corresponding request for RequestId {op.RequestId.Id} and command FirstCommand.");
                }

                var entity = requestBundle.Entity;
                firstCommandStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);
                if (!entityManager.Exists(entity))
                {
                    LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("Op", "CommandResponseOp - FirstCommand")
                        .WithField("Component", "Improbable.Gdk.Tests.BlittableTypes.BlittableComponent")
                    );
                    return;
                }

                global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse? response = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    response = global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse> responses;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.FirstCommand>(entity))
                {
                    responses = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.FirstCommand>(entity).Responses;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.FirstCommand
                    {
                        CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Allocate(World)
                    };
                    responses = data.Responses = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse>();
                    entityManager.AddComponentData(entity, data);
                }

                responses.Add(new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.FirstCommand.ReceivedResponse(op.EntityId,
                    op.Message,
                    op.StatusCode,
                    response,
                    requestBundle.Request,
                    requestBundle.Context,
                    requestBundle.RequestId));
            }

            private void OnSecondCommandRequest(CommandRequestOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                var deserializedRequest = global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest> requests;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.SecondCommand>(entity))
                {
                    requests = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.SecondCommand>(entity).Requests;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandRequests.SecondCommand
                    {
                        CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Allocate(World)
                    };
                    requests = data.Requests = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest>();
                    entityManager.AddComponentData(entity, data);
                }

                requests.Add(new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedRequest(op.RequestId.Id,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest));
            }

            private void OnSecondCommandResponse(CommandResponseOp op)
            {
                if (!secondCommandStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out var requestBundle))
                {
                    throw new InvalidOperationException($"Could not find corresponding request for RequestId {op.RequestId.Id} and command SecondCommand.");
                }

                var entity = requestBundle.Entity;
                secondCommandStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);
                if (!entityManager.Exists(entity))
                {
                    LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("Op", "CommandResponseOp - SecondCommand")
                        .WithField("Component", "Improbable.Gdk.Tests.BlittableTypes.BlittableComponent")
                    );
                    return;
                }

                global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse? response = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    response = global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse> responses;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.SecondCommand>(entity))
                {
                    responses = entityManager.GetComponentData<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.SecondCommand>(entity).Responses;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponses.SecondCommand
                    {
                        CommandListHandle = Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Allocate(World)
                    };
                    responses = data.Responses = new List<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse>();
                    entityManager.AddComponentData(entity, data);
                }

                responses.Add(new Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.SecondCommand.ReceivedResponse(op.EntityId,
                    op.Message,
                    op.StatusCode,
                    response,
                    requestBundle.Request,
                    requestBundle.Context,
                    requestBundle.RequestId));
            }
        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 1001;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<EventSender.FirstEvent>(),
                    ComponentType.Create<EventSender.SecondEvent>(),
                    ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
            };

            public override EntityArchetypeQuery[] CommandQueries => new EntityArchetypeQuery[]
            {
                new EntityArchetypeQuery()
                {
                    All = new[]
                    {
                        ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.FirstCommand>(),
                        ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.FirstCommand>(),
                    },
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                },
                new EntityArchetypeQuery()
                {
                    All = new[]
                    {
                        ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.SecondCommand>(),
                        ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.SecondCommand>(),
                    },
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                },
            };

            private CommandStorages.FirstCommand firstCommandStorage;
            private CommandStorages.SecondCommand secondCommandStorage;

            public ComponentReplicator(EntityManager entityManager, Unity.Entities.World world) : base(entityManager)
            {
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                firstCommandStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.FirstCommand>();
                secondCommandStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.SecondCommand>();
            }

            public override void ExecuteReplication(ComponentGroup replicationGroup, ComponentSystemBase system, global::Improbable.Worker.Core.Connection connection)
            {
                Profiler.BeginSample("BlittableComponent");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.Temp);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>();
                var eventFirstEventType = system.GetArchetypeChunkComponentType<EventSender.FirstEvent>(true);
                var eventSecondEventType = system.GetArchetypeChunkComponentType<EventSender.SecondEvent>(true);
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var componentArray = chunk.GetNativeArray(componentType);
                    var eventFirstEventArray = chunk.GetNativeArray(eventFirstEventType);
                    var eventSecondEventArray = chunk.GetNativeArray(eventSecondEventType);
                    for (var i = 0; i < componentArray.Length; i++)
                    {
                        var data = componentArray[i];
                        var dirtyEvents = 0;
                        var eventsFirstEvent = eventFirstEventArray[i].Events;
                        dirtyEvents += eventsFirstEvent.Count;
                        var eventsSecondEvent = eventSecondEventArray[i].Events;
                        dirtyEvents += eventsSecondEvent.Count;

                        if (data.DirtyBit || dirtyEvents > 0)
                        {
                            var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(1001);
                            Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Serialization.SerializeUpdate(data, update);

                            // Serialize events
                            var eventsObject = update.GetEvents();
                            if (eventsFirstEvent.Count > 0)
                            {
                                foreach (var e in eventsFirstEvent)
                                {
                                    var obj = eventsObject.AddObject(1);
                                    global::Improbable.Gdk.Tests.BlittableTypes.FirstEventPayload.Serialization.Serialize(e, obj);
                                }

                                eventsFirstEvent.Clear();
                            }

                            if (eventsSecondEvent.Count > 0)
                            {
                                foreach (var e in eventsSecondEvent)
                                {
                                    var obj = eventsObject.AddObject(2);
                                    global::Improbable.Gdk.Tests.BlittableTypes.SecondEventPayload.Serialization.Serialize(e, obj);
                                }

                                eventsSecondEvent.Clear();
                            }

                            // Send serialized update over the wire
                            connection.SendComponentUpdate(entityIdArray[i].EntityId, new global::Improbable.Worker.Core.ComponentUpdate(update));

                            data.DirtyBit = false;
                            componentArray[i] = data;
                        }
                    }
                }

                chunkArray.Dispose();
                Profiler.EndSample();
            }

            public override void SendCommands(ComponentGroup commandGroup, ComponentSystemBase system, global::Improbable.Worker.Core.Connection connection)
            {
                Profiler.BeginSample("BlittableComponent");
                var entityType = system.GetArchetypeChunkEntityType();
                {
                    var senderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.FirstCommand>(true);
                    var responderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.FirstCommand>(true);

                    var chunks = commandGroup.CreateArchetypeChunkArray(Allocator.TempJob);
                    foreach (var chunk in chunks)
                    {
                        var entities = chunk.GetNativeArray(entityType);
                        var senders = chunk.GetNativeArray(senderType);
                        var responders = chunk.GetNativeArray(responderType);
                        for (var i = 0; i < senders.Length; i++)
                        {
                            var requests = senders[i].RequestsToSend;
                            var responses = responders[i].ResponsesToSend;
                            if (requests.Count > 0)
                            {
                                foreach (var request in requests)
                                {
                                    var schemaCommandRequest = new global::Improbable.Worker.Core.SchemaCommandRequest(ComponentId, 1);
                                    global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest.Serialization.Serialize(request.Payload, schemaCommandRequest.GetObject());

                                    var requestId = connection.SendCommandRequest(request.TargetEntityId,
                                        new global::Improbable.Worker.Core.CommandRequest(schemaCommandRequest),
                                        request.TimeoutMillis,
                                        request.AllowShortCircuiting ? ShortCircuitParameters : null);

                                    firstCommandStorage.CommandRequestsInFlight[requestId.Id] =
                                        new CommandRequestStore<global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest>(entities[i], request.Payload, request.Context, request.RequestId);
                                }

                                requests.Clear();
                            }

                            if (responses.Count > 0)
                            {
                                foreach (var response in responses)
                                {
                                    var requestId = new global::Improbable.Worker.Core.RequestId<IncomingCommandRequest>(response.RequestId);

                                    if (response.FailureMessage != null)
                                    {
                                        // Send a command failure if the string is non-null.
                                        connection.SendCommandFailure(requestId, response.FailureMessage);
                                        continue;
                                    }

                                    var schemaCommandResponse = new global::Improbable.Worker.Core.SchemaCommandResponse(ComponentId, 1);
                                    global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                                    connection.SendCommandResponse(requestId, new global::Improbable.Worker.Core.CommandResponse(schemaCommandResponse));
                                }

                                responses.Clear();
                            }
                        }
                    }

                    chunks.Dispose();
                }
                {
                    var senderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandSenders.SecondCommand>(true);
                    var responderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.CommandResponders.SecondCommand>(true);

                    var chunks = commandGroup.CreateArchetypeChunkArray(Allocator.TempJob);
                    foreach (var chunk in chunks)
                    {
                        var entities = chunk.GetNativeArray(entityType);
                        var senders = chunk.GetNativeArray(senderType);
                        var responders = chunk.GetNativeArray(responderType);
                        for (var i = 0; i < senders.Length; i++)
                        {
                            var requests = senders[i].RequestsToSend;
                            var responses = responders[i].ResponsesToSend;
                            if (requests.Count > 0)
                            {
                                foreach (var request in requests)
                                {
                                    var schemaCommandRequest = new global::Improbable.Worker.Core.SchemaCommandRequest(ComponentId, 2);
                                    global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest.Serialization.Serialize(request.Payload, schemaCommandRequest.GetObject());

                                    var requestId = connection.SendCommandRequest(request.TargetEntityId,
                                        new global::Improbable.Worker.Core.CommandRequest(schemaCommandRequest),
                                        request.TimeoutMillis,
                                        request.AllowShortCircuiting ? ShortCircuitParameters : null);

                                    secondCommandStorage.CommandRequestsInFlight[requestId.Id] =
                                        new CommandRequestStore<global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest>(entities[i], request.Payload, request.Context, request.RequestId);
                                }

                                requests.Clear();
                            }

                            if (responses.Count > 0)
                            {
                                foreach (var response in responses)
                                {
                                    var requestId = new global::Improbable.Worker.Core.RequestId<IncomingCommandRequest>(response.RequestId);

                                    if (response.FailureMessage != null)
                                    {
                                        // Send a command failure if the string is non-null.
                                        connection.SendCommandFailure(requestId, response.FailureMessage);
                                        continue;
                                    }

                                    var schemaCommandResponse = new global::Improbable.Worker.Core.SchemaCommandResponse(ComponentId, 2);
                                    global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                                    connection.SendCommandResponse(requestId, new global::Improbable.Worker.Core.CommandResponse(schemaCommandResponse));
                                }

                                responses.Clear();
                            }
                        }
                    }

                    chunks.Dispose();
                }

                Profiler.EndSample();
            }
        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override EntityArchetypeQuery CleanupArchetypeQuery => new EntityArchetypeQuery
            {
                All = Array.Empty<ComponentType>(),
                Any = new ComponentType[]
                {
                    ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(),
                    ComponentType.Create<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(),
                    ComponentType.Create<ReceivedEvents.FirstEvent>(),
                    ComponentType.Create<ReceivedEvents.SecondEvent>(),
                    ComponentType.Create<CommandRequests.FirstCommand>(),
                    ComponentType.Create<CommandResponses.FirstCommand>(),
                    ComponentType.Create<CommandRequests.SecondCommand>(),
                    ComponentType.Create<CommandResponses.SecondCommand>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>();
                var firstEventEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.FirstEvent>();
                var secondEventEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.SecondEvent>();

                var firstCommandRequestType = system.GetArchetypeChunkComponentType<CommandRequests.FirstCommand>();
                var firstCommandResponseType = system.GetArchetypeChunkComponentType<CommandResponses.FirstCommand>();

                var secondCommandRequestType = system.GetArchetypeChunkComponentType<CommandRequests.SecondCommand>();
                var secondCommandResponseType = system.GetArchetypeChunkComponentType<CommandResponses.SecondCommand>();

                var chunkArray = group.CreateArchetypeChunkArray(Allocator.Temp);

                foreach (var chunk in chunkArray)
                {
                    var entities = chunk.GetNativeArray(entityType);

                    // Updates
                    if (chunk.Has(receivedUpdateType))
                    {
                        var updateArray = chunk.GetNativeArray(receivedUpdateType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.Gdk.Tests.BlittableTypes.BlittableComponent.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // FirstEvent Event
                    if (chunk.Has(firstEventEventType))
                    {
                        var firstEventEventArray = chunk.GetNativeArray(firstEventEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.FirstEvent>(entities[i]);
                            ReferenceTypeProviders.FirstEventProvider.Free(firstEventEventArray[i].handle);
                        }
                    }

                    // SecondEvent Event
                    if (chunk.Has(secondEventEventType))
                    {
                        var secondEventEventArray = chunk.GetNativeArray(secondEventEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.SecondEvent>(entities[i]);
                            ReferenceTypeProviders.SecondEventProvider.Free(secondEventEventArray[i].handle);
                        }
                    }

                    // FirstCommand Command
                    if (chunk.Has(firstCommandRequestType))
                    {
                            var firstCommandRequestArray = chunk.GetNativeArray(firstCommandRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.FirstCommand>(entities[i]);
                            ReferenceTypeProviders.FirstCommandRequestsProvider.Free(firstCommandRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(firstCommandResponseType))
                    {
                        var firstCommandResponseArray = chunk.GetNativeArray(firstCommandResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.FirstCommand>(entities[i]);
                            ReferenceTypeProviders.FirstCommandResponsesProvider.Free(firstCommandResponseArray[i].CommandListHandle);
                        }
                    }
                    // SecondCommand Command
                    if (chunk.Has(secondCommandRequestType))
                    {
                            var secondCommandRequestArray = chunk.GetNativeArray(secondCommandRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.SecondCommand>(entities[i]);
                            ReferenceTypeProviders.SecondCommandRequestsProvider.Free(secondCommandRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(secondCommandResponseType))
                    {
                        var secondCommandResponseArray = chunk.GetNativeArray(secondCommandResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.SecondCommand>(entities[i]);
                            ReferenceTypeProviders.SecondCommandResponsesProvider.Free(secondCommandResponseArray[i].CommandListHandle);
                        }
                    }
                }

                chunkArray.Dispose();
            }
        }
    }
}
