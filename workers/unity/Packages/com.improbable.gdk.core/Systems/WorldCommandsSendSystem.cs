using Improbable.Gdk.Core.Commands;
using Unity.Collections;
using Unity.Entities;

namespace Improbable.Gdk.Core
{
    [UpdateInGroup(typeof(SpatialOSSendGroup.InternalSpatialOSSendGroup))]
    public class WorldCommandsSendSystem : ComponentSystem
    {
        private Worker worker;

        private struct CreateEntitySenderData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.CreateEntity.CommandSender> Senders;
        }

        [Inject] private CreateEntitySenderData createEntitySenderData;

        private struct DeleteEntitySenderData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.DeleteEntity.CommandSender> Senders;
        }

        [Inject] private DeleteEntitySenderData deleteEntitySenderData;

        private struct ReserveEntityIdsSenderData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.ReserveEntityIds.CommandSender> Senders;
        }

        [Inject] private ReserveEntityIdsSenderData reserveEntityIdsSenderData;

        private struct EntityQuerySenderData
        {
            public readonly int Length;
            public EntityArray Entities;
            [ReadOnly] public ComponentDataArray<WorldCommands.EntityQuery.CommandSender> Senders;
        }

        [Inject] private EntityQuerySenderData entityQuerySenderData;

        private WorldCommands.CreateEntity.Storage createEntityStorage;
        private WorldCommands.DeleteEntity.Storage deleteEntityStorage;
        private WorldCommands.ReserveEntityIds.Storage reserveEntityIdsStorage;
        private WorldCommands.EntityQuery.Storage entityQueryStorage;

        protected override void OnCreateManager(int capacity)
        {
            base.OnCreateManager(capacity);
            worker = Worker.GetWorkerFromWorld(World);

            var requestTracker = World.GetOrCreateManager<CommandRequestTrackerSystem>();
            createEntityStorage = requestTracker.GetCommandStorageForType<WorldCommands.CreateEntity.Storage>();
            deleteEntityStorage = requestTracker.GetCommandStorageForType<WorldCommands.DeleteEntity.Storage>();
            reserveEntityIdsStorage = requestTracker.GetCommandStorageForType<WorldCommands.ReserveEntityIds.Storage>();
            entityQueryStorage = requestTracker.GetCommandStorageForType<WorldCommands.EntityQuery.Storage>();
        }

        protected override void OnUpdate()
        {
            if (worker.Connection == null)
            {
                return;
            }

            for (var i = 0; i < createEntitySenderData.Length; i++)
            {
                var sender = createEntitySenderData.Senders[i];
                var entity = createEntitySenderData.Entities[i];
                foreach (var req in sender.RequestsToSend)
                {
                    var reqId = worker.Connection.SendCreateEntityRequest(req.Entity, req.EntityId, req.TimeoutMillis);
                    createEntityStorage.CommandRequestsInFlight.Add(reqId.Id,
                        new CommandRequestStore<WorldCommands.CreateEntity.Request>(entity, req, null));
                }

                sender.RequestsToSend.Clear();
            }

            for (var i = 0; i < deleteEntitySenderData.Length; i++)
            {
                var sender = deleteEntitySenderData.Senders[i];
                var entity = deleteEntitySenderData.Entities[i];
                foreach (var req in sender.RequestsToSend)
                {
                    var reqId = worker.Connection.SendDeleteEntityRequest(req.EntityId, req.TimeoutMillis);
                    deleteEntityStorage.CommandRequestsInFlight.Add(reqId.Id,
                        new CommandRequestStore<WorldCommands.DeleteEntity.Request>(entity, req, null));
                }

                sender.RequestsToSend.Clear();
            }


            for (var i = 0; i < reserveEntityIdsSenderData.Length; i++)
            {
                var sender = reserveEntityIdsSenderData.Senders[i];
                var entity = reserveEntityIdsSenderData.Entities[i];
                foreach (var req in sender.RequestsToSend)
                {
                    var reqId = worker.Connection.SendReserveEntityIdsRequest(req.NumberOfEntityIds, req.TimeoutMillis);
                    reserveEntityIdsStorage.CommandRequestsInFlight.Add(reqId.Id,
                        new CommandRequestStore<WorldCommands.ReserveEntityIds.Request>(entity, req, null));
                }

                sender.RequestsToSend.Clear();
            }

            for (var i = 0; i < entityQuerySenderData.Length; i++)
            {
                var sender = entityQuerySenderData.Senders[i];
                var entity = entityQuerySenderData.Entities[i];
                foreach (var req in sender.RequestsToSend)
                {
                    var reqId = worker.Connection.SendEntityQueryRequest(req.EntityQuery, req.TimeoutMillis);
                    entityQueryStorage.CommandRequestsInFlight.Add(reqId.Id,
                        new CommandRequestStore<WorldCommands.EntityQuery.Request>(entity, req, null));
                }

                sender.RequestsToSend.Clear();
            }
        }
    }
}
