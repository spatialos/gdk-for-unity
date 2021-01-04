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
        public static void AddPlayerLifecycleComponents(EntityTemplate template, string clientWorkerId)
        {
            // TODO: These need to be put in the right component sets for this to work.
            // Should we implement this in schema that we ship?
            template.AddComponent(new PlayerHeartbeatClient.Snapshot());
            template.AddComponent(new PlayerHeartbeatServer.Snapshot());
            template.AddComponent(new OwningWorker.Snapshot(clientWorkerId));
        }

        public static bool IsOwningWorker(EntityId entityId, World workerWorld)
        {
            var worker = workerWorld.GetExistingSystem<WorkerSystem>();
            var entityManager = workerWorld.EntityManager;

            if (worker == null)
            {
                throw new InvalidOperationException("Provided World does not have an associated worker");
            }

            if (!worker.TryGetEntity(entityId, out var entity))
            {
                return false;
            }

            if (!entityManager.HasComponent<OwningWorker.Component>(entity))
            {
                return false;
            }

            var ownerId = entityManager.GetComponentData<OwningWorker.Component>(entity).WorkerId;
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
            world.GetOrCreateSystem<EntityReservationSystem>();
            world.GetOrCreateSystem<HandleCreatePlayerRequestSystem>();
            world.GetOrCreateSystem<PlayerHeartbeatInitializationSystem>();
            world.GetOrCreateSystem<SendPlayerHeartbeatRequestSystem>();
            world.GetOrCreateSystem<HandlePlayerHeartbeatResponseSystem>();
        }
    }
}
