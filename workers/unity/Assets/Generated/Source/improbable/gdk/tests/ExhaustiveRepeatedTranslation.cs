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

namespace Generated.Improbable.Gdk.Tests
{
    public partial class ExhaustiveRepeated
    {
        public class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 197717;

            private readonly EntityManager entityManager;

            private const string LoggerName = "ExhaustiveRepeated.DispatcherHandler";


            public DispatcherHandler(Worker worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
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
                if (!IsValidEntityId(op.EntityId, "AddComponentOp", out var entity))
                {
                    return;
                }

                var data = global::Generated.Improbable.Gdk.Tests.SpatialOSExhaustiveRepeated.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponentData(entity, new NotAuthoritative<SpatialOSExhaustiveRepeated>());

                if (entityManager.HasComponent<ComponentRemoved<SpatialOSExhaustiveRepeated>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<SpatialOSExhaustiveRepeated>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<SpatialOSExhaustiveRepeated>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentAdded<SpatialOSExhaustiveRepeated>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "SpatialOSExhaustiveRepeated")
                    );
                }
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "RemoveComponentOp", out var entity))
                {
                    return;
                }

                var data = entityManager.GetComponentData<SpatialOSExhaustiveRepeated>(entity);
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

                entityManager.RemoveComponent<SpatialOSExhaustiveRepeated>(entity);

                if (entityManager.HasComponent<ComponentAdded<SpatialOSExhaustiveRepeated>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<SpatialOSExhaustiveRepeated>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<SpatialOSExhaustiveRepeated>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentRemoved<SpatialOSExhaustiveRepeated>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "SpatialOSExhaustiveRepeated")
                    );
                }
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                if (!IsValidEntityId(op.EntityId, "OnComponentUpdate", out var entity))
                {
                    return;
                }

                var data = entityManager.GetComponentData<SpatialOSExhaustiveRepeated>(entity);

                var update = global::Generated.Improbable.Gdk.Tests.SpatialOSExhaustiveRepeated.Serialization.GetAndApplyUpdate(op.Update.SchemaData.Value.GetFields(), ref data);

                List<SpatialOSExhaustiveRepeated.Update> updates;
                if (entityManager.HasComponent<SpatialOSExhaustiveRepeated.ReceivedUpdates>(entity))
                {
                    updates = entityManager.GetComponentData<SpatialOSExhaustiveRepeated.ReceivedUpdates>(entity).Updates;

                }
                else
                {
                    var updatesComponent = new SpatialOSExhaustiveRepeated.ReceivedUpdates
                    {
                        handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                    };
                    ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, new List<SpatialOSExhaustiveRepeated.Update>());
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
                    default:
                        LogDispatcher.HandleLog(LogType.Error, new LogEvent(CommandIndexNotFound)
                            .WithField(LoggingUtils.LoggerName, LoggerName)
                            .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                            .WithField("CommandIndex", commandIndex)
                            .WithField("Component", "SpatialOSExhaustiveRepeated")
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
                            .WithField("Component", "SpatialOSExhaustiveRepeated")
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
                        if (!entityManager.HasComponent<NotAuthoritative<SpatialOSExhaustiveRepeated>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<SpatialOSExhaustiveRepeated>>(entity);
                        entityManager.AddComponentData(entity, new Authoritative<SpatialOSExhaustiveRepeated>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<SpatialOSExhaustiveRepeated>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponentData(entity, new AuthorityLossImminent<SpatialOSExhaustiveRepeated>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<SpatialOSExhaustiveRepeated>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<SpatialOSExhaustiveRepeated>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<SpatialOSExhaustiveRepeated>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<SpatialOSExhaustiveRepeated>>(entity);
                        entityManager.AddComponentData(entity, new NotAuthoritative<SpatialOSExhaustiveRepeated>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<SpatialOSExhaustiveRepeated>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<SpatialOSExhaustiveRepeated>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<SpatialOSExhaustiveRepeated>
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
                        .WithField("Component", "SpatialOSExhaustiveRepeated")
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
                    .WithField("Component", "SpatialOSExhaustiveRepeated")
                );
            }

        }

        public class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 197717;

            public override ComponentType[] ReplicationComponentTypes => new ComponentType[] {
                ComponentType.Create<SpatialOSExhaustiveRepeated>(),
                ComponentType.ReadOnly<Authoritative<SpatialOSExhaustiveRepeated>>(),
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
                var componentDataArray = replicationGroup.GetComponentDataArray<SpatialOSExhaustiveRepeated>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var data = componentDataArray[i];
                    var dirtyEvents = 0;

                    if (data.DirtyBit || dirtyEvents > 0)
                    {
                        var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(197717);
                        SpatialOSExhaustiveRepeated.Serialization.Serialize(data, update.GetFields());

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
                ComponentType.ReadOnly<ComponentAdded<SpatialOSExhaustiveRepeated>>(),
                ComponentType.ReadOnly<ComponentRemoved<SpatialOSExhaustiveRepeated>>(),
            };

            public override ComponentType[] EventComponentTypes => new ComponentType[] {
            };

            public override ComponentType ComponentUpdateType => ComponentType.ReadOnly<SpatialOSExhaustiveRepeated.ReceivedUpdates>();
            public override ComponentType AuthorityChangesType => ComponentType.ReadOnly<AuthorityChanges<SpatialOSExhaustiveRepeated>>();

            public override ComponentType[] CommandReactiveTypes => new ComponentType[] {
            };

            public override void CleanupUpdates(ComponentGroup updateGroup, ref EntityCommandBuffer buffer)
            {
                var entities = updateGroup.GetEntityArray();
                var data = updateGroup.GetComponentDataArray<SpatialOSExhaustiveRepeated.ReceivedUpdates>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<SpatialOSExhaustiveRepeated.ReceivedUpdates>(entities[i]);
                    ReferenceTypeProviders.UpdatesProvider.Free(data[i].handle);
                }
            }

            public override void CleanupAuthChanges(ComponentGroup authorityChangeGroup, ref EntityCommandBuffer buffer)
            {
                var entities = authorityChangeGroup.GetEntityArray();
                var data = authorityChangeGroup.GetComponentDataArray<AuthorityChanges<SpatialOSExhaustiveRepeated>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<AuthorityChanges<SpatialOSExhaustiveRepeated>>(entities[i]);
                    AuthorityChangesProvider.Free(data[i].Handle);
                }
            }

            public override void CleanupEvents(ComponentGroup[] eventGroups, ref EntityCommandBuffer buffer)
            {
            }

            public override void CleanupCommands(ComponentGroup[] commandCleanupGroups, ref EntityCommandBuffer buffer)
            {
            }
        }
    }

}
