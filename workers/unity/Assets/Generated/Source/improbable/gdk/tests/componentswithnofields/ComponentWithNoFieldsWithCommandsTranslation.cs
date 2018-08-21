// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Improbable.Worker.Core;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Gdk.Core.Commands;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 1005;

            private readonly EntityManager entityManager;

            private const string LoggerName = "ComponentWithNoFieldsWithCommands.DispatcherHandler";

            private CommandStorages.Cmd CmdStorage;

            public DispatcherHandler(Worker worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                CmdStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.Cmd>();
            }

            public override void Dispose()
            {
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "AddComponentOp", out var entity))
                {
                    return;
                }

                var data = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.SpatialOSComponentWithNoFieldsWithCommands.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponentData(entity, new NotAuthoritative<SpatialOSComponentWithNoFieldsWithCommands>());

                if (entityManager.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "SpatialOSComponentWithNoFieldsWithCommands")
                    );
                }
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "RemoveComponentOp", out var entity))
                {
                    return;
                }

                entityManager.RemoveComponent<SpatialOSComponentWithNoFieldsWithCommands>(entity);

                if (entityManager.HasComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "SpatialOSComponentWithNoFieldsWithCommands")
                    );
                }
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                if (!IsValidEntityId(op.EntityId, "OnComponentUpdate", out var entity))
                {
                    return;
                }

                var data = entityManager.GetComponentData<SpatialOSComponentWithNoFieldsWithCommands>(entity);

                var update = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.SpatialOSComponentWithNoFieldsWithCommands.Serialization.GetAndApplyUpdate(op.Update.SchemaData.Value.GetFields(), ref data);

                List<SpatialOSComponentWithNoFieldsWithCommands.Update> updates;
                if (entityManager.HasComponent<SpatialOSComponentWithNoFieldsWithCommands.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<SpatialOSComponentWithNoFieldsWithCommands.ReceivedUpdates>(entity).Updates;

                }
                else
                {
                    var updatesComponent = new SpatialOSComponentWithNoFieldsWithCommands.ReceivedUpdates
                    {
                        handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                    };
                    ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, new List<SpatialOSComponentWithNoFieldsWithCommands.Update>());
                    updates = updatesComponent.Updates;
                    entityManager.AddComponentData(entity, updatesComponent);
                }

                updates.Add(update);

                data.DirtyBit = false;
                entityManager.SetComponentData(entity, data);
            }

            public override void OnAuthorityChange(AuthorityChangeOp op)
            {
                if (!IsValidEntityId(op.EntityId, "AuthorityChangeOp", out var entity))
                {
                    return;
                }

                ApplyAuthorityChange(entity, op.Authority, op.EntityId);
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                if (!IsValidEntityId(op.EntityId, "CommandRequestOp", out var entity))
                {
                    return;
                }

                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();
                switch (commandIndex)
                {
                    case 1:
                        OnCmdRequest(op);
                        break;
                    default:
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(CommandIndexNotFound)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                            .WithField("CommandIndex", commandIndex)
                            .WithField("Component", "SpatialOSComponentWithNoFieldsWithCommands")
                        );
                        break;
                }
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.Response.CommandIndex;
                switch (commandIndex)
                {
                    case 1:
                        OnCmdResponse(op);
                        break;
                    default:
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(CommandIndexNotFound)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                            .WithField("CommandIndex", commandIndex)
                            .WithField("Component", "SpatialOSComponentWithNoFieldsWithCommands")
                        );
                        break;
                }
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
                {
                    var commandSender = new Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandSenders.Cmd();
                    commandSender.CommandListHandle = Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdSenderProvider.Allocate(World);
                    commandSender.RequestsToSend = new List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Request>();

                    entityManager.AddComponentData(entity, commandSender);

                    var commandResponder = new Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponders.Cmd();
                    commandResponder.CommandListHandle = Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Allocate(World);
                    commandResponder.ResponsesToSend = new List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.Response>();

                    entityManager.AddComponentData(entity, commandResponder);
                }
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Worker.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<SpatialOSComponentWithNoFieldsWithCommands>>(entity);
                        entityManager.AddComponentData(entity, new Authoritative<SpatialOSComponentWithNoFieldsWithCommands>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponentData(entity, new AuthorityLossImminent<SpatialOSComponentWithNoFieldsWithCommands>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<SpatialOSComponentWithNoFieldsWithCommands>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<SpatialOSComponentWithNoFieldsWithCommands>>(entity);
                        entityManager.AddComponentData(entity, new NotAuthoritative<SpatialOSComponentWithNoFieldsWithCommands>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<SpatialOSComponentWithNoFieldsWithCommands>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<SpatialOSComponentWithNoFieldsWithCommands>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<SpatialOSComponentWithNoFieldsWithCommands>
                    {
                        Handle = AuthorityChangesProvider.Allocate(World)
                    };
                    AuthorityChangesProvider.Set(changes.Handle, new List<Authority>());
                    authorityChanges = changes.Changes;
                    entityManager.AddComponentData(entity, changes);
                }

                authorityChanges.Add(authority);
            }

            private bool IsValidEntityId(global::Improbable.Worker.EntityId entityId, string opType, out Unity.Entities.Entity entity)
            {
                if (!Worker.TryGetEntity(entityId, out entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(EntityNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, entityId.Id)
                        .WithField("Op", opType)
                        .WithField("Component", "SpatialOSComponentWithNoFieldsWithCommands")
                    );
                    return false;
                }

                return true;
            }

            private void LogInvalidAuthorityTransition(Authority newAuthority, Authority expectedOldAuthority, global::Improbable.Worker.EntityId entityId)
            {
                LogDispatcher.HandleLog(LogType.Error, new LogEvent(InvalidAuthorityChange)
                    .WithField(LoggingUtils.LoggerName, LoggerName)
                    .WithField(LoggingUtils.EntityId, entityId.Id)
                    .WithField("New Authority", newAuthority)
                    .WithField("Expected Old Authority", expectedOldAuthority)
                    .WithField("Component", "SpatialOSComponentWithNoFieldsWithCommands")
                );
            }

            private void OnCmdRequest(CommandRequestOp op)
            {
                if (!IsValidEntityId(op.EntityId, "CommandRequestOp", out var entity))
                {
                    return;
                }

                var deserializedRequest = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Deserialize(op.Request.SchemaData.Value.GetObject());

                List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest> requests;
                if (entityManager.HasComponent<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd>(entity))
                {
                    requests = entityManager.GetComponentData<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd>(entity).Requests;
                }
                else
                {
                    var data = new Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandRequests.Cmd
                    {
                        CommandListHandle = Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdRequestsProvider.Allocate(World)
                    };
                    requests = data.Requests = new List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest>();
                    entityManager.AddComponentData(entity, data);
                }

                requests.Add(new Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest(op.RequestId.Id,
                    op.CallerWorkerId,
                    op.CallerAttributeSet,
                    deserializedRequest));
            }

            private void OnCmdResponse(CommandResponseOp op)
            {
                if (!CmdStorage.CommandRequestsInFlight.TryGetValue(op.RequestId.Id, out var requestBundle))
                {
                    throw new InvalidOperationException($"Could not find corresponding request for RequestId {op.RequestId.Id} and command Cmd.");
                }

                var entity = requestBundle.Entity;
                CmdStorage.CommandRequestsInFlight.Remove(op.RequestId.Id);
                if (!entityManager.Exists(entity))
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(EntityNotFound)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField("Op", "CommandResponseOp - Cmd")
                        .WithField("Component", "SpatialOSComponentWithNoFieldsWithCommands")
                    );
                    return;
                }

                global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty? response = null;
                if (op.StatusCode == StatusCode.Success)
                {
                    response = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Deserialize(op.Response.SchemaData.Value.GetObject());
                }

                List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse> responses;
                if (entityManager.HasComponent<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd>(entity))
                {
                    responses = entityManager.GetComponentData<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd>(entity).Responses;
                }
                else
                {
                    var data = new Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponses.Cmd
                    {
                        CommandListHandle = Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponsesProvider.Allocate(World)
                    };
                    responses = data.Responses = new List<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse>();
                    entityManager.AddComponentData(entity, data);
                }

                responses.Add(new Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse(op.EntityId,
                    op.Message,
                    op.StatusCode,
                    response,
                    requestBundle.Request));
            }
        }

        public class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 1005;

            public override ComponentType[] ReplicationComponentTypes => new ComponentType[] {
                ComponentType.Create<SpatialOSComponentWithNoFieldsWithCommands>(),
                ComponentType.ReadOnly<Authoritative<SpatialOSComponentWithNoFieldsWithCommands>>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            };

            public override ComponentType[] CommandTypes => new ComponentType[] {
                ComponentType.ReadOnly<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandSenders.Cmd>(),
                ComponentType.ReadOnly<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponders.Cmd>(),
            };

            private CommandStorages.Cmd CmdStorage;

            public ComponentReplicator(EntityManager entityManager, Unity.Entities.World world) : base(entityManager)
            {
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
                CmdStorage = bookkeepingSystem.GetCommandStorageForType<CommandStorages.Cmd>();
            }

            public override void ExecuteReplication(ComponentGroup replicationGroup, global::Improbable.Worker.Core.Connection connection)
            {
                var entityIdDataArray = replicationGroup.GetComponentDataArray<SpatialEntityId>();
                var componentDataArray = replicationGroup.GetComponentDataArray<SpatialOSComponentWithNoFieldsWithCommands>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var data = componentDataArray[i];
                    var dirtyEvents = 0;

                    if (data.DirtyBit || dirtyEvents > 0)
                    {
                        var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(1005);
                        SpatialOSComponentWithNoFieldsWithCommands.Serialization.Serialize(data, update.GetFields());

                        // Send serialized update over the wire
                        connection.SendComponentUpdate(entityIdDataArray[i].EntityId, new global::Improbable.Worker.Core.ComponentUpdate(update));

                        data.DirtyBit = false;
                        componentDataArray[i] = data;
                    }
                }
            }

            public override void SendCommands(List<ComponentGroup> commandComponentGroups, global::Improbable.Worker.Core.Connection connection)
            {
                if (!commandComponentGroups[0].IsEmptyIgnoreFilter)
                {
                    var componentGroup = commandComponentGroups[0];
                    var commandSenderDataArray = componentGroup.GetComponentDataArray<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandSenders.Cmd>();
                    var entityArray = componentGroup.GetEntityArray();

                    for (var j = 0; j < commandSenderDataArray.Length; j++)
                    {
                        var requests = commandSenderDataArray[j];
                        for (var k = 0; k < requests.RequestsToSend.Count; k++)
                        {
                            var wrappedCommandRequest = requests.RequestsToSend[k];

                            var schemaCommandRequest = new global::Improbable.Worker.Core.SchemaCommandRequest(ComponentId, 1);
                            global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Serialize(wrappedCommandRequest.RawRequest, schemaCommandRequest.GetObject());

                            var requestId = connection.SendCommandRequest(wrappedCommandRequest.TargetEntityId,
                                new global::Improbable.Worker.Core.CommandRequest(schemaCommandRequest),
                                wrappedCommandRequest.TimeoutMillis,
                                wrappedCommandRequest.AllowShortCircuiting ? ShortCircuitParameters : null);

                            CmdStorage.CommandRequestsInFlight[requestId.Id] =
                                new CommandRequestStore<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>(entityArray[j], wrappedCommandRequest.RawRequest, null);
                        }

                        requests.RequestsToSend.Clear();
                    }
                }
                if (!commandComponentGroups[1].IsEmptyIgnoreFilter)
                {
                    var componentGroup = commandComponentGroups[1];
                    var commandResponderDataArray = componentGroup.GetComponentDataArray<Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.ComponentWithNoFieldsWithCommands.CommandResponders.Cmd>();

                    for (var j = 0; j < commandResponderDataArray.Length; j++)
                    {
                        var responses = commandResponderDataArray[j];
                        for (var k = 0; k < responses.ResponsesToSend.Count; k++)
                        {
                            var wrappedCommandResponse = responses.ResponsesToSend[k];
                            var requestId = new global::Improbable.Worker.Core.RequestId<IncomingCommandRequest>(wrappedCommandResponse.RequestId);

                            if (wrappedCommandResponse.FailureMessage != null)
                            {
                                // Send a command failure if the string is non-null.
                                connection.SendCommandFailure(requestId, wrappedCommandResponse.FailureMessage);
                                continue;
                            }

                            var schemaCommandResponse = new global::Improbable.Worker.Core.SchemaCommandResponse(ComponentId, 1);
                            global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Serialize(wrappedCommandResponse.RawResponse.Value, schemaCommandResponse.GetObject());

                            connection.SendCommandResponse(requestId, new global::Improbable.Worker.Core.CommandResponse(schemaCommandResponse));
                        }

                        responses.ResponsesToSend.Clear();
                    }
                }
            }

        }

        public class ComponentCleanup : ComponentCleanupHandler
        {
            public override ComponentType[] CleanUpComponentTypes => new ComponentType[] {
                typeof(ComponentAdded<SpatialOSComponentWithNoFieldsWithCommands>),
                typeof(ComponentRemoved<SpatialOSComponentWithNoFieldsWithCommands>),
            };

            public override ComponentType[] EventComponentTypes => new ComponentType[] {
            };

            public override ComponentType ComponentUpdateType => ComponentType.ReadOnly<SpatialOSComponentWithNoFieldsWithCommands.ReceivedUpdates>();
            public override ComponentType AuthorityChangesType => ComponentType.ReadOnly<AuthorityChanges<SpatialOSComponentWithNoFieldsWithCommands>>();

            public override ComponentType[] CommandReactiveTypes => new ComponentType[] {
                ComponentType.ReadOnly<CommandRequests.Cmd>(),
                ComponentType.ReadOnly<CommandResponses.Cmd>(),
            };

            public override void CleanupUpdates(ComponentGroup updateGroup, ref EntityCommandBuffer buffer)
            {
                var entities = updateGroup.GetEntityArray();
                var data = updateGroup.GetComponentDataArray<SpatialOSComponentWithNoFieldsWithCommands.ReceivedUpdates>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<SpatialOSComponentWithNoFieldsWithCommands.ReceivedUpdates>(entities[i]);
                    ReferenceTypeProviders.UpdatesProvider.Free(data[i].handle);
                }
            }

            public override void CleanupAuthChanges(ComponentGroup authorityChangeGroup, ref EntityCommandBuffer buffer)
            {
                var entities = authorityChangeGroup.GetEntityArray();
                var data = authorityChangeGroup.GetComponentDataArray<AuthorityChanges<SpatialOSComponentWithNoFieldsWithCommands>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<AuthorityChanges<SpatialOSComponentWithNoFieldsWithCommands>>(entities[i]);
                    AuthorityChangesProvider.Free(data[i].Handle);
                }
            }

            public override void CleanupEvents(ComponentGroup[] eventGroups, ref EntityCommandBuffer buffer)
            {
            }

            public override void CleanupCommands(ComponentGroup[] commandCleanupGroups, ref EntityCommandBuffer buffer)
            {
                if (!commandCleanupGroups[0].IsEmptyIgnoreFilter)
                {
                    var requestsGroup = commandCleanupGroups[0];
                    var entities = requestsGroup.GetEntityArray();
                    var data = requestsGroup.GetComponentDataArray<CommandRequests.Cmd>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        buffer.RemoveComponent<CommandRequests.Cmd>(entities[i]);
                        ReferenceTypeProviders.CmdRequestsProvider.Free(data[i].CommandListHandle);
                    }
                }

                if (!commandCleanupGroups[1].IsEmptyIgnoreFilter)
                {
                    var responsesGroup = commandCleanupGroups[1];
                    var entities = responsesGroup.GetEntityArray();
                    var data = responsesGroup.GetComponentDataArray<CommandResponses.Cmd>();
                    for (var i = 0; i < entities.Length; i++)
                    {
                        buffer.RemoveComponent<CommandResponses.Cmd>(entities[i]);
                        ReferenceTypeProviders.CmdResponsesProvider.Free(data[i].CommandListHandle);
                    }
                }
            }
        }
    }

}
