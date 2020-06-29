using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Alpha;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents an implementation of a flow to connect to SpatialOS.
    /// </summary>
    public interface IConnectionFlow
    {
        /// <summary>
        ///     Creates a <see cref="Connection"/> asynchronously.
        /// </summary>
        /// <param name="parameters">The connection parameters to use for the connection.</param>
        /// <param name="token">A cancellation token which should cancel the underlying connection attempt.</param>
        /// <returns>A task that represents the asynchronous creation of the <see cref="Connection"/> object.</returns>
        Task<Connection> CreateAsync(ConnectionParameters parameters, CancellationToken? token = null);
    }

    /// <summary>
    ///     Represents the Receptionist connection flow.
    /// </summary>
    public class ReceptionistFlow : IConnectionFlow
    {
        /// <summary>
        ///     The IP address of the Receptionist to use when connecting.
        /// </summary>
        public string ReceptionistHost = RuntimeConfigDefaults.ReceptionistHost;

        /// <summary>
        ///     The port of the Receptionist to use when connecting.
        /// </summary>
        public ushort ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort;

        /// <summary>
        ///     The worker ID to use for the worker connection that will be created when <see cref="CreateAsync"/>
        ///     is called.
        /// </summary>
        public string WorkerId;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReceptionistFlow"/> class.
        /// </summary>
        /// <param name="workerId">The worker ID to use for the worker connection.</param>
        /// <param name="initializer">Optional. An initializer to seed the data required to connect via the Receptionist flow.</param>
        public ReceptionistFlow(string workerId, IConnectionFlowInitializer<ReceptionistFlow> initializer = null)
        {
            WorkerId = workerId;
            initializer?.Initialize(this);
        }

        public async Task<Connection> CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)
        {
            using (var connectionFuture =
                Connection.ConnectAsync(ReceptionistHost, ReceptionistPort, WorkerId, parameters))
            {
                return await Utils.TryToConnectAsync(connectionFuture, token).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    ///     Represents the Alpha Locator connection flow.
    /// </summary>
    public class LocatorFlow : IConnectionFlow
    {
        /// <summary>
        ///     The host of the Locator to use for the development authentication flow and the Locator.
        /// </summary>
        public string LocatorHost = RuntimeConfigDefaults.LocatorHost;

        /// <summary>
        ///     The port of the Locator to use for the development authentication flow and the Locator.
        /// </summary>
        public ushort LocatorPort = RuntimeConfigDefaults.LocatorPort;

        /// <summary>
        ///     The development authentication token to use when connecting via with development authentication.
        /// </summary>
        public string DevAuthToken = string.Empty;

        /// <summary>
        ///    Denotes whether we should connect with development authentication.
        /// </summary>
        /// <remarks>
        ///     If this is false, it is assumed that the <see cref="PlayerIdentityCredentials"/> element
        ///     in the <see cref="LocatorParameters"/> has been filled.
        /// </remarks>
        public bool UseDevAuthFlow = true;

        /// <summary>
        ///     The login token to use to connect via the Locator.
        /// </summary>
        public string LoginToken = string.Empty;

        /// <summary>
        ///     The player identity token to use to connect via the Locator.
        /// </summary>
        public string PlayerIdentityToken = string.Empty;

        /// <summary>
        ///     Denotes whether to connect to the Locator via an insecure connection or not.
        /// </summary>
        public bool UseInsecureConnection = false;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LocatorFlow"/> class.
        /// </summary>
        /// <param name="initializer">Optional. An initializer to seed the data required to connect via the Alpha Locator flow.</param>
        public LocatorFlow(IConnectionFlowInitializer<LocatorFlow> initializer = null)
        {
            initializer?.Initialize(this);
        }

        public async Task<Connection> CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)
        {
            parameters.Network.UseExternalIp = true;

            if (UseDevAuthFlow)
            {
                PlayerIdentityToken = GetDevelopmentPlayerIdentityToken();
                var loginTokenDetails = GetDevelopmentLoginTokens(parameters.WorkerType, PlayerIdentityToken);
                LoginToken = SelectLoginToken(loginTokenDetails);
            }

            var locatorParameters = new LocatorParameters
            {
                PlayerIdentity = new PlayerIdentityCredentials
                {
                    PlayerIdentityToken = PlayerIdentityToken,
                    LoginToken = LoginToken
                },
                CredentialsType = LocatorCredentialsType.PlayerIdentity,
                UseInsecureConnection = UseInsecureConnection
            };

            using (var locator = new Locator(LocatorHost, LocatorPort, locatorParameters))
            {
                using (var connectionFuture = locator.ConnectAsync(parameters))
                {
                    return await Utils.TryToConnectAsync(connectionFuture, token).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        ///     Retrieves a development player identity token using development authentication.
        /// </summary>
        /// <returns>The player identity token string.</returns>
        /// <exception cref="AuthenticationFailedException">Failed to get a development player identity token.</exception>
        protected virtual string GetDevelopmentPlayerIdentityToken()
        {
            var result = DevelopmentAuthentication.CreateDevelopmentPlayerIdentityTokenAsync(
                LocatorHost,
                LocatorPort,
                new PlayerIdentityTokenRequest
                {
                    DevelopmentAuthenticationToken = DevAuthToken,
                    PlayerId = GetPlayerId(),
                    DisplayName = GetDisplayName(),
                    UseInsecureConnection = UseInsecureConnection,
                }
            ).Get();

            if (!result.HasValue)
            {
                throw new AuthenticationFailedException("Did not receive a player identity token.");
            }

            if (result.Value.Status.Code != ConnectionStatusCode.Success)
            {
                throw new AuthenticationFailedException("Failed to retrieve a player identity token.\n" +
                    $"error code: {result.Value.Status.Code}\nerror message: {result.Value.Status.Detail}");
            }

            return result.Value.PlayerIdentityToken;
        }

        /// <summary>
        ///     Retrieves the login tokens for all active deployments that the player
        ///     can connect to via the development authentication flow.
        /// </summary>
        /// <param name="workerType">The type of the worker that wants to connect.</param>
        /// <param name="playerIdentityToken">The player identity token of the player that wants to connect.</param>
        /// <returns>A list of all available login tokens and their deployments.</returns>
        protected virtual List<LoginTokenDetails> GetDevelopmentLoginTokens(string workerType, string playerIdentityToken)
        {
            var result = DevelopmentAuthentication.CreateDevelopmentLoginTokensAsync(
                LocatorHost,
                LocatorPort,
                new LoginTokensRequest
                {
                    WorkerType = workerType,
                    PlayerIdentityToken = playerIdentityToken,
                    UseInsecureConnection = UseInsecureConnection,
                    DurationSeconds = 120,
                }
            ).Get();

            if (!result.HasValue)
            {
                throw new AuthenticationFailedException("Did not receive any login tokens back.");
            }

            if (result.Value.Status.Code != ConnectionStatusCode.Success)
            {
                throw new AuthenticationFailedException("Failed to retrieve any login tokens.\n" +
                    $"error code: {result.Value.Status.Code}\nerror message: {result.Value.Status.Detail}");
            }

            return result.Value.LoginTokens;
        }

        /// <summary>
        ///     Selects which login token to use to connect via the development authentication flow.
        /// </summary>
        /// <param name="loginTokens">A list of available login tokens.</param>
        /// <returns>The selected login token.</returns>
        protected virtual string SelectLoginToken(List<LoginTokenDetails> loginTokens)
        {
            if (loginTokens.Count == 0)
            {
                throw new AuthenticationFailedException("Did not receive any login tokens. Do you have a valid deployment running?");
            }

            return loginTokens[0].LoginToken;
        }

        /// <summary>
        ///     Gets the player ID for the player trying to connect via the development authentication flow.
        /// </summary>
        /// <returns>A string containing the player id.</returns>
        protected virtual string GetPlayerId()
        {
            return $"Player-{Guid.NewGuid()}";
        }

        /// <summary>
        ///     Retrieves the display name for the player trying to connect via the development authentication flow.
        /// </summary>
        /// <returns>A string containing the display name.</returns>
        protected virtual string GetDisplayName()
        {
            return string.Empty;
        }
    }

    internal static class Utils
    {
        internal static async Task<Connection> TryToConnectAsync(Future<Connection> connectionFuture, CancellationToken? token = null)
        {
            Connection connection;
            if (token == null)
            {
                connection = await Task.Run(connectionFuture.Get).ConfigureAwait(false);
            }
            else
            {
                connection = await Task.Run(connectionFuture.Get)
                    .WithCancellation(token.Value)
                    .ConfigureAwait(false);
            }

            if (connection.GetConnectionStatusCode() != ConnectionStatusCode.Success)
            {
                throw new ConnectionFailedException(connection.GetConnectionStatusCodeDetailString(), ConnectionErrorReason.CannotEstablishConnection);
            }

            return connection;
        }
    }
}
