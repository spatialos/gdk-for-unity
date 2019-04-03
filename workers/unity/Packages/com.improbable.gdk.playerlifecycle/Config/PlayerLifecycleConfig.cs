using Improbable.Gdk.Core;

namespace Improbable.Gdk.PlayerLifecycle
{
    /// <summary>
    ///     Signature of the function called to get a player <see cref="EntityTemplate"/> instance.
    /// </summary>
    /// <param name="clientWorkerId">
    ///     The worker ID of a client-worker that sent a player creation request.
    /// </param>
    /// <param name="serializedArguments">
    ///     A serialized byte array of arbitrary data. By default this is null unless you provided
    ///     the serialized data when calling `RequestPlayerCreation` manually.
    /// </param>
    /// <remarks>
    ///     The `CreatePlayerEntityTemplate` method in the <see cref="PlayerLifecycleConfig"/>, called by the
    ///     <see cref="HandleCreatePlayerRequestSystem"/>, must match this signature.
    /// </remarks>
    /// <returns>
    ///     An <see cref="EntityTemplate"/> to create a SpatialOS player entity from.
    /// </returns>
    public delegate EntityTemplate GetPlayerEntityTemplateDelegate(
        string clientWorkerId,
        byte[] serializedArguments);

    public static class PlayerLifecycleConfig
    {
        /// <summary>
        ///     The time in seconds between player heartbeat requests.
        /// </summary>
        /// <remarks>
        ///     This is used by the <see cref="SendPlayerHeartbeatRequestSystem"/> on the server-worker to determine
        ///     how often to send a `PlayerHeartbeat` request to client-workers.
        /// </remarks>
        public static float PlayerHeartbeatIntervalSeconds = 5f;

        /// <summary>
        ///     The maximum number of failed heartbeats before a player is disconnected.
        /// </summary>
        /// <remarks>
        ///     The <see cref="HandlePlayerHeartbeatResponseSystem"/> deletes a player entity if the corresponding client-worker
        ///     fails to respond successfully to this number of consecutive `PlayerHeartbeat` requests.
        /// </remarks>
        public static int MaxNumFailedPlayerHeartbeats = 2;

        /// <summary>
        ///     The maximum number of retries for player creation requests.
        /// </summary>
        /// <remarks>
        ///     The number of times a player creation request is retried after the first attempt. Setting this
        ///     to 0 disables retrying player creation after calling `RequestPlayerCreation`.
        /// </remarks>
        public static int MaxPlayerCreationRetries = 5;

        /// <summary>
        ///     The maximum number of retries for finding player creator entities, before any player creation occurs.
        /// </summary>
        /// <remarks>
        ///     All player creation requests must be sent to a player creator entity, which are initially queried for when
        ///     the <see cref="SendCreatePlayerRequestSystem"/> starts. This field indicates the maximum number of retries for the
        //      initial entity query to find the player creator entities.
        /// </remarks>
        public static int MaxPlayerCreatorQueryRetries = 5;

        /// <summary>
        ///     This indicates whether a player should be created automatically upon a worker connecting to SpatialOS.
        /// </summary>
        /// <remarks>
        ///     If `true`, a Player entity is automatically created upon a client-worker connecting to SpatialOS. However,
        ///     to be able to send arbitrary serialized data in the player creation request, or to provide a callback to be
        ///     invoked upon receiving a player creation response, this field must be set to `false`.
        /// </remarks>
        public static bool AutoRequestPlayerCreation = true;

        /// <summary>
        ///     The delegate responsible for returning a player <see cref="EntityTemplate"/> when creating a player.
        /// </summary>
        /// <remarks>
        ///     This must be set before initiating any player creation because it is called by the <see cref="HandleCreatePlayerRequestSystem"/>.
        ///     The system uses this delegate to request a new player entity based on the returned <see cref="EntityTemplate"/>.
        /// </remarks>
        public static GetPlayerEntityTemplateDelegate CreatePlayerEntityTemplate;
    }
}
