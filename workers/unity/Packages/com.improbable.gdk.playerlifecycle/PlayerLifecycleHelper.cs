using System;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public static class PlayerLifecycleHelper
    {
        public static EntityBuilder AddPlayerLifecycleComponents(this EntityBuilder entityBuilder,
            string workerId,
            string clientAccess,
            string serverAccess)
        {
            var clientHeartbeat = PlayerHeartbeatClient.Component.CreateSchemaComponentData();
            var serverHeartbeat = PlayerHeartbeatServer.Component.CreateSchemaComponentData();
            var owningComponent = OwningWorker.Component.CreateSchemaComponentData(workerId);

            return entityBuilder
                .AddComponent(clientHeartbeat, clientAccess)
                .AddComponent(serverHeartbeat, serverAccess)
                .AddComponent(owningComponent, serverAccess);
        }

        public static bool IsOwningWorker(SpatialEntityId entityId, World workerWorld)
        {
            var entityManager = workerWorld.GetOrCreateManager<EntityManager>();
            var worker = workerWorld.GetExistingManager<WorkerSystem>();

            if (worker == null)
            {
                throw new InvalidOperationException("Provided World does not have an associated worker");
            }

            if (!worker.TryGetEntity(entityId.EntityId, out var entity))
            {
                throw new InvalidOperationException(
                    $"Entity with SpatialOS Entity ID {entityId.EntityId.Id} is not in this worker's view");
            }

            if (!entityManager.HasComponent<OwningWorker.Component>(entity))
            {
                return false;
            }

            var ownerId = entityManager.GetComponentData<OwningWorker.Component>(entity).WorkerId;

            return worker.Connection.GetWorkerId() == ownerId;
        }

        public static void AddClientSystems(World world)
        {
            world.GetOrCreateManager<SendCreatePlayerRequestSystem>();
            world.GetOrCreateManager<HandlePlayerHeartbeatRequestSystem>();
        }

        public static void AddServerSystems(World world)
        {
            world.GetOrCreateManager<HandleCreatePlayerRequestSystem>();
            world.GetOrCreateManager<PlayerHeartbeatInitializationSystem>();
            world.GetOrCreateManager<SendPlayerHeartbeatRequestSystem>();
            world.GetOrCreateManager<HandlePlayerHeartbeatResponseSystem>();
        }
    }
}
