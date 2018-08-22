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

namespace Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax
{
    public partial class Connection
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 1105;

            private readonly EntityManager entityManager;

            private const string LoggerName = "Connection.DispatcherHandler";


            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
                Connection.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);

                Connection.ReferenceTypeProviders.MyEventProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "AddComponentOp", out var entity))
                {
                    return;
                }

                var data = Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponentData(entity, new NotAuthoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>());

                var update = new Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update
                {
                };

                var updates = new List<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>
                {
                    update
                };

                var updatesComponent = new Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentAdded<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
                    );
                }
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "RemoveComponentOp", out var entity))
                {
                    return;
                }

                entityManager.RemoveComponent<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentRemoved<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
                    );
                }
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                if (!IsValidEntityId(op.EntityId, "OnComponentUpdate", out var entity))
                {
                    return;
                }

                if (entityManager.HasComponent<NotAuthoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>(entity);

                    var update = Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Serialization.GetAndApplyUpdate(op.Update.SchemaData.Value.GetFields(), ref data);

                    List<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update> updates;
                    if (entityManager.HasComponent<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(entity))
                    {
                        updates = entityManager.GetComponentData<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(entity).Updates;

                    }
                    else
                    {
                        var updatesComponent = new Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates
                        {
                            handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                        };
                        ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, new List<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>());
                        updates = updatesComponent.Updates;
                        entityManager.AddComponentData(entity, updatesComponent);
                    }

                    updates.Add(update);

                    data.DirtyBit = false;
                    entityManager.SetComponentData(entity, data);
                }

                var eventsObject = op.Update.SchemaData.Value.GetEvents();
                {
                    var eventCount = eventsObject.GetObjectCount(1);
                    if (eventCount > 0)
                    {
                        // Create component to hold received events
                        ReceivedEvents.MyEvent eventsReceived;
                        List<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> eventList;
                        if (!entityManager.HasComponent<ReceivedEvents.MyEvent>(entity))
                        {
                            eventsReceived = new ReceivedEvents.MyEvent() {
                                handle = ReferenceTypeProviders.MyEventProvider.Allocate(World)
                            };
                            eventList = new List<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>((int) eventCount);
                            ReferenceTypeProviders.MyEventProvider.Set(eventsReceived.handle, eventList);
                            entityManager.AddComponentData(entity, eventsReceived);
                        }
                        else
                        {
                            eventsReceived = entityManager.GetComponentData<ReceivedEvents.MyEvent>(entity);
                            eventList = eventsReceived.Events;
                        }

                        // Deserialize events onto component
                        for (var i = 0; i < eventCount; i++)
                        {
                            var e = global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType.Serialization.Deserialize(eventsObject.GetObject(1));
                            eventList.Add(e);
                        }
                    }
                }

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
                    default:
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(CommandIndexNotFound)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                            .WithField("CommandIndex", commandIndex)
                            .WithField("Component", "Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
                        );
                        break;
                }
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.Response.CommandIndex;
                switch (commandIndex)
                {
                    default:
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(CommandIndexNotFound)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                            .WithField("CommandIndex", commandIndex)
                            .WithField("Component", "Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
                        );
                        break;
                }
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Worker.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                        entityManager.AddComponentData(entity, new Authoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>());

                        // Add event senders
                        {
                            var eventSender = new EventSender.MyEvent()
                            {
                                handle = ReferenceTypeProviders.MyEventProvider.Allocate(World)
                            };
                            ReferenceTypeProviders.MyEventProvider.Set(eventSender.handle, new List<global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>());
                            entityManager.AddComponentData(entity, eventSender);
                        }
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponentData(entity, new AuthorityLossImminent<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                        entityManager.AddComponentData(entity, new NotAuthoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>());

                        // Remove event senders
                        {
                            var eventSender = entityManager.GetComponentData<EventSender.MyEvent>(entity);
                            ReferenceTypeProviders.MyEventProvider.Free(eventSender.handle);
                            entityManager.RemoveComponent<EventSender.MyEvent>(entity);
                        }
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>
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
                        .WithField("Component", "Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
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
                    .WithField("Component", "Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
                );
            }

        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 1105;

            public override ComponentType[] ReplicationComponentTypes => new ComponentType[] {
                ComponentType.ReadOnly<EventSender.MyEvent>(),
                ComponentType.Create<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>(),
                ComponentType.ReadOnly<Authoritative<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                ComponentType.ReadOnly<SpatialEntityId>()
            };

            public override ComponentType[] CommandTypes => new ComponentType[] {
            };


            public ComponentReplicator(EntityManager entityManager, Unity.Entities.World world) : base(entityManager)
            {
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void ExecuteReplication(ComponentGroup replicationGroup, global::Improbable.Worker.Core.Connection connection)
            {
                var entityIdDataArray = replicationGroup.GetComponentDataArray<SpatialEntityId>();
                var componentDataArray = replicationGroup.GetComponentDataArray<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>();
                var eventMyEventArray = replicationGroup.GetComponentDataArray<EventSender.MyEvent>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var data = componentDataArray[i];
                    var dirtyEvents = 0;
                    var eventsMyEvent = eventMyEventArray[i].Events;
                    dirtyEvents += eventsMyEvent.Count;

                    if (data.DirtyBit || dirtyEvents > 0)
                    {
                        var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(1105);
                        Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Serialization.Serialize(data, update.GetFields());

                        // Serialize events
                        var eventsObject = update.GetEvents();
                        if (eventsMyEvent.Count > 0)
                        {
                            foreach (var e in eventsMyEvent)
                            {
                                var obj = eventsObject.AddObject(1);
                                global::Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType.Serialization.Serialize(e, obj);
                            }

                            eventsMyEvent.Clear();
                        }

                        // Send serialized update over the wire
                        connection.SendComponentUpdate(entityIdDataArray[i].EntityId, new global::Improbable.Worker.Core.ComponentUpdate(update));

                        data.DirtyBit = false;
                        componentDataArray[i] = data;
                    }
                }
            }

            public override void SendCommands(List<ComponentGroup> commandComponentGroups, global::Improbable.Worker.Core.Connection connection)
            {
            }

        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override ComponentType[] CleanUpComponentTypes => new ComponentType[] {
                ComponentType.ReadOnly<ComponentAdded<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                ComponentType.ReadOnly<ComponentRemoved<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
            };

            public override ComponentType[] EventComponentTypes => new ComponentType[] {
                ComponentType.ReadOnly<ReceivedEvents.MyEvent>(),
            };

            public override ComponentType ComponentUpdateType => ComponentType.ReadOnly<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>();
            public override ComponentType AuthorityChangesType => ComponentType.ReadOnly<AuthorityChanges<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();

            public override ComponentType[] CommandReactiveTypes => new ComponentType[] {
            };

            public override void CleanupUpdates(ComponentGroup updateGroup, ref EntityCommandBuffer buffer)
            {
                var entities = updateGroup.GetEntityArray();
                var data = updateGroup.GetComponentDataArray<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(entities[i]);
                    ReferenceTypeProviders.UpdatesProvider.Free(data[i].handle);
                }
            }

            public override void CleanupAuthChanges(ComponentGroup authorityChangeGroup, ref EntityCommandBuffer buffer)
            {
                var entities = authorityChangeGroup.GetEntityArray();
                var data = authorityChangeGroup.GetComponentDataArray<AuthorityChanges<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<AuthorityChanges<Generated.Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entities[i]);
                    AuthorityChangesProvider.Free(data[i].Handle);
                }
            }

            public override void CleanupEvents(ComponentGroup[] eventGroups, ref EntityCommandBuffer buffer)
            {
                // Clean MyEvent
                {
                    var group = eventGroups[0];
                    if (!group.IsEmptyIgnoreFilter)
                    {
                        var entities = group.GetEntityArray();
                        var data = group.GetComponentDataArray<ReceivedEvents.MyEvent>();
                        for (var i = 0; i < entities.Length; i++)
                        {
                            buffer.RemoveComponent<ReceivedEvents.MyEvent>(entities[i]);
                            ReferenceTypeProviders.MyEventProvider.Free(data[i].handle);
                        }
                    }
                }
            }

            public override void CleanupCommands(ComponentGroup[] commandCleanupGroups, ref EntityCommandBuffer buffer)
            {
            }
        }
    }

}
