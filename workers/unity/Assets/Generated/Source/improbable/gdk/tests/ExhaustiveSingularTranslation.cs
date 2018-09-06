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
    public partial class ExhaustiveSingular
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 197715;

            private readonly EntityManager entityManager;

            private const string LoggerName = "ExhaustiveSingular.DispatcherHandler";


            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
                ExhaustiveSingular.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);

                ExhaustiveSingular.ReferenceTypeProviders.Field3Provider.CleanDataInWorld(World);
                ExhaustiveSingular.ReferenceTypeProviders.Field7Provider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "AddComponentOp", out var entity))
                {
                    return;
                }

                var data = Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>());

                var update = new Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Update
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

                var updates = new List<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Update>
                {
                    update
                };

                var updatesComponent = new Generated.Improbable.Gdk.Tests.ExhaustiveSingular.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentAdded<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Generated.Improbable.Gdk.Tests.ExhaustiveSingular")
                    );
                }
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "RemoveComponentOp", out var entity))
                {
                    return;
                }

                var data = entityManager.GetComponentData<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>(entity);
                ExhaustiveSingular.ReferenceTypeProviders.Field3Provider.Free(data.field3Handle);
                ExhaustiveSingular.ReferenceTypeProviders.Field7Provider.Free(data.field7Handle);

                entityManager.RemoveComponent<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                {
                    entityManager.AddComponent(entity, ComponentType.Create<ComponentRemoved<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Generated.Improbable.Gdk.Tests.ExhaustiveSingular")
                    );
                }
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                if (!IsValidEntityId(op.EntityId, "OnComponentUpdate", out var entity))
                {
                    return;
                }

                if (entityManager.HasComponent<NotAuthoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>(entity);

                    var update = Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Serialization.GetAndApplyUpdate(op.Update.SchemaData.Value.GetFields(), ref data);

                    List<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Update> updates;
                    if (entityManager.HasComponent<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.ReceivedUpdates>(entity))
                    {
                        updates = entityManager.GetComponentData<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.ReceivedUpdates>(entity).Updates;

                    }
                    else
                    {
                        var updatesComponent = new Generated.Improbable.Gdk.Tests.ExhaustiveSingular.ReceivedUpdates
                        {
                            handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                        };
                        ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, new List<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Update>());
                        updates = updatesComponent.Updates;
                        entityManager.AddComponentData(entity, updatesComponent);
                    }

                    updates.Add(update);

                    data.DirtyBit = false;
                    entityManager.SetComponentData(entity, data);
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
                            .WithField("Component", "Generated.Improbable.Gdk.Tests.ExhaustiveSingular")
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
                            .WithField("Component", "Generated.Improbable.Gdk.Tests.ExhaustiveSingular")
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
                        if (!entityManager.HasComponent<NotAuthoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<Authoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponent(entity, ComponentType.Create<AuthorityLossImminent<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity);
                        entityManager.AddComponent(entity, ComponentType.Create<NotAuthoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>
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
                        .WithField("Component", "Generated.Improbable.Gdk.Tests.ExhaustiveSingular")
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
                    .WithField("Component", "Generated.Improbable.Gdk.Tests.ExhaustiveSingular")
                );
            }

        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 197715;

            public override ComponentType[] ReplicationComponentTypes => new ComponentType[] {
                ComponentType.Create<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>(),
                ComponentType.ReadOnly<Authoritative<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(),
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
                var componentDataArray = replicationGroup.GetComponentDataArray<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var data = componentDataArray[i];
                    var dirtyEvents = 0;

                    if (data.DirtyBit || dirtyEvents > 0)
                    {
                        var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(197715);
                        Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Serialization.Serialize(data, update.GetFields());

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
                ComponentType.ReadOnly<ComponentAdded<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(),
                ComponentType.ReadOnly<ComponentRemoved<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(),
            };

            public override ComponentType[] EventComponentTypes => new ComponentType[] {
            };

            public override ComponentType ComponentUpdateType => ComponentType.ReadOnly<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.ReceivedUpdates>();
            public override ComponentType AuthorityChangesType => ComponentType.ReadOnly<AuthorityChanges<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>();

            public override ComponentType[] CommandReactiveTypes => new ComponentType[] {
            };

            public override void CleanupUpdates(ComponentGroup updateGroup, ref EntityCommandBuffer buffer)
            {
                var entities = updateGroup.GetEntityArray();
                var data = updateGroup.GetComponentDataArray<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.ReceivedUpdates>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.ReceivedUpdates>(entities[i]);
                    ReferenceTypeProviders.UpdatesProvider.Free(data[i].handle);
                }
            }

            public override void CleanupAuthChanges(ComponentGroup authorityChangeGroup, ref EntityCommandBuffer buffer)
            {
                var entities = authorityChangeGroup.GetEntityArray();
                var data = authorityChangeGroup.GetComponentDataArray<AuthorityChanges<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<AuthorityChanges<Generated.Improbable.Gdk.Tests.ExhaustiveSingular.Component>>(entities[i]);
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
