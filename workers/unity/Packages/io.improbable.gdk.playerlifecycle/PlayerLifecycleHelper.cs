using System;
using Improbable.Gdk.Core;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public static class PlayerLifecycleHelper
    {
        /// <summary>
        ///     Adds the SpatialOS components used by the player lifecycle module to an entity template.
        /// </summary>
        /// <param name="template">The entity template to add player lifecycle components to.</param>
        /// <param name="clientWorkerId">The ID of the client-worker.</param>
        /// <param name="serverAccess">The server-worker write access attribute.</param>
        public static void AddPlayerLifecycleComponents(EntityTemplate template,
            string clientWorkerId,
            string serverAccess)
        {
            template.AddComponent(new PlayerHeartbeatClient.Snapshot(),
                EntityTemplate.GetWorkerAccessAttribute(clientWorkerId));
            template.AddComponent(new PlayerHeartbeatServer.Snapshot(), serverAccess);
            template.AddComponent(new OwningWorker.Snapshot(clientWorkerId), serverAccess);
        }

        public static bool IsOwningWorker(EntityId entityId, World workerWorld)
        {
            var worker = workerWorld.GetExistingSystem<WorkerSystem>();
            var updateSystem = workerWorld.GetExistingSystem<ComponentUpdateSystem>();
            var entitySystem = workerWorld.GetExistingSystem<EntitySystem>();

            if (worker == null)
            {
                throw new InvalidOperationException("Provided World does not have an associated worker");
            }

            if (!entitySystem.GetEntitiesInView().Contains(entityId))
            {
                return false;
            }

            if (!updateSystem.HasComponent(OwningWorker.ComponentId, entityId))
            {
                return false;
            }

            var ownerId = updateSystem.GetComponent<OwningWorker.Snapshot>(entityId).WorkerId;
            return worker.WorkerId == ownerId;
        }

        /// <summary>
        ///     Adds all the systems a client-worker requires for the player lifecycle module.
        /// </summary>
        /// <param name="world">A world that belongs to a client-worker.</param>
        /// <param name="autoRequestPlayerCreation">An option to toggle automatic player creation.</param>
        public static void AddClientSystems(World world, bool autoRequestPlayerCreation = true)
        {
            PlayerLifecycleConfig.AutoRequestPlayerCreation = autoRequestPlayerCreation;
            world.GetOrCreateSystem<SendCreatePlayerRequestSystem>();
            world.GetOrCreateSystem<HandlePlayerHeartbeatRequestSystem>();
        }

        /// <summary>
        ///     Adds all the systems a server-worker requires for the player lifecycle module.
        /// </summary>
        /// <param name="world">A world that belongs to a server-worker.</param>
        public static void AddServerSystems(World world)
        {
            world.GetOrCreateSystem<HandleCreatePlayerRequestSystem>();
            world.GetOrCreateSystem<PlayerHeartbeatInitializationSystem>();
            world.GetOrCreateSystem<SendPlayerHeartbeatRequestSystem>();
            world.GetOrCreateSystem<HandlePlayerHeartbeatResponseSystem>();
        }
    }
}
