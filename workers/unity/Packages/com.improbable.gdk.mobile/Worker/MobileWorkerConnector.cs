using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Alpha;
using UnityEngine;

namespace Improbable.Gdk.Mobile
{
    public abstract class MobileWorkerConnector : WorkerConnector
    {
        [SerializeField] private string DevelopmentAuthToken;
        
        protected abstract string GetHostIp();

        protected override ConnectionParameters GetConnectionParameters(string workerType, ConnectionService service)
        {
            return new ConnectionParameters
            {
                WorkerType = workerType,
                Network =
                {
                    ConnectionType = NetworkConnectionType.Tcp,
                    UseExternalIp = true,
                },
                EnableProtocolLoggingAtStartup = false,
                DefaultComponentVtable = new ComponentVtable(),
            };
        }

        protected override ReceptionistConfig GetReceptionistConfig(string workerType)
        {
            return new ReceptionistConfig
            {
                ReceptionistHost = GetHostIp(),
                ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort,
                WorkerId = CreateNewWorkerId(workerType)
            };
        }

        protected override LocatorConfig GetLocatorConfig()
        {
            throw new NotImplementedException("The locator flow is currently not available for mobile workers.");
        }

        protected override AlphaLocatorConfig GetAlphaLocatorConfig(string workerType)
        {
            var pit = DevelopmentAuthentication.CreateDevelopmentPlayerIdentityTokenAsync(
                RuntimeConfigDefaults.LocatorHost,
                444,
                new PlayerIdentityTokenRequest
                {
                    DevelopmentAuthenticationTokenId = DevelopmentAuthToken,
                    PlayerId = $"Player-{Guid.NewGuid()}",
                    DisplayName = "",
                }
            ).Get().Value;

            if (!string.IsNullOrEmpty(pit.Error))
            {
                throw new ConnectionFailedException(pit.Error, ConnectionErrorReason.InvalidConfig);
            }
            
            var loginTokenRequestResult = DevelopmentAuthentication.CreateDevelopmentLoginTokensAsync(
                RuntimeConfigDefaults.LocatorHost,
                444,
                new LoginTokensRequest
                {
                    WorkerType = workerType,
                    PlayerIdentityToken = pit.PlayerIdentityToken,
                    UseInsecureConnection = false,
                    DurationSeconds = 120,
                }
            ).Get();

            if (!loginTokenRequestResult.HasValue)
            {
                throw new ConnectionFailedException("Did not receive any login tokens back.", 
                    ConnectionErrorReason.InvalidConfig);
            }

            if (!loginTokenRequestResult.Value.Status.Equals(ConnectionStatusCode.Success))
            {
                throw new ConnectionFailedException($"Failed to retrieve login token, " +
                    $"error code: {loginTokenRequestResult.Value.Status}", ConnectionErrorReason.InvalidConfig);
            }
            
            // just select first deployment for now...
            var loginTokenDetails = loginTokenRequestResult.Value.LoginTokens[0];

            return new AlphaLocatorConfig
            {
                LocatorHost = RuntimeConfigDefaults.LocatorHost,
                LocatorParameters = new Worker.CInterop.Alpha.LocatorParameters
                {
                    PlayerIdentity = new PlayerIdentityCredentials
                    {
                        PlayerIdentityToken  = pit.PlayerIdentityToken,
                        LoginToken = loginTokenDetails.LoginToken,
                    },
                    UseInsecureConnection = false,
                }
            };
        }

        protected override ConnectionService GetConnectionService()
        {
            return ConnectionService.AlphaLocator;
        }
    }
}
