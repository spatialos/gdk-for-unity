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

namespace Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 1005;

            private readonly EntityManager entityManager;

            private const string LoggerName = "ComponentWithNoFieldsWithCommands.DispatcherHandler";

            private CommandStorages.Cmd cmdStorage;

            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                cmdStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.Cmd>();
            }

            public override void Dispose()
            {
                ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);

                ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdSenderProvider.CleanDataInWorld(World);
                ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdRequestsProvider.CleanDataInWorld(World);
                ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.CleanDataInWorld(World);
                ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponsesProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("ComponentWithNoFieldsWithCommands");
                var data = Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>());

                var update = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update
                {
                };

                var updates = new List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("ComponentWithNoFieldsWithCommands");

                entityManager.RemoveComponent<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("ComponentWithNoFieldsWithCommands");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>(entity);
                    Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.DirtyBit = false;
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update> updates;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update.Pool.Count > 0 ? Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update.Pool.Pop() : new List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update>();
                    var updatesComponent = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReceivedUpdates
                    {
                        handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                    };
                    ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                    entityManager.AddComponentData(entity, updatesComponent);
                }

                updates.Add(update);

                Profiler.EndSample();
            }

            public override void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("ComponentWithNoFieldsWithCommands");
                ApplyAuthorityChange(entity, op.Authority, op.EntityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();

                Profiler.BeginSample("ComponentWithNoFieldsWithCommands");
                switch (commandIndex)
                {
                    case 1:
                        OnCmdRequest(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "ComponentWithNoFieldsWithCommands");
                }

                Profiler.EndSample();
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.Response.CommandIndex;

                Profiler.BeginSample("ComponentWithNoFieldsWithCommands");
                switch (commandIndex)
                {
                    case 1:
                        OnCmdResponse(op);
                        break;
                    default:
                        throw new UnknownCommandIndexException(commandIndex, "ComponentWithNoFieldsWithCommands");
                }

                Profiler.EndSample();
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
                {
                    var commandSender = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandSenders.Cmd();
                    commandSender.CommandListHandle = Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdSenderProvider.Allocate(World);
                    commandSender.RequestsToSend = new List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Request>();

                    entityManager.AddComponentData(entity, commandSender);

                    var commandResponder = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponders.Cmd();
                    commandResponder.CommandListHandle = Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Allocate(World);
                    commandResponder.ResponsesToSend = new List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Response>();

                    entityManager.AddComponentData(entity, commandResponder);
                }
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Worker.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>
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
                    .WithField("Component", "Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands")
                );
            }

            private void OnCmdRequest(CommandRequestOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                var deserializedRequest = global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest> requests;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd>(entity))
                {
                    requests = entityManager.GetComponentData<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd>(entity).Requests;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd
                    {
                        CommandListHandle = Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdRequestsProvider.Allocate(World)
                    };
                    requests = data.Requests = new List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest>();
                    entityManager.AddComponentData(entity, data);
                }

                requests.Add(new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest(op.RequestId.Id,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest));
            }

            private void OnCmdResponse(CommandResponseOp op)
            {
                if (!cmdStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out var requestBundle))
                {
                    throw new InvalidOperationException($"Could not find corresponding request for RequestId {op.RequestId.Id} and command Cmd.");
                }

                var entity = requestBundle.Entity;
                cmdStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);
                if (!entityManager.Exists(entity))
                {
                    LogDispatcher.HandleLog(LogType.Log, new LogEvent(EntityNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("Op", "CommandResponseOp - Cmd")
                        .WithField("Component", "Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands")
                    );
                    return;
                }

                global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty? response = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    response = global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse> responses;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd>(entity))
                {
                    responses = entityManager.GetComponentData<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd>(entity).Responses;
                }
                else
                {
                    var data = new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd
                    {
                        CommandListHandle = Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponsesProvider.Allocate(World)
                    };
                    responses = data.Responses = new List<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse>();
                    entityManager.AddComponentData(entity, data);
                }

                responses.Add(new Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse(op.EntityId,
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
            public override uint ComponentId => 1005;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(),
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
                        ComponentType.Create<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandSenders.Cmd>(),
                        ComponentType.Create<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponders.Cmd>(),
                    },
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                },
            };

            private CommandStorages.Cmd cmdStorage;

            public ComponentReplicator(EntityManager entityManager, Unity.Entities.World world) : base(entityManager)
            {
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                cmdStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.Cmd>();
            }

            public override void ExecuteReplication(ComponentGroup replicationGroup, ComponentSystemBase system, global::Improbable.Worker.Core.Connection connection)
            {
                Profiler.BeginSample("ComponentWithNoFieldsWithCommands");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.Temp);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>();
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var componentArray = chunk.GetNativeArray(componentType);
                    for (var i = 0; i < componentArray.Length; i++)
                    {
                        var data = componentArray[i];
                        var dirtyEvents = 0;

                        if (data.DirtyBit || dirtyEvents > 0)
                        {
                            var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(1005);
                            Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Serialization.SerializeUpdate(data, update);

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
                Profiler.BeginSample("ComponentWithNoFieldsWithCommands");
                var entityType = system.GetArchetypeChunkEntityType();
                {
                    var senderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandSenders.Cmd>(true);
                    var responderType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponders.Cmd>(true);

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
                                    global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Serialize(request.Payload, schemaCommandRequest.GetObject());

                                    var requestId = connection.SendCommandRequest(request.TargetEntityId,
                                        new global::Improbable.Worker.Core.CommandRequest(schemaCommandRequest),
                                        request.TimeoutMillis,
                                        request.AllowShortCircuiting ? ShortCircuitParameters : null);

                                    cmdStorage.CommandRequestsInFlight[requestId.Id] =
                                        new CommandRequestStore<global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>(entities[i], request.Payload, request.Context, request.RequestId);
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
                                    global::Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Serialize(response.Payload.Value, schemaCommandResponse.GetObject());

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
                    ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(),
                    ComponentType.Create<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(),
                    ComponentType.Create<CommandRequests.Cmd>(),
                    ComponentType.Create<CommandResponses.Cmd>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>();

                var cmdRequestType = system.GetArchetypeChunkComponentType<CommandRequests.Cmd>();
                var cmdResponseType = system.GetArchetypeChunkComponentType<CommandResponses.Cmd>();

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
                            buffer.RemoveComponent<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // Cmd Command
                    if (chunk.Has(cmdRequestType))
                    {
                            var cmdRequestArray = chunk.GetNativeArray(cmdRequestType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandRequests.Cmd>(entities[i]);
                            ReferenceTypeProviders.CmdRequestsProvider.Free(cmdRequestArray[i].CommandListHandle);
                        }
                    }

                    if (chunk.Has(cmdResponseType))
                    {
                        var cmdResponseArray = chunk.GetNativeArray(cmdResponseType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<CommandResponses.Cmd>(entities[i]);
                            ReferenceTypeProviders.CmdResponsesProvider.Free(cmdResponseArray[i].CommandListHandle);
                        }
                    }
                }

                chunkArray.Dispose();
            }
        }
    }
}
