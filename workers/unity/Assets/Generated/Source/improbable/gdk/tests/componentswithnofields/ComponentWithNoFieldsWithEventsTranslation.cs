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
    public partial class ComponentWithNoFieldsWithEvents
    {
        public class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 1004;

            private readonly EntityManager entityManager;

            private const string LoggerName = "ComponentWithNoFieldsWithEvents.DispatcherHandler";


            public DispatcherHandler(Worker worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
                ComponentWithNoFieldsWithEvents.ReferenceTypeProviders.EvtProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "AddComponentOp", out var entity))
                {
                    return;
                }

                var data = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.SpatialOSComponentWithNoFieldsWithEvents.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponentData(entity, new NotAuthoritative<SpatialOSComponentWithNoFieldsWithEvents>());

                var update = new SpatialOSComponentWithNoFieldsWithEvents.Update 
                {
                };
                
                var updates = new List<SpatialOSComponentWithNoFieldsWithEvents.Update>
                {
                    update
                };
                
                var updatesComponent = new SpatialOSComponentWithNoFieldsWithEvents.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };
                
                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);
                
                if (entityManager.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "SpatialOSComponentWithNoFieldsWithEvents")
                    );
                }
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "RemoveComponentOp", out var entity))
                {
                    return;
                }

                entityManager.RemoveComponent<SpatialOSComponentWithNoFieldsWithEvents>(entity);

                if (entityManager.HasComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "SpatialOSComponentWithNoFieldsWithEvents")
                    );
                }
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                if (!IsValidEntityId(op.EntityId, "OnComponentUpdate", out var entity))
                {
                    return;
                }

                if (entityManager.HasComponent<NotAuthoritative<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    var data = entityManager.GetComponentData<SpatialOSComponentWithNoFieldsWithEvents>(entity);

                    var update = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.SpatialOSComponentWithNoFieldsWithEvents.Serialization.GetAndApplyUpdate(op.Update.SchemaData.Value.GetFields(), ref data);

                    List<SpatialOSComponentWithNoFieldsWithEvents.Update> updates;
                    if (entityManager.HasComponent<SpatialOSComponentWithNoFieldsWithEvents.ReceivedUpdates>(entity))
                    {
                        updates = entityManager.GetComponentData<SpatialOSComponentWithNoFieldsWithEvents.ReceivedUpdates>(entity).Updates;

                    }
                    else
                    {
                        var updatesComponent = new SpatialOSComponentWithNoFieldsWithEvents.ReceivedUpdates
                        {
                            handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                        };
                        ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, new List<SpatialOSComponentWithNoFieldsWithEvents.Update>());
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
                        ReceivedEvents.Evt eventsReceived;
                        List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty> eventList;
                        if (!entityManager.HasComponent<ReceivedEvents.Evt>(entity))
                        {
                            eventsReceived = new ReceivedEvents.Evt() {
                                handle = ReferenceTypeProviders.EvtProvider.Allocate(World)
                            };
                            eventList = new List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>((int) eventCount);
                            ReferenceTypeProviders.EvtProvider.Set(eventsReceived.handle, eventList);
                            entityManager.AddComponentData(entity, eventsReceived);
                        }
                        else
                        {
                            eventsReceived = entityManager.GetComponentData<ReceivedEvents.Evt>(entity);
                            eventList = eventsReceived.Events;
                        }

                        // Deserialize events onto component
                        for (var i = 0; i < eventCount; i++)
                        {
                            var e = global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Deserialize(eventsObject.GetObject(1));
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
                            .WithField("Component", "SpatialOSComponentWithNoFieldsWithEvents")
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
                            .WithField("Component", "SpatialOSComponentWithNoFieldsWithEvents")
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
                        if (!entityManager.HasComponent<NotAuthoritative<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<SpatialOSComponentWithNoFieldsWithEvents>>(entity);
                        entityManager.AddComponentData(entity, new Authoritative<SpatialOSComponentWithNoFieldsWithEvents>());

                        // Add event senders
                        {
                            var eventSender = new EventSender.Evt()
                            {
                                handle = ReferenceTypeProviders.EvtProvider.Allocate(World)
                            };
                            ReferenceTypeProviders.EvtProvider.Set(eventSender.handle, new List<global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty>());
                            entityManager.AddComponentData(entity, eventSender);
                        }
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponentData(entity, new AuthorityLossImminent<SpatialOSComponentWithNoFieldsWithEvents>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<SpatialOSComponentWithNoFieldsWithEvents>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<SpatialOSComponentWithNoFieldsWithEvents>>(entity);
                        entityManager.AddComponentData(entity, new NotAuthoritative<SpatialOSComponentWithNoFieldsWithEvents>());

                        // Remove event senders
                        {
                            var eventSender = entityManager.GetComponentData<EventSender.Evt>(entity);
                            ReferenceTypeProviders.EvtProvider.Free(eventSender.handle);
                            entityManager.RemoveComponent<EventSender.Evt>(entity);
                        }
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>
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
                        .WithField("Component", "SpatialOSComponentWithNoFieldsWithEvents")
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
                    .WithField("Component", "SpatialOSComponentWithNoFieldsWithEvents")
                );
            }

        }

        public class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 1004;

            public override ComponentType[] ReplicationComponentTypes => new ComponentType[] {
                ComponentType.ReadOnly<EventSender.Evt>(),
                ComponentType.Create<SpatialOSComponentWithNoFieldsWithEvents>(),
                ComponentType.ReadOnly<Authoritative<SpatialOSComponentWithNoFieldsWithEvents>>(),
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
                var componentDataArray = replicationGroup.GetComponentDataArray<SpatialOSComponentWithNoFieldsWithEvents>();
                var eventEvtArray = replicationGroup.GetComponentDataArray<EventSender.Evt>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var data = componentDataArray[i];
                    var dirtyEvents = 0;
                    var eventsEvt = eventEvtArray[i].Events;
                    dirtyEvents += eventsEvt.Count;

                    if (data.DirtyBit || dirtyEvents > 0)
                    {
                        var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(1004);
                        SpatialOSComponentWithNoFieldsWithEvents.Serialization.Serialize(data, update.GetFields());

                        // Serialize events
                        var eventsObject = update.GetEvents();
                        if (eventsEvt.Count > 0)
                        {
                            foreach (var e in eventsEvt)
                            {
                                var obj = eventsObject.AddObject(1);
                                global::Generated.Improbable.Gdk.Tests.ComponentsWithNoFields.Empty.Serialization.Serialize(e, obj);
                            }

                            eventsEvt.Clear();
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

        public class ComponentCleanup : ComponentCleanupHandler
        {
            public override ComponentType[] CleanUpComponentTypes => new ComponentType[] {
                ComponentType.ReadOnly<ComponentAdded<SpatialOSComponentWithNoFieldsWithEvents>>(),
                ComponentType.ReadOnly<ComponentRemoved<SpatialOSComponentWithNoFieldsWithEvents>>(),
            };

            public override ComponentType[] EventComponentTypes => new ComponentType[] {
                ComponentType.ReadOnly<ReceivedEvents.Evt>(),
            };

            public override ComponentType ComponentUpdateType => ComponentType.ReadOnly<SpatialOSComponentWithNoFieldsWithEvents.ReceivedUpdates>();
            public override ComponentType AuthorityChangesType => ComponentType.ReadOnly<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>();

            public override ComponentType[] CommandReactiveTypes => new ComponentType[] {
            };

            public override void CleanupUpdates(ComponentGroup updateGroup, ref EntityCommandBuffer buffer)
            {
                var entities = updateGroup.GetEntityArray();
                var data = updateGroup.GetComponentDataArray<SpatialOSComponentWithNoFieldsWithEvents.ReceivedUpdates>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<SpatialOSComponentWithNoFieldsWithEvents.ReceivedUpdates>(entities[i]);
                    ReferenceTypeProviders.UpdatesProvider.Free(data[i].handle);
                }
            }

            public override void CleanupAuthChanges(ComponentGroup authorityChangeGroup, ref EntityCommandBuffer buffer)
            {
                var entities = authorityChangeGroup.GetEntityArray();
                var data = authorityChangeGroup.GetComponentDataArray<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<AuthorityChanges<SpatialOSComponentWithNoFieldsWithEvents>>(entities[i]);
                    AuthorityChangesProvider.Free(data[i].Handle);
                }
            }

            public override void CleanupEvents(ComponentGroup[] eventGroups, ref EntityCommandBuffer buffer)
            {
                // Clean Evt
                {
                    var group = eventGroups[0];
                    if (!group.IsEmptyIgnoreFilter)
                    {
                        var entities = group.GetEntityArray();
                        var data = group.GetComponentDataArray<ReceivedEvents.Evt>();
                        for (var i = 0; i < entities.Length; i++)
                        {
                            buffer.RemoveComponent<ReceivedEvents.Evt>(entities[i]);
                            ReferenceTypeProviders.EvtProvider.Free(data[i].handle);
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
