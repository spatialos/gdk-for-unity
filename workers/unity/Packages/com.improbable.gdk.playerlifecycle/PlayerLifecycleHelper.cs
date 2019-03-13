using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public static class PlayerLifecycleHelper
    {
        public static void AddPlayerLifecycleComponents(EntityTemplate template,
            string workerId,
            string clientAccess,
            string serverAccess)
        {
            var clientHeartbeat = new PlayerHeartbeatClient.Snapshot();
            var serverHeartbeat = new PlayerHeartbeatServer.Snapshot();
            var owningComponent = new OwningWorker.Snapshot { WorkerId = workerId };

            template.AddComponent(clientHeartbeat, clientAccess);
            template.AddComponent(serverHeartbeat, serverAccess);
            template.AddComponent(owningComponent, serverAccess);
        }

        public static bool IsOwningWorker(EntityId entityId, World workerWorld)
        {
            var worker = workerWorld.GetExistingManager<WorkerSystem>();
            var updateSystem = workerWorld.GetExistingManager<ComponentUpdateSystem>();
            var entitySystem = workerWorld.GetExistingManager<EntitySystem>();

            if (worker == null)
            {
                throw new InvalidOperationException("Provided World does not have an associated worker");
            }

            if (entitySystem.GetEntitiesInView().Contains(entityId))
            {
                throw new InvalidOperationException(
                    $"Entity with SpatialOS Entity ID {entityId.Id} is not in this worker's view");
            }

            if (!updateSystem.HasComponent(OwningWorker.ComponentId, entityId))
            {
                return false;
            }


            var ownerId = updateSystem.GetComponent<OwningWorker.Snapshot>(entityId).WorkerId;
            return worker.WorkerId == ownerId;
        }

        public static void AddClientSystems(World world, bool autoRequestPlayerCreation = true)
        {
            PlayerLifecycleConfig.AutoRequestPlayerCreation = autoRequestPlayerCreation;
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
