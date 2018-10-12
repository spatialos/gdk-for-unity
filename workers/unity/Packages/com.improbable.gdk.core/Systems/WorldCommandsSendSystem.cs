using System;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.Core;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Sends World Command requests.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class WorldCommandsSendSystem : ComponentSystem
    {
        private Connection connection;

        private readonly EntityArchetypeQuery worldCommandSendersQuery = new EntityArchetypeQuery
        {
            All = new[]
            {
                ComponentType.Create<WorldCommands.CreateEntity.CommandSender>(),
                ComponentType.Create<WorldCommands.DeleteEntity.CommandSender>(),
                ComponentType.Create<WorldCommands.ReserveEntityIds.CommandSender>(),
                ComponentType.Create<WorldCommands.EntityQuery.CommandSender>(),
            },
            Any = Array.Empty<ComponentType>(),
            None = Array.Empty<ComponentType>(),
        };

        private ComponentGroup group;

        private WorldCommands.CreateEntity.Storage createEntityStorage;
        private WorldCommands.DeleteEntity.Storage deleteEntityStorage;
        private WorldCommands.ReserveEntityIds.Storage reserveEntityIdsStorage;
        private WorldCommands.EntityQuery.Storage entityQueryStorage;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            connection = World.GetExistingManager<WorkerSystem>().Connection;

            group = GetComponentGroup(worldCommandSendersQuery);

            var requestTracker = World.GetOrCreateManager<CommandRequestTrackerSystem>();
            createEntityStorage = requestTracker.GetCommandStorageForType<WorldCommands.CreateEntity.Storage>();
            deleteEntityStorage = requestTracker.GetCommandStorageForType<WorldCommands.DeleteEntity.Storage>();
            reserveEntityIdsStorage = requestTracker.GetCommandStorageForType<WorldCommands.ReserveEntityIds.Storage>();
            entityQueryStorage = requestTracker.GetCommandStorageForType<WorldCommands.EntityQuery.Storage>();
        }

        protected override void OnUpdate()
        {
            if (connection == null)
            {
                return;
            }

            var entityType = GetArchetypeChunkEntityType();

            var createEntityType = GetArchetypeChunkComponentType<WorldCommands.CreateEntity.CommandSender>();
            var deleteEntityType = GetArchetypeChunkComponentType<WorldCommands.DeleteEntity.CommandSender>();
            var reserveEntityIdsType = GetArchetypeChunkComponentType<WorldCommands.ReserveEntityIds.CommandSender>();
            var entityQueryType = GetArchetypeChunkComponentType<WorldCommands.EntityQuery.CommandSender>();

            var chunkArray = group.CreateArchetypeChunkArray(Allocator.Temp);

            foreach (var chunk in chunkArray)
            {
                var entityArray = chunk.GetNativeArray(entityType);

                var createEntitySenders = chunk.GetNativeArray(createEntityType);
                for (int i = 0; i < createEntitySenders.Length; ++i)
                {
                    var requestsToSend = createEntitySenders[i].RequestsToSend;
                    var entity = entityArray[i];
                    foreach (var req in requestsToSend)
                    {
                        var reqId = connection.SendCreateEntityRequest(req.Entity, req.EntityId, req.TimeoutMillis);
                        createEntityStorage.CommandRequestsInFlight.Add(reqId.Id,
                            new CommandRequestStore<WorldCommands.CreateEntity.Request>(entity, req, req.Context, req.RequestId));
                    }

                    requestsToSend.Clear();
                }

                var deleteEntitySenders = chunk.GetNativeArray(deleteEntityType);
                for (int i = 0; i < deleteEntitySenders.Length; ++i)
                {
                    var requestsToSend = deleteEntitySenders[i].RequestsToSend;
                    var entity = entityArray[i];
                    foreach (var req in requestsToSend)
                    {
                        var reqId = connection.SendDeleteEntityRequest(req.EntityId, req.TimeoutMillis);
                        deleteEntityStorage.CommandRequestsInFlight.Add(reqId.Id,
                            new CommandRequestStore<WorldCommands.DeleteEntity.Request>(entity, req, req.Context, req.RequestId));
                    }

                    requestsToSend.Clear();
                }

                var reserveEntityIdsSenders = chunk.GetNativeArray(reserveEntityIdsType);
                for (int i = 0; i < reserveEntityIdsSenders.Length; ++i)
                {
                    var requestsToSend = reserveEntityIdsSenders[i].RequestsToSend;
                    var entity = entityArray[i];
                    foreach (var req in requestsToSend)
                    {
                        var reqId = connection.SendReserveEntityIdsRequest(req.NumberOfEntityIds, req.TimeoutMillis);
                        reserveEntityIdsStorage.CommandRequestsInFlight.Add(reqId.Id,
                            new CommandRequestStore<WorldCommands.ReserveEntityIds.Request>(entity, req, req.Context, req.RequestId));
                    }

                    requestsToSend.Clear();
                }

                var entityQuerySenders = chunk.GetNativeArray(entityQueryType);
                for (int i = 0; i < entityQuerySenders.Length; ++i)
                {
                    var requestsToSend = entityQuerySenders[i].RequestsToSend;
                    var entity = entityArray[i];
                    foreach (var req in requestsToSend)
                    {
                        var reqId = connection.SendEntityQueryRequest(req.EntityQuery, req.TimeoutMillis);
                        entityQueryStorage.CommandRequestsInFlight.Add(reqId.Id,
                            new CommandRequestStore<WorldCommands.EntityQuery.Request>(entity, req, req.Context, req.RequestId));
                    }

                    requestsToSend.Clear();
                }
            }

            chunkArray.Dispose();
        }
    }
}
