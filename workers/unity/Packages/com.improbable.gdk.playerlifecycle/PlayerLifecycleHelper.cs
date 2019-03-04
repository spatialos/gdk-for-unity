using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public class PlayerLifecycleHelper
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

        public static byte[] SerializeParams<T>(T playerCreationArguments)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, playerCreationArguments);
                    return memoryStream.ToArray();
                }
            }
            catch
            {
                UnityEngine.Debug.LogWarning("Unable to serialize player creation arguments.");
                return null;
            }
        }

        public static T DeserializeParams<T>(byte[] serializedArguments)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    var binaryFormatter = new BinaryFormatter();
                    memoryStream.Write(serializedArguments, 0, serializedArguments.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return (T) binaryFormatter.Deserialize(memoryStream);
                }
            }
            catch
            {
                UnityEngine.Debug.LogWarning("Unable to deserialize player creation arguments.");
                return default;
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
