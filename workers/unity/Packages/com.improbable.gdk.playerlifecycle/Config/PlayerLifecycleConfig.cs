using System.Collections.Generic;

namespace Improbable.Gdk.PlayerLifecycle
{
    public delegate Worker.Core.Entity GetPlayerEntityTemplateDelegate(List<string> clientAttributeSet,
        Vector3f position);

    public static class PlayerLifecycleConfig
    {
        public const float PlayerHeartbeatIntervalSeconds = 5f;
        public const int MaxNumFailedPlayerHeartbeats = 2;

        public static GetPlayerEntityTemplateDelegate CreatePlayerEntityTemplate;
    }
}
