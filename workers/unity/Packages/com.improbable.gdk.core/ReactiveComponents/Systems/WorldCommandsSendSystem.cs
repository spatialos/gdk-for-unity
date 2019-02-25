using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.ReactiveComponents
{
    /// <summary>
    ///     Sends World Command requests.
    /// </summary>
    [DisableAutoCreation]
    [AlwaysUpdateSystem]
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class WorldCommandsSendSystem : ComponentSystem
    {
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

        private IConnectionHandler connection;
        private CommandSystem commandSystem;
        private ComponentGroup group;

        protected override void OnCreateManager()
        {
            base.OnCreateManager();
            connection = World.GetExistingManager<WorkerSystem>().ConnectionHandler;
            commandSystem = World.GetExistingManager<CommandSystem>();
            group = GetComponentGroup(worldCommandSendersQuery);
        }

        protected override void OnUpdate()
        {
            if (!connection.IsConnected())
            {
                return;
            }

            var entityType = GetArchetypeChunkEntityType();

            var createEntityType = GetArchetypeChunkComponentType<WorldCommands.CreateEntity.CommandSender>();
            var deleteEntityType = GetArchetypeChunkComponentType<WorldCommands.DeleteEntity.CommandSender>();
            var reserveEntityIdsType = GetArchetypeChunkComponentType<WorldCommands.ReserveEntityIds.CommandSender>();
            var entityQueryType = GetArchetypeChunkComponentType<WorldCommands.EntityQuery.CommandSender>();

            var chunkArray = group.CreateArchetypeChunkArray(Allocator.TempJob);

            foreach (var chunk in chunkArray)
            {
                var entityArray = chunk.GetNativeArray(entityType);

                var createEntitySenders = chunk.GetNativeArray(createEntityType);
                for (var i = 0; i < createEntitySenders.Length; ++i)
                {
                    var requestsToSend = createEntitySenders[i].RequestsToSend;
                    var entity = entityArray[i];
                    foreach (var req in requestsToSend)
                    {
                        commandSystem.SendCommand(req, entity);
                    }

                    requestsToSend.Clear();
                }

                var deleteEntitySenders = chunk.GetNativeArray(deleteEntityType);
                for (var i = 0; i < deleteEntitySenders.Length; ++i)
                {
                    var requestsToSend = deleteEntitySenders[i].RequestsToSend;
                    var entity = entityArray[i];
                    foreach (var req in requestsToSend)
                    {
                        commandSystem.SendCommand(req, entity);
                    }

                    requestsToSend.Clear();
                }

                var reserveEntityIdsSenders = chunk.GetNativeArray(reserveEntityIdsType);
                for (var i = 0; i < reserveEntityIdsSenders.Length; ++i)
                {
                    var requestsToSend = reserveEntityIdsSenders[i].RequestsToSend;
                    var entity = entityArray[i];
                    foreach (var req in requestsToSend)
                    {
                        commandSystem.SendCommand(req, entity);
                    }

                    requestsToSend.Clear();
                }

                var entityQuerySenders = chunk.GetNativeArray(entityQueryType);
                for (var i = 0; i < entityQuerySenders.Length; ++i)
                {
                    var requestsToSend = entityQuerySenders[i].RequestsToSend;
                    var entity = entityArray[i];
                    foreach (var req in requestsToSend)
                    {
                        commandSystem.SendCommand(req, entity);
                    }

                    requestsToSend.Clear();
                }
            }

            chunkArray.Dispose();
        }
    }
}
