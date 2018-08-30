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

namespace Generated.Improbable
{
    public partial class Metadata
    {
        internal class DispatcherHandler : ComponentDispatcherHandler
        {
            public override uint ComponentId => 53;

            private readonly EntityManager entityManager;

            private const string LoggerName = "Metadata.DispatcherHandler";


            public DispatcherHandler(WorkerSystem worker, World world) : base(worker, world)
            {
                entityManager = world.GetOrCreateManager<EntityManager>();
                var bookkeepingSystem = world.GetOrCreateManager<CommandRequestTrackerSystem>();
            }

            public override void Dispose()
            {
                Metadata.ReferenceTypeProviders.UpdatesProvider.CleanDataInWorld(World);

                Metadata.ReferenceTypeProviders.EntityTypeProvider.CleanDataInWorld(World);
            }

            public override void OnAddComponent(AddComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "AddComponentOp", out var entity))
                {
                    return;
                }

                var data = Generated.Improbable.Metadata.Serialization.Deserialize(op.Data.SchemaData.Value.GetFields(), World);
                data.DirtyBit = false;
                entityManager.AddComponentData(entity, data);
                entityManager.AddComponentData(entity, new NotAuthoritative<Generated.Improbable.Metadata.Component>());

                var update = new Generated.Improbable.Metadata.Update
                {
                    EntityType = data.EntityType,
                };

                var updates = new List<Generated.Improbable.Metadata.Update>
                {
                    update
                };

                var updatesComponent = new Generated.Improbable.Metadata.ReceivedUpdates
                {
                    handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                };

                ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, updates);
                entityManager.AddComponentData(entity, updatesComponent);

                if (entityManager.HasComponent<ComponentRemoved<Generated.Improbable.Metadata.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentRemoved<Generated.Improbable.Metadata.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentAdded<Generated.Improbable.Metadata.Component>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentAdded<Generated.Improbable.Metadata.Component>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentAdded)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Generated.Improbable.Metadata")
                    );
                }
            }

            public override void OnRemoveComponent(RemoveComponentOp op)
            {
                if (!IsValidEntityId(op.EntityId, "RemoveComponentOp", out var entity))
                {
                    return;
                }

                var data = entityManager.GetComponentData<Generated.Improbable.Metadata.Component>(entity);
                Metadata.ReferenceTypeProviders.EntityTypeProvider.Free(data.entityTypeHandle);

                entityManager.RemoveComponent<Generated.Improbable.Metadata.Component>(entity);

                if (entityManager.HasComponent<ComponentAdded<Generated.Improbable.Metadata.Component>>(entity))
                {
                    entityManager.RemoveComponent<ComponentAdded<Generated.Improbable.Metadata.Component>>(entity);
                }
                else if (!entityManager.HasComponent<ComponentRemoved<Generated.Improbable.Metadata.Component>>(entity))
                {
                    entityManager.AddComponentData(entity, new ComponentRemoved<Generated.Improbable.Metadata.Component>());
                }
                else
                {
                    LogDispatcher.HandleLog(LogType.Error, new LogEvent(ReceivedDuplicateComponentRemoved)
                        .WithField(LoggingUtils.LoggerName, LoggerName)
                        .WithField(LoggingUtils.EntityId, op.EntityId.Id)
                        .WithField("Component", "Generated.Improbable.Metadata")
                    );
                }
            }

            public override void OnComponentUpdate(ComponentUpdateOp op)
            {
                if (!IsValidEntityId(op.EntityId, "OnComponentUpdate", out var entity))
                {
                    return;
                }

                if (entityManager.HasComponent<NotAuthoritative<Generated.Improbable.Metadata.Component>>(entity))
                {
                    var data = entityManager.GetComponentData<Generated.Improbable.Metadata.Component>(entity);

                    var update = Generated.Improbable.Metadata.Serialization.GetAndApplyUpdate(op.Update.SchemaData.Value.GetFields(), ref data);

                    List<Generated.Improbable.Metadata.Update> updates;
                    if (entityManager.HasComponent<Generated.Improbable.Metadata.ReceivedUpdates>(entity))
                    {
                        updates = entityManager.GetComponentData<Generated.Improbable.Metadata.ReceivedUpdates>(entity).Updates;

                    }
                    else
                    {
                        var updatesComponent = new Generated.Improbable.Metadata.ReceivedUpdates
                        {
                            handle = ReferenceTypeProviders.UpdatesProvider.Allocate(World)
                        };
                        ReferenceTypeProviders.UpdatesProvider.Set(updatesComponent.handle, new List<Generated.Improbable.Metadata.Update>());
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
                            .WithField("Component", "Generated.Improbable.Metadata")
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
                            .WithField("Component", "Generated.Improbable.Metadata")
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
                        if (!entityManager.HasComponent<NotAuthoritative<Generated.Improbable.Metadata.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.Authoritative, Authority.NotAuthoritative, entityId);
                            return;
                        }

                        entityManager.RemoveComponent<NotAuthoritative<Generated.Improbable.Metadata.Component>>(entity);
                        entityManager.AddComponentData(entity, new Authoritative<Generated.Improbable.Metadata.Component>());

                        // Add event senders
                        break;
                    case Authority.AuthorityLossImminent:
                        if (!entityManager.HasComponent<Authoritative<Generated.Improbable.Metadata.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.AuthorityLossImminent, Authority.Authoritative, entityId);
                            return;
                        }

                        entityManager.AddComponentData(entity, new AuthorityLossImminent<Generated.Improbable.Metadata.Component>());
                        break;
                    case Authority.NotAuthoritative:
                        if (!entityManager.HasComponent<Authoritative<Generated.Improbable.Metadata.Component>>(entity))
                        {
                            LogInvalidAuthorityTransition(Authority.NotAuthoritative, Authority.Authoritative, entityId);
                            return;
                        }

                        if (entityManager.HasComponent<AuthorityLossImminent<Generated.Improbable.Metadata.Component>>(entity))
                        {
                            entityManager.RemoveComponent<AuthorityLossImminent<Generated.Improbable.Metadata.Component>>(entity);
                        }

                        entityManager.RemoveComponent<Authoritative<Generated.Improbable.Metadata.Component>>(entity);
                        entityManager.AddComponentData(entity, new NotAuthoritative<Generated.Improbable.Metadata.Component>());

                        // Remove event senders
                        break;
                }

                List<Authority> authorityChanges;
                if (entityManager.HasComponent<AuthorityChanges<Generated.Improbable.Metadata.Component>>(entity))
                {
                    authorityChanges = entityManager.GetComponentData<AuthorityChanges<Generated.Improbable.Metadata.Component>>(entity).Changes;

                }
                else
                {
                    var changes = new AuthorityChanges<Generated.Improbable.Metadata.Component>
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
                        .WithField("Component", "Generated.Improbable.Metadata")
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
                    .WithField("Component", "Generated.Improbable.Metadata")
                );
            }

        }

        internal class ComponentReplicator : ComponentReplicationHandler
        {
            public override uint ComponentId => 53;

            public override ComponentType[] ReplicationComponentTypes => new ComponentType[] {
                ComponentType.Create<Generated.Improbable.Metadata.Component>(),
                ComponentType.ReadOnly<Authoritative<Generated.Improbable.Metadata.Component>>(),
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
                var componentDataArray = replicationGroup.GetComponentDataArray<Generated.Improbable.Metadata.Component>();

                for (var i = 0; i < componentDataArray.Length; i++)
                {
                    var data = componentDataArray[i];
                    var dirtyEvents = 0;

                    if (data.DirtyBit || dirtyEvents > 0)
                    {
                        var update = new global::Improbable.Worker.Core.SchemaComponentUpdate(53);
                        Generated.Improbable.Metadata.Serialization.Serialize(data, update.GetFields());

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
                ComponentType.ReadOnly<ComponentAdded<Generated.Improbable.Metadata.Component>>(),
                ComponentType.ReadOnly<ComponentRemoved<Generated.Improbable.Metadata.Component>>(),
            };

            public override ComponentType[] EventComponentTypes => new ComponentType[] {
            };

            public override ComponentType ComponentUpdateType => ComponentType.ReadOnly<Generated.Improbable.Metadata.ReceivedUpdates>();
            public override ComponentType AuthorityChangesType => ComponentType.ReadOnly<AuthorityChanges<Generated.Improbable.Metadata.Component>>();

            public override ComponentType[] CommandReactiveTypes => new ComponentType[] {
            };

            public override void CleanupUpdates(ComponentGroup updateGroup, ref EntityCommandBuffer buffer)
            {
                var entities = updateGroup.GetEntityArray();
                var data = updateGroup.GetComponentDataArray<Generated.Improbable.Metadata.ReceivedUpdates>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<Generated.Improbable.Metadata.ReceivedUpdates>(entities[i]);
                    ReferenceTypeProviders.UpdatesProvider.Free(data[i].handle);
                }
            }

            public override void CleanupAuthChanges(ComponentGroup authorityChangeGroup, ref EntityCommandBuffer buffer)
            {
                var entities = authorityChangeGroup.GetEntityArray();
                var data = authorityChangeGroup.GetComponentDataArray<AuthorityChanges<Generated.Improbable.Metadata.Component>>();
                for (var i = 0; i < entities.Length; i++)
                {
                    buffer.RemoveComponent<AuthorityChanges<Generated.Improbable.Metadata.Component>>(entities[i]);
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
