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

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 1002;

            private readonly EntityManager entityManager;

            private const string LoggerName = "NonBlittableComponent.DispatcherHandler";

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
                NonBlittableComponent.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);

                NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.FirstEventProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.SecondEventProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.CleanDataInWorld(World);
                NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("NonBlittableComponent");
                var data = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());

                var update = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update
                {
                    BoolField = data.BoolField,
                    IntField = data.IntField,
                    LongField = data.LongField,
                    FloatField = data.FloatField,
                    DoubleField = data.DoubleField,
                    StringField = data.StringField,
                    OptionalField = data.OptionalField,
                    ListField = data.ListField,
                    MapField = data.MapField,
                };

                var updates = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("NonBlittableComponent");

                var data = entityManager.GetComponentData<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>(entity);
                NonBlittableComponent.ReferenceTypeProviders.StringFieldProvider.Free(data.stringFieldHandle);
                NonBlittableComponent.ReferenceTypeProviders.OptionalFieldProvider.Free(data.optionalFieldHandle);
                NonBlittableComponent.ReferenceTypeProviders.ListFieldProvider.Free(data.listFieldHandle);
                NonBlittableComponent.ReferenceTypeProviders.MapFieldProvider.Free(data.mapFieldHandle);

                entityManager.RemoveComponent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("NonBlittableComponent");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>(entity);
                    Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.DirtyBit = false;
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update> updates;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update.Pool.Count > 0 ? Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update.Pool.Pop() : new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update>();
                    var updatesComponent = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates
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
                        List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload> eventList;
                        if (!entityManager.HasComponent<ReceivedEvents.FirstEvent>(entity))
                        {
                            eventsReceived = new ReceivedEvents.FirstEvent() {
                                handle = ReferenceTypeProviders.FirstEventProvider.Allocate(World)
                            };
                            eventList = new List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>((int) eventCount);
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
                            var e = global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload.Serialization.Deserialize(eventsObject.GetObject(1));
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
                        List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload> eventList;
                        if (!entityManager.HasComponent<ReceivedEvents.SecondEvent>(entity))
                        {
                            eventsReceived = new ReceivedEvents.SecondEvent() {
                                handle = ReferenceTypeProviders.SecondEventProvider.Allocate(World)
                            };
                            eventList = new List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>((int) eventCount);
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
                            var e = global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload.Serialization.Deserialize(eventsObject.GetObject(2));
                            eventList.Add(e);
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("NonBlittableComponent");
                ApplyAuthorityChange(entity, op.Authority, op.EntityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();

                Profiler.BeginSample("NonBlittableComponent");
                switch (commandIndex)
                {
                    case 1:
                        OnFirstCommandRequest(op);
                        break;
                    case 2:
                        OnSecondCommandRequest(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "NonBlittableComponent");
                }

                Profiler.EndSample();
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.Response.CommandIndex;

                Profiler.BeginSample("NonBlittableComponent");
                switch (commandIndex)
                {
                    case 1:
                        OnFirstCommandResponse(op);
                        break;
                    case 2:
                        OnSecondCommandResponse(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "NonBlittableComponent");
                }

                Profiler.EndSample();
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
                {
                    var commandSender = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandSenders.FirstCommand();
                    commandSender.CommandListHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandSenderProvider.Allocate(World);
                    commandSender.RequestsToSend = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Request>();

                    entityManager.AddComponentData(entity, commandSender);

                    var commandResponder = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponders.FirstCommand();
                    commandResponder.CommandListHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponderProvider.Allocate(World);
                    commandResponder.ResponsesToSend = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.Response>();

                    entityManager.AddComponentData(entity, commandResponder);
                }
                {
                    var commandSender = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandSenders.SecondCommand();
                    commandSender.CommandListHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandSenderProvider.Allocate(World);
                    commandSender.RequestsToSend = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Request>();

                    entityManager.AddComponentData(entity, commandSender);

                    var commandResponder = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponders.SecondCommand();
                    commandResponder.CommandListHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponderProvider.Allocate(World);
                    commandResponder.ResponsesToSend = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.Response>();

                    entityManager.AddComponentData(entity, commandResponder);
                }
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Worker.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());

                        // Add event senders
                        {
                            var eventSender = new EventSender.FirstEvent()
                            {
                                handle = ReferenceTypeProviders.FirstEventProvider.Allocate(World)
                            };
                            ReferenceTypeProviders.FirstEventProvider.Set(eventSender.handle, new List<global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload>());
                            entityManager.AddComponentData(entity, eventSender);
                        }
                        {
                            var eventSender = new EventSender.SecondEvent()
                            {
                                handle = ReferenceTypeProviders.SecondEventProvider.Allocate(World)
                            };
                            ReferenceTypeProviders.SecondEventProvider.Set(eventSender.handle, new List<global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload>());
                            entityManager.AddComponentData(entity, eventSender);
                        }
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>());

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
                if (entityManager.HasComponent<AuthorityChanges<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>
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
                    .WithField("Component", "Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent")
                );
            }

            private void OnFirstCommandRequest(CommandRequestOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                var deserializedRequest = global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest> requests;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.FirstCommand>(entity))
                {
                    requests = entityManager.GetComponentData<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.FirstCommand>(entity).Requests;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.FirstCommand
                    {
                        CommandListHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandRequestsProvider.Allocate(World)
                    };
                    requests = data.Requests = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest>();
                    entityManager.AddComponentData(entity, data);
                }

                requests.Add(new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedRequest(op.RequestId.Id,
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
                        .WithField("Component", "Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent")
                    );
                    return;
                }

                global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse? response = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    response = global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse> responses;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.FirstCommand>(entity))
                {
                    responses = entityManager.GetComponentData<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.FirstCommand>(entity).Responses;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.FirstCommand
                    {
                        CommandListHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.FirstCommandResponsesProvider.Allocate(World)
                    };
                    responses = data.Responses = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse>();
                    entityManager.AddComponentData(entity, data);
                }

                responses.Add(new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.FirstCommand.ReceivedResponse(op.EntityId,
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

                var deserializedRequest = global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest> requests;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.SecondCommand>(entity))
                {
                    requests = entityManager.GetComponentData<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.SecondCommand>(entity).Requests;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandRequests.SecondCommand
                    {
                        CommandListHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandRequestsProvider.Allocate(World)
                    };
                    requests = data.Requests = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest>();
                    entityManager.AddComponentData(entity, data);
                }

                requests.Add(new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedRequest(op.RequestId.Id,
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
                        .WithField("Component", "Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent")
                    );
                    return;
                }

                global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse? response = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    response = global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse> responses;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.SecondCommand>(entity))
                {
                    responses = entityManager.GetComponentData<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.SecondCommand>(entity).Responses;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponses.SecondCommand
                    {
                        CommandListHandle = Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReferenceTypeProviders.SecondCommandResponsesProvider.Allocate(World)
                    };
                    responses = data.Responses = new List<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse>();
                    entityManager.AddComponentData(entity, data);
                }

                responses.Add(new Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondCommand.ReceivedResponse(op.EntityId,
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
            public override uint ComponentId => 1002;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<EventSender.FirstEvent>(),
                    ComponentType.Create<EventSender.SecondEvent>(),
                    ComponentType.Create<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(),
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
                        ComponentType.Create<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandSenders.FirstCommand>(),
                        ComponentType.Create<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponders.FirstCommand>(),
                    },
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                },
                new EntityArchetypeQuery()
                {
                    All = new[]
                    {
                        ComponentType.Create<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandSenders.SecondCommand>(),
                        ComponentType.Create<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponders.SecondCommand>(),
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
                Profiler.BeginSample("NonBlittableComponent");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.Temp);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>();
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
                            var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(1002);
                            Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Serialization.SerializeUpdate(data, update);

                            // Serialize events
                            var eventsObject = update.GetEvents();
                            if (eventsFirstEvent.Count > 0)
                            {
                                foreach (var e in eventsFirstEvent)
                                {
                                    var obj = eventsObject.AddObject(1);
                                    global::Improbable.Gdk.Tests.NonblittableTypes.FirstEventPayload.Serialization.Serialize(e, obj);
                                }

                                eventsFirstEvent.Clear();
                            }

                            if (eventsSecondEvent.Count > 0)
                            {
                                foreach (var e in eventsSecondEvent)
                                {
                                    var obj = eventsObject.AddObject(2);
                                    global::Improbable.Gdk.Tests.NonblittableTypes.SecondEventPayload.Serialization.Serialize(e, obj);
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
                Profiler.BeginSample("NonBlittableComponent");
                var entityType = system.GetArchetypeChunkEntityType();
                {
                    var senderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandSenders.FirstCommand>(true);
                    var responderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponders.FirstCommand>(true);

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
                                    global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest.Serialization.Serialize(request.Payload, schemaCommandRequest.GetObject());

                                    var requestId = connection.SendCommandRequest(request.TargetEntityId,
                                        new global::Improbable.Worker.Core.CommandRequest(schemaCommandRequest),
                                        request.TimeoutMillis,
                                        request.AllowShortCircuiting ? ShortCircuitParameters : null);

                                    firstCommandStorage.CommandRequestsInFlight[requestId.Id] =
                                        new CommandRequestStore<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>(entities[i], request.Payload, request.Context, request.RequestId);
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
                                    global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

                                    connection.SendCommandResponse(requestId, new global::Improbable.Worker.Core.CommandResponse(schemaCommandResponse));
                                }

                                responses.Clear();
                            }
                        }
                    }

                    chunks.Dispose();
                }
                {
                    var senderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandSenders.SecondCommand>(true);
                    var responderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.CommandResponders.SecondCommand>(true);

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
                                    global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest.Serialization.Serialize(request.Payload, schemaCommandRequest.GetObject());

                                    var requestId = connection.SendCommandRequest(request.TargetEntityId,
                                        new global::Improbable.Worker.Core.CommandRequest(schemaCommandRequest),
                                        request.TimeoutMillis,
                                        request.AllowShortCircuiting ? ShortCircuitParameters : null);

                                    secondCommandStorage.CommandRequestsInFlight[requestId.Id] =
                                        new CommandRequestStore<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>(entities[i], request.Payload, request.Context, request.RequestId);
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
                                    global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandResponse.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

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
                    ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(),
                    ComponentType.Create<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(),
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
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>();
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
                            buffer.RemoveComponent<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.Component>>(entities[i]);
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
