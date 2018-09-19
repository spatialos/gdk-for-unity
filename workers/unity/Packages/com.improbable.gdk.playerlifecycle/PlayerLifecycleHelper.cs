using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public class PlayerLifecycleHelper
    {
        public static void AddComponents(ref EntityBuilder entityBuilder, string clientAccess, string serverAccess)
        {
            var clientHeartbeat = PlayerHeartbeatClient.Component.CreateSchemaComponentData();
            var serverHeartbeat = PlayerHeartbeatServer.Component.CreateSchemaComponentData();
            entityBuilder = entityBuilder
                .AddComponent(clientHeartbeat, clientAccess)
                .AddComponent(serverHeartbeat, serverAccess);
        }

        public static void AddClientSystems(World world)
        {
            world.GetOrCreateManager<SendCreatePlayerRequestSystem>();
            world.GetOrCreateManager<HandlePlayerHeartbeatRequestSystem>();
        }

        public static void AddServerSystems(World world)
        {
            world.GetOrCreateManager<HandleCreatePlayerRequestSystem>();
            world.GetOrCreateManager<SendPlayerHeartbeatRequestSystem>();
            world.GetOrCreateManager<HandlePlayerHeartbeatResponseSystem>();
        }
    }
}
