using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Alpha;
using Locator = Improbable.Worker.CInterop.Locator;
using LocatorParameters = Improbable.Worker.CInterop.LocatorParameters;
using AlphaLocator = Improbable.Worker.CInterop.Alpha.Locator;

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
    ///     Represents the Locator connection flow.
    /// </summary>
    public class LocatorFlow : IConnectionFlow
    {
        /// <summary>
        ///     The IP address of the Locator to use when connecting.
        /// </summary>
        public string LocatorHost = RuntimeConfigNames.LocatorHost;

        /// <summary>
        ///     The parameters to use to connect to the Locator.
        /// </summary>
        public LocatorParameters LocatorParameters = new LocatorParameters
        {
            LoginToken = new LoginTokenCredentials
            {
                Token = string.Empty
            },
            Steam = new SteamCredentials
            {
                Ticket = string.Empty,
                DeploymentTag = string.Empty
            },
            Logging = new ProtocolLoggingParameters(),
            ProjectName = string.Empty,
            CredentialsType = LocatorCredentialsType.LoginToken,
            EnableLogging = false,
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="LocatorFlow"/> class.
        /// </summary>
        /// <param name="initializer">Optional. An initializer to seed the data required to connect via the Locator flow.</param>
        public LocatorFlow(IConnectionFlowInitializer<LocatorFlow> initializer = null)
        {
            initializer?.Initialize(this);
        }

        public async Task<Connection> CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)
        {
            parameters.Network.UseExternalIp = true;

            using (var locator = new Locator(LocatorHost, LocatorParameters))
            {
                var deploymentList = await GetDeploymentList(locator);

                var deploymentName = SelectDeploymentName(deploymentList);
                if (string.IsNullOrEmpty(deploymentName))
                {
                    throw new ConnectionFailedException("No deployment name chosen",
                        ConnectionErrorReason.DeploymentNotFound);
                }

                using (var connectionFuture = locator.ConnectAsync(deploymentName, parameters, _ => true))
                {
                    return await Utils.TryToConnectAsync(connectionFuture, token).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        ///     Selects a deployment to connect to.
        /// </summary>
        /// <param name="deploymentList">The list of deployments to choose from.</param>
        /// <returns>The name of the deployment to connect to.</returns>
        /// <exception cref="ConnectionFailedException">The deployment list contains an error or is empty.</exception>
        protected virtual string SelectDeploymentName(DeploymentList deploymentList)
        {
            if (deploymentList.Error != null)
            {
                throw new ConnectionFailedException($"Failed to list deployments with error: ${deploymentList.Error}", ConnectionErrorReason.DeploymentNotFound);
            }

            if (deploymentList.Deployments.Count == 0)
            {
                throw new ConnectionFailedException("Could not find any deployments to connect to.", ConnectionErrorReason.DeploymentNotFound);
            }

            return deploymentList.Deployments[0].DeploymentName;
        }

        private static async Task<DeploymentList> GetDeploymentList(Locator locator)
        {
            using (var deploymentsFuture = locator.GetDeploymentListAsync())
            {
                var deploymentList = await Task.Run(() => deploymentsFuture.Get()).ConfigureAwait(false);
                // Guard against null refs. This shouldn't be triggered.
                if (!deploymentList.HasValue)
                {
                    throw new ConnectionFailedException("Deployment list future returned null.",
                        ConnectionErrorReason.DeploymentNotFound);
                }

                if (deploymentList.Value.Error != null)
                {
                    throw new ConnectionFailedException(deploymentList.Value.Error,
                        ConnectionErrorReason.DeploymentNotFound);
                }

                return deploymentList.Value;
            }
        }
    }

    /// <summary>
    ///     Represents the Alpha Locator connection flow.
    /// </summary>
    public class AlphaLocatorFlow : IConnectionFlow
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
        ///     The parameters to use to connect to the Locator.
        /// </summary>
        public Improbable.Worker.CInterop.Alpha.LocatorParameters LocatorParameters =
            new Improbable.Worker.CInterop.Alpha.LocatorParameters
            {
                PlayerIdentity = new PlayerIdentityCredentials
                {
                    LoginToken = string.Empty,
                    PlayerIdentityToken = string.Empty
                }
            };

        /// <summary>
        ///     Initializes a new instance of the <see cref="AlphaLocatorFlow"/> class.
        /// </summary>
        /// <param name="initializer">Optional. An initializer to seed the data required to connect via the Alpha Locator flow.</param>
        public AlphaLocatorFlow(IConnectionFlowInitializer<AlphaLocatorFlow> initializer = null)
        {
            initializer?.Initialize(this);
        }

        public async Task<Connection> CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)
        {
            parameters.Network.UseExternalIp = true;

            if (UseDevAuthFlow)
            {
                var pit = GetDevelopmentPlayerIdentityToken();
                var loginTokenDetails = GetDevelopmentLoginTokens(parameters.WorkerType, pit);

                LocatorParameters.PlayerIdentity.PlayerIdentityToken = pit;
                LocatorParameters.PlayerIdentity.LoginToken = SelectLoginToken(loginTokenDetails);
            }

            using (var locator = new AlphaLocator(LocatorHost, LocatorPort, LocatorParameters))
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
                    UseInsecureConnection = LocatorParameters.UseInsecureConnection,
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
                    UseInsecureConnection = LocatorParameters.UseInsecureConnection,
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
