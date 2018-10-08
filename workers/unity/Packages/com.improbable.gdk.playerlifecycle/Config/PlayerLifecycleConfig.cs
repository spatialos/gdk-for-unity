using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.PlayerLifecycle
{
    public delegate EntityTemplate GetPlayerEntityTemplateDelegate(
        string clientWorkerId,
        Vector3f position);

    public static class PlayerLifecycleConfig
    {
        public const float PlayerHeartbeatIntervalSeconds = 5f;
        public const int MaxNumFailedPlayerHeartbeats = 2;

        public static GetPlayerEntityTemplateDelegate CreatePlayerEntityTemplate;
    }
}
