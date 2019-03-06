using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Entities;
using UnityEngine;
using Object = System.Object;

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

        public static byte[] SerializeArguments(object playerCreationArguments)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, playerCreationArguments);
                return memoryStream.ToArray();
            }
        }

        public static T DeserializeArguments<T>(byte[] serializedArguments)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                memoryStream.Write(serializedArguments, 0, serializedArguments.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T) binaryFormatter.Deserialize(memoryStream);
            }
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

        public static void AddClientSystems(World world, bool autoRequestPlayerCreation = true,
            Vector3 spawnPosition = default,
            byte[] serializedArguments = null)
        {
            PlayerLifecycleConfig.AutoRequestPlayerCreation = autoRequestPlayerCreation;

            var createPlayerRequestSystem = world.GetOrCreateManager<SendCreatePlayerRequestSystem>();
            createPlayerRequestSystem.SetPlayerCreationArguments(serializedArguments);

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
