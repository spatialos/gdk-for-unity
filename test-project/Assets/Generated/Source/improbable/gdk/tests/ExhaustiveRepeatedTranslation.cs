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

namespace Improbable.Gdk.Tests
{
    public partial class ExhaustiveRepeated
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 197717;

            private readonly EntityManager entityManager;

            private const string LoggerName = "ExhaustiveRepeated.DispatcherHandler";


            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
                ExhaustiveRepeated.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);

                ExhaustiveRepeated.ReferenceTypeProviders.Field1Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field2Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field3Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field4Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field5Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field6Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field7Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field8Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field9Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field10Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field11Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field12Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field13Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field14Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field15Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field16Provider.CleanDataInWorld(World);
                ExhaustiveRepeated.ReferenceTypeProviders.Field17Provider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("ExhaustiveRepeated");
                var data = Improbable.Gdk.Tests.ExhaustiveRepeated.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>());

                var update = new Improbable.Gdk.Tests.ExhaustiveRepeated.Update
                {
                    Field1 = data.Field1,
                    Field2 = data.Field2,
                    Field3 = data.Field3,
                    Field4 = data.Field4,
                    Field5 = data.Field5,
                    Field6 = data.Field6,
                    Field7 = data.Field7,
                    Field8 = data.Field8,
                    Field9 = data.Field9,
                    Field10 = data.Field10,
                    Field11 = data.Field11,
                    Field12 = data.Field12,
                    Field13 = data.Field13,
                    Field14 = data.Field14,
                    Field15 = data.Field15,
                    Field16 = data.Field16,
                    Field17 = data.Field17,
                };

                var updates = new List<Improbable.Gdk.Tests.ExhaustiveRepeated.Update>
                {
                    update
                };

                var updatesComponent = new Improbable.Gdk.Tests.ExhaustiveRepeated.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.ExhaustiveRepeated")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("ExhaustiveRepeated");

                var data = entityManager.GetComponentData<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>(entity);
                ExhaustiveRepeated.ReferenceTypeProviders.Field1Provider.Free(data.field1Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field2Provider.Free(data.field2Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field3Provider.Free(data.field3Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field4Provider.Free(data.field4Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field5Provider.Free(data.field5Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field6Provider.Free(data.field6Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field7Provider.Free(data.field7Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field8Provider.Free(data.field8Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field9Provider.Free(data.field9Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field10Provider.Free(data.field10Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field11Provider.Free(data.field11Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field12Provider.Free(data.field12Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field13Provider.Free(data.field13Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field14Provider.Free(data.field14Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field15Provider.Free(data.field15Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field16Provider.Free(data.field16Handle);
                ExhaustiveRepeated.ReferenceTypeProviders.Field17Provider.Free(data.field17Handle);

                entityManager.RemoveComponent<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Improbable.Gdk.Tests.ExhaustiveRepeated")
                    );
                }

                Profiler.EndSample();
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                var entity = TryGetEntityFromEntityId(op.EntityId);

                Profiler.BeginSample("ExhaustiveRepeated");
                if (entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>(entity);
                    Improbable.Gdk.Tests.ExhaustiveRepeated.Serialization.ApplyUpdate(op.Update.SchemaData.Value, ref data);
                    data.DirtyBit = false;
                    entityManager.SetComponentData(entity, data);
                }

                var update = Improbable.Gdk.Tests.ExhaustiveRepeated.Serialization.DeserializeUpdate(op.Update.SchemaData.Value);

                List<Improbable.Gdk.Tests.ExhaustiveRepeated.Update> updates;
                if (entityManager.HasComponent<Improbable.Gdk.Tests.ExhaustiveRepeated.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<Improbable.Gdk.Tests.ExhaustiveRepeated.ReceivedUpdates>(entity).Updates;
                }
                else
                {
                    updates = Improbable.Gdk.Tests.ExhaustiveRepeated.Update.Pool.Count > 0 ? Improbable.Gdk.Tests.ExhaustiveRepeated.Update.Pool.Pop() : new List<Improbable.Gdk.Tests.ExhaustiveRepeated.Update>();
                    var updatesComponent = new Improbable.Gdk.Tests.ExhaustiveRepeated.ReceivedUpdates
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

                Profiler.BeginSample("ExhaustiveRepeated");
                ApplyAuthorityChange(entity, op.Authority, op.EntityId);
                Profiler.EndSample();
            }

            public override void OnCommandRequest(CommandRequestOp op)
            {
                var commandIndex = op.Request.SchemaData.Value.GetCommandIndex();
                throw new UnknownCommandIndexException(commandIndex, "ExhaustiveRepeated");
            }

            public override void OnCommandResponse(CommandResponseOp op)
            {
                var commandIndex = op.Response.CommandIndex;
                throw new UnknownCommandIndexException(commandIndex, "ExhaustiveRepeated");
            }

            public override void AddCommandComponents(Unity.Entities.Entity entity)
            {
            }

            private void ApplyAuthorityChange(Unity.Entities.Entity entity, Authority authority, global::Improbable.Worker.EntityId entityId)
            {
                switch (authority)
                {
                    case Authority.Authoritative:
                        if (!entityManager.HasComponent<NotAuthoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>
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
                    .WithField("Component", "Improbable.Gdk.Tests.ExhaustiveRepeated")
                );
            }
        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 197717;

            public override EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
            {
                All = new[]
                {
                    ComponentType.Create<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>(),
                    ComponentType.ReadOnly<Authoritative<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(),
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
                Profiler.BeginSample("ExhaustiveRepeated");

                var chunkArray = replicationGroup.CreateArchetypeChunkArray(Allocator.Temp);
                var spatialOSEntityType = system.GetArchetypeChunkComponentType<SpatialEntityId>(true);
                var componentType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>();
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
                            var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(197717);
                            Improbable.Gdk.Tests.ExhaustiveRepeated.Serialization.SerializeUpdate(data, update);

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
                    ComponentType.Create<ComponentAdded<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(),
                    ComponentType.Create<ComponentRemoved<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(),
                    ComponentType.Create<Improbable.Gdk.Tests.ExhaustiveRepeated.ReceivedUpdates>(),
                    ComponentType.Create<AuthorityChanges<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(),
                },
                None = Array.Empty<ComponentType>(),
            };

            public override void CleanComponents(ComponentGroup group, ComponentSystemBase system,
                EntityCommandBuffer buffer)
            {
                var entityType = system.GetArchetypeChunkEntityType();
                var componentAddedType = system.GetArchetypeChunkComponentType<ComponentAdded<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>();
                var componentRemovedType = system.GetArchetypeChunkComponentType<ComponentRemoved<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>();
                var receivedUpdateType = system.GetArchetypeChunkComponentType<Improbable.Gdk.Tests.ExhaustiveRepeated.ReceivedUpdates>();
                var authorityChangeType = system.GetArchetypeChunkComponentType<AuthorityChanges<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>();

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
                            buffer.RemoveComponent<Improbable.Gdk.Tests.ExhaustiveRepeated.ReceivedUpdates>(entities[i]);
                            var updateList = updateArray[i].Updates;

                            // Pool update lists to avoid excessive allocation
                            updateList.Clear();
                            Improbable.Gdk.Tests.ExhaustiveRepeated.Update.Pool.Push(updateList);

                            ReferenceTypeProviders.UpdatesProvider.Free(updateArray[i].handle);
                        }
                    }

                    // Component Added
                    if (chunk.Has(componentAddedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentAdded<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entities[i]);
                        }
                    }

                    // Component Removed
                    if (chunk.Has(componentRemovedType))
                    {
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<ComponentRemoved<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entities[i]);
                        }
                    }

                    // Authority
                    if (chunk.Has(authorityChangeType))
                    {
                        var authorityChangeArray = chunk.GetNativeArray(authorityChangeType);
                        for (int i = 0; i < entities.Length; ++i)
                        {
                            buffer.RemoveComponent<AuthorityChanges<Improbable.Gdk.Tests.ExhaustiveRepeated.Component>>(entities[i]);
                            AuthorityChangesProvider.Free(authorityChangeArray[i].Handle);
                        }
                    }

                }

                chunkArray.Dispose();
            }
        }
    }
}
