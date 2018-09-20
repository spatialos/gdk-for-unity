using Improbable.Gdk.Core;
using Improbable.PlayerLifecycle;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public static class PlayerLifecycleHelper
    {
        public static EntityBuilder AddPlayerLifecycleComponents(this EntityBuilder entityBuilder,
            string clientAccess,
            string serverAccess)
        {
            var clientHeartbeat = PlayerHeartbeatClient.Component.CreateSchemaComponentData();
            var serverHeartbeat = PlayerHeartbeatServer.Component.CreateSchemaComponentData();
            return entityBuilder
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
