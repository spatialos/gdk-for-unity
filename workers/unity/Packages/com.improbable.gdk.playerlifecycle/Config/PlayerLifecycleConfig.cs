using System;
using System.Collections.Generic;

namespace Improbable.Gdk.PlayerLifecycle
{
    public delegate Worker.Core.Entity GetPlayerEntityTemplateDelegate(List<string> clientAttributeSet,
        Generated.Improbable.Vector3f position);

    public static class PlayerLifecycleConfig
    {
        public const float PlayerHeartbeatIntervalSeconds = 5f;
        public const int MaxNumFailedPlayerHeartbeats = 2;

        public static GetPlayerEntityTemplateDelegate CreatePlayerEntityTemplate;

        public static readonly Type[] ClientSystems =
        {
            typeof(SendCreatePlayerRequestSystem),
            typeof(HandlePlayerHeartbeatRequestSystem),
        };

        public static readonly Type[] ServerSystems =
        {
            typeof(HandleCreatePlayerRequestSystem),
            typeof(SendPlayerHeartbeatRequestSystem),
            typeof(HandlePlayerHeartbeatResponseSystem),
        };
    }
}

