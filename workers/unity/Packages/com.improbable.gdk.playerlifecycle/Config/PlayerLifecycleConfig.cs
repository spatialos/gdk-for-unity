using Improbable.Gdk.Core;

namespace Improbable.Gdk.PlayerLifecycle
{
    public delegate EntityTemplate GetPlayerEntityTemplateDelegate(
        string clientWorkerId,
        byte[] serializedArguments);

    public static class PlayerLifecycleConfig
    {
        /// <summary>
        ///     The time in seconds between player heartbeat requests.
        /// </summary>
        public static float PlayerHeartbeatIntervalSeconds = 5f;

        /// <summary>
        ///     The maximum number of failed heartbeats before a player is disconnected.
        /// </summary>
        public static int MaxNumFailedPlayerHeartbeats = 2;

        /// <summary>
        ///     The maximum number of retries for player creation requests.
        /// </summary>
        public static int MaxPlayerCreationRetries = 5;

        /// <summary>
        ///     The maximum number of retries for finding player creator entities, before any player creation occurs.
        /// </summary>
        public static int MaxPlayerCreatorQueryRetries = 5;

        /// <summary>
        ///     This indicates whether a player should be created automatically upon a worker connecting to SpatialOS.
        /// </summary>
        public static bool AutoRequestPlayerCreation = true;

        /// <summary>
        ///     The delegate responsible for returning a player entity template when creating a player.
        /// </summary>
        public static GetPlayerEntityTemplateDelegate CreatePlayerEntityTemplate;
    }
}
