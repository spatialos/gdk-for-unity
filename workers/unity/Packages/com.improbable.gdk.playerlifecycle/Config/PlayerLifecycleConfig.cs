using System.Collections.Generic;
using Unity.Entities;

namespace Improbable.Gdk.PlayerLifecycle
{
    public delegate Worker.Entity GetPlayerEntityTemplateDelegate(List<string> clientAttributeSet,
        Generated.Improbable.Vector3f position);

    public static class PlayerLifecycleConfig
    {
        public const float PlayerHeartbeatIntervalSeconds = 5f;
        public const int MaxNumFailedPlayerHeartbeats = 2;

        public static GetPlayerEntityTemplateDelegate CreatePlayerEntityTemplate;

        public static void RegisterClientSystems(World world)
        {
            world.GetOrCreateManager<SendCreatePlayerRequestSystem>();
            world.GetOrCreateManager<HandlePlayerHeartbeatRequestSystem>();
        }

        public static void RegisterServerSystems(World world)
        {
            world.GetOrCreateManager<HandleCreatePlayerRequestSystem>();
            world.GetOrCreateManager<SendPlayerHeartbeatRequestSystem>();
            world.GetOrCreateManager<HandlePlayerHeartbeatResponseSystem>();
        }
    }
}
