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

namespace Improbable.Gdk.Tests.AlternateSchemaSyntax
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
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("Connection");
                var data = Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>());

                var update = new Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update
                {
                };

                var updates = new List<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("Connection");

                entityManager.RemoveComponent<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("Connection");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>(entity);
                    Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.DirtyBit = false;
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update> updates;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update.Pool.Count > 0 ? Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update.Pool.Pop() : new List<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update>();
                    var updatesComponent = new Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates
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
                        ReceivedEvents.MyEvent eventsReceived;
                        List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType> eventList;
                        if (!entityManager.HasComponent<ReceivedEvents.MyEvent>(entity))
                        {
                            eventsReceived = new ReceivedEvents.MyEvent() {
                                handle = ReferenceTypeProviders.MyEventProvider.Allocate(World)
                            };
                            eventList = new List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>((int) eventCount);
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
                            var e = global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType.Serialization.Deserialize(eventsObject.GetObject(1));
                            eventList.Add(e);
                        }
                    }
                }

                Profiler.EndSample();
            }

            public override void OnAuthorityChange(AuthorityChangeOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("Connection");
                ApplyAuthorityChange(entity, op.Authority, op.EntityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();
                throw new UnknownCommandIndexException(commandIndex, "Connection");
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.Response.CommandIndex;
                throw new UnknownCommandIndexException(commandIndex, "Connection");
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Worker.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>());

                        // Add event senders
                        {
                            var eventSender = new EventSender.MyEvent()
                            {
                                handle = ReferenceTypeProviders.MyEventProvider.Allocate(World)
                            };
                            ReferenceTypeProviders.MyEventProvider.Set(eventSender.handle, new List<global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType>());
                            entityManager.AddComponentData(entity, eventSender);
                        }
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>());

                        // Remove event senders
                        {
                            var eventSender = entityManager.GetComponentData<EventSender.MyEvent>(entity);
                            ReferenceTypeProviders.MyEventProvider.Free(eventSender.handle);
                            entityManager.RemoveComponent<EventSender.MyEvent>(entity);
                        }
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>
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
                    .WithField("Component", "Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection")
                );
            }
        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 1105;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<EventSender.MyEvent>(),
                    ComponentType.Create<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                    ComponentType.ReadOnly<SpatialEntityId>()
                },
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
            };

            public override EntityArchetypeQuery[] CommandQueries => new EntityArchetypeQuery[]
            {
            };


            public ComponentReplicator(EntityManager entityManager, Unity.Entities.World world) : base(entityManager)
            {
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void ExecuteReplication(ComponentGroup replicationGroup, ComponentSystemBase system, global::Improbable.Worker.Core.Connection connection)
            {
                Profiler.BeginSample("Connection");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.Temp);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>();
                var eventMyEventType = system.GetArchetypeChunkComponentType<EventSender.MyEvent>(true);
                foreach (var chunk in chunkArray)
                {
                    var entityIdArray = chunk.GetNativeArray(spatialOSEntityType);
                    var componentArray = chunk.GetNativeArray(componentType);
                    var eventMyEventArray = chunk.GetNativeArray(eventMyEventType);
                    for (var i = 0; i < componentArray.Length; i++)
                    {
                        var data = componentArray[i];
                        var dirtyEvents = 0;
                        var eventsMyEvent = eventMyEventArray[i].Events;
                        dirtyEvents += eventsMyEvent.Count;

                        if (data.DirtyBit || dirtyEvents > 0)
                        {
                            var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(1105);
                            Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Serialization.SerializeUpdate(data, update);

                            // Serialize events
                            var eventsObject = update.GetEvents();
                            if (eventsMyEvent.Count > 0)
                            {
                                foreach (var e in eventsMyEvent)
                                {
                                    var obj = eventsObject.AddObject(1);
                                    global::Improbable.Gdk.Tests.AlternateSchemaSyntax.RandomDataType.Serialization.Serialize(e, obj);
                                }

                                eventsMyEvent.Clear();
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
            }
        }

        internal class ComponentCleanup : ComponentCleanupHandler
        {
            public override EntityArchetypeQuery CleanupArchetypeQuery => new EntityArchetypeQuery
            {
                All = Array.Empty<ComponentType>(),
                Any = new ComponentType[]
                {
                    ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                    ComponentType.Create<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(),
                    ComponentType.Create<ReceivedEvents.MyEvent>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>();
                var myEventEventType = system.GetArchetypeChunkComponentType<ReceivedEvents.MyEvent>();

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
                            buffer.RemoveComponent<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.Gdk.Tests.AlternateSchemaSyntax.Connection.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                    // MyEvent Event
                    if (chunk.Has(myEventEventType))
                    {
                        var myEventEventArray = chunk.GetNativeArray(myEventEventType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ReceivedEvents.MyEvent>(entities[i]);
                            ReferenceTypeProviders.MyEventProvider.Free(myEventEventArray[i].handle);
                        }
                    }

                }

                chunkArray.Dispose();
            }
        }
    }
}
