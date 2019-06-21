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
    public interface IConnectionFlow
    {
        Task<Connection> CreateAsync(ConnectionParameters parameters, CancellationToken? token = null);
    }

    public class ReceptionistFlow : IConnectionFlow
    {
        public string ReceptionistHost = RuntimeConfigDefaults.ReceptionistHost;
        public ushort ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort;
        public string WorkerId;

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

    public class LocatorFlow : IConnectionFlow
    {
        public string LocatorHost = RuntimeConfigNames.LocatorHost;

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

        public LocatorFlow(IConnectionFlowInitializer<LocatorFlow> initializer)
        {
            initializer.Initialize(this);
        }

        public async Task<Connection> CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)
        {
            // TODO: Should we force external IP ?
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

    public class AlphaLocatorFlow : IConnectionFlow
    {
        public string LocatorHost = RuntimeConfigDefaults.LocatorHost;
        public ushort AnonymousAuthPort = RuntimeConfigDefaults.AnonymousAuthenticationPort;
        public string DevAuthToken = string.Empty;

        public bool UseDevAuthFlow = true;

        public Improbable.Worker.CInterop.Alpha.LocatorParameters LocatorParameters =
            new Improbable.Worker.CInterop.Alpha.LocatorParameters
            {
                PlayerIdentity = new PlayerIdentityCredentials
                {
                    LoginToken = string.Empty,
                    PlayerIdentityToken = string.Empty
                }
            };

        public AlphaLocatorFlow(IConnectionFlowInitializer<AlphaLocatorFlow> initializer)
        {
            initializer.Initialize(this);
        }

        public async Task<Connection> CreateAsync(ConnectionParameters parameters, CancellationToken? token = null)
        {
            // TODO: Should we force external IP ?
            if (UseDevAuthFlow)
            {
                var pit = GetDevelopmentPlayerIdentityToken();
                var loginTokenDetails = GetDevelopmentLoginTokens(parameters.WorkerType, pit);

                LocatorParameters.PlayerIdentity.PlayerIdentityToken = pit;
                LocatorParameters.PlayerIdentity.LoginToken = SelectLoginToken(loginTokenDetails);
            }

            using (var locator = new AlphaLocator(LocatorHost, LocatorParameters))
            {
                using (var connectionFuture = locator.ConnectAsync(parameters))
                {
                    return await Utils.TryToConnectAsync(connectionFuture, token);
                }
            }
        }

        protected virtual string GetDevelopmentPlayerIdentityToken()
        {
            var result = DevelopmentAuthentication.CreateDevelopmentPlayerIdentityTokenAsync(
                LocatorHost,
                AnonymousAuthPort,
                new PlayerIdentityTokenRequest
                {
                    DevelopmentAuthenticationToken = DevAuthToken,
                    PlayerId = GetPlayerId(),
                    DisplayName = GetDisplayName(),
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
        ///     can connect to via the anonymous authentication flow.
        /// </summary>
        /// <param name="workerType">The type of the worker that wants to connect.</param>
        /// <param name="playerIdentityToken">The player identity token of the player that wants to connect.</param>
        /// <returns>A list of all available login tokens and their deployments.</returns>
        protected virtual List<LoginTokenDetails> GetDevelopmentLoginTokens(string workerType, string playerIdentityToken)
        {
            var result = DevelopmentAuthentication.CreateDevelopmentLoginTokensAsync(
                RuntimeConfigDefaults.LocatorHost,
                RuntimeConfigDefaults.AnonymousAuthenticationPort,
                new LoginTokensRequest
                {
                    WorkerType = workerType,
                    PlayerIdentityToken = playerIdentityToken,
                    UseInsecureConnection = false,
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
        ///     Selects which login token to use to connect via the anonymous authentication flow.
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
        /// Retrieves the player id for the player trying to connect via the anonymous authentication flow.
        /// </summary>
        /// <returns>A string containing the player id.</returns>
        protected virtual string GetPlayerId()
        {
            return $"Player-{Guid.NewGuid()}";
        }

        /// <summary>
        /// Retrieves the display name for the player trying to connect via the anonymous authentication flow.
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
