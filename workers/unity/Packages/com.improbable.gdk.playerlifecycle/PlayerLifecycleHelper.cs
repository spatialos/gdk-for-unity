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

        /// <summary>
        ///     Returns whether an entity is owned by a worker. It can be used to determine whether a client-worker is
        ///     responsible for a particular player entity.
        /// </summary>
        /// <param name="entityId">An ECS component containing a SpatialOS Entity ID.</param>
        /// <param name="workerWorld">An ECS World associated with a worker.</param>
        /// <returns>
        ///     True if the entity with ID entityId contains an OwningWorker component with a value matching the
        ///     workerWorld's workerId. False if the entity does not contain an OwningWorker component, or if the value
        ///     does not match the workerId. Throws an InvalidOperationException if workerWorld does not contain a
        ///     WorkerSystem, or if the entity does not exist in the worker's view.
        /// </returns>
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

        /// <summary>
        ///     Adds all the systems a client-worker requires for the player lifecycle module.
        /// </summary>
        /// <param name="world">A world that belongs to a client-worker.</param>
        /// <param name="autoRequestPlayerCreation">An option to toggle automatic player creation.</param>
        public static void AddClientSystems(World world, bool autoRequestPlayerCreation = true)
        {
            PlayerLifecycleConfig.AutoRequestPlayerCreation = autoRequestPlayerCreation;
            world.GetOrCreateManager<SendCreatePlayerRequestSystem>();
            world.GetOrCreateManager<HandlePlayerHeartbeatRequestSystem>();
        }

        /// <summary>
        ///     Adds all the systems a server-worker requires for the player lifecycle module.
        /// </summary>
        /// <param name="world">A world that belongs to a server-worker.</param>
        public static void AddServerSystems(World world)
        {
            world.GetOrCreateManager<HandleCreatePlayerRequestSystem>();
            world.GetOrCreateManager<PlayerHeartbeatInitializationSystem>();
            world.GetOrCreateManager<SendPlayerHeartbeatRequestSystem>();
            world.GetOrCreateManager<HandlePlayerHeartbeatResponseSystem>();
        }
    }
}
