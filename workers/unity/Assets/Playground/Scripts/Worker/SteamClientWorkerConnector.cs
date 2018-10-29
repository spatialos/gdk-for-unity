using System;
using Improbable.Gdk.Core;
using Improbable.Worker;
using UnityEngine;

namespace Playground
{
    /// <summary>
    ///     The SteamClientWorkerConnector class provides a sample solution for connecting a game client published
    ///     on Steam to a SpatialOS deployment. It provides example code for retrieving a
    ///     Steam auth session token at runtime using either Steamworks.NET (https://steamworks.github.io) or
    ///     Facepunch.Steamworks (https://github.com/Facepunch/Facepunch.Steamworks).
    ///     You can also use the ClientWorkerConnector parent class to parse the Steam auth session token
    ///     from the commandline arguments in case the Steam auth session token is known in advance.
    ///     You will need to install either Steamworks.NET (https://steamworks.github.io) or
    ///     Facepunch.Steamworks (https://github.com/Facepunch/Facepunch.Steamworks) in order to use this code.
    /// </summary>
    public class SteamClientWorkerConnector : ClientWorkerConnector
    {
        private bool attemptedToConnect = false;

        // We have an empty Start() method here to overwrite the method implementation of the parent class.
        // Unlike the parent class, we do not attempt to connect during Start() because the Steam client
        // that we depend on for retrieving a Steam auth session token might not be initialized yet at that time.
        private void Start()
        {
        }

        private async void Update()
        {
            if (attemptedToConnect)
            {
                return;
            }

            // Attempt to connect during the first time Update() is called.
            await Connect(WorkerUtils.UnityClient, new ForwardingDispatcher()).ConfigureAwait(false);
            attemptedToConnect = true;
        }

        protected override bool ShouldUseLocator()
        {
            return true;
        }

        /// <summary>
        ///     Creates a Locator configuration where the Steam ticket field is populated through an API call.
        /// </summary>
        /// <remarks>
        ///     All other configuration parameters are populated through command line arguments
        ///     according to the logic of the parent class implementation. In case a SpatialOS login token is specified,
        ///     this method will not retrieve a Steam auth session ticket and behave just like the parent class method.
        ///     In case a Steam auth session ticket is already specified, this method will not retrieve a Steam auth
        ///     session ticket and behave just like the parent class method (the command line argument takes priority).
        /// </remarks>
        /// <param name="workerType">The type of the worker to create.</param>
        /// <returns>The Locator connection configuration</returns>
        protected override LocatorConfig GetLocatorConfig(string workerType)
        {
            var config = base.GetLocatorConfig(workerType);
            if (config.LocatorParameters.CredentialsType == LocatorCredentialsType.LoginToken)
            {
                // Game client is started via the Launcher. Do not retrieve a Steam auth session ticket and connect normally.
                return config;
            }

            if (!string.IsNullOrEmpty(config.LocatorParameters.Steam.Ticket))
            {
                // Steam auth session ticket has been specified through the command line. The command line takes priority.
                return config;
            }

            var result = GetSteamAuthSessionTicketSteamworksNET(out var steamTicket, out var errorMessage);
            // var result = GetSteamAuthSessionTicketFacepunch(out var steamTicket, out var errorMessage); // Comment/un-comment this line depending on which Steamworks library you are using.
            if (!result)
            {
                Debug.LogError("Error getting Steam auth session ticket: " + errorMessage);
            }
            else
            {
                config.SetSteamTicket(steamTicket);
            }

            return config;
        }

        /// <summary>
        ///     Retrieves a Steam auth session token (referred to as Steam ticket in the SpatialOS worker SDK) for
        ///     authenticating a Steam user trying to join a SpatialOS deployment using Steamworks.NET.
        /// </summary>
        /// <param name="steamTicket">Steam auth session token in case true is returned.</param>
        /// <param name="errorMessage">Reason for failing the retrieval in case false is returned.</param>
        /// <returns>True, if a Steam auth session token was successfully retrieved.</returns>
        private bool GetSteamAuthSessionTicketSteamworksNET(out string steamTicket, out string errorMessage)
        {
            steamTicket = string.Empty;
            errorMessage = "Method not implemented.";
            return false;

            // Uncomment this code and remove the preceding method stub after installing Steamworks.NET. (https://steamworks.github.io)
            /*
            steamTicket = string.Empty;
            errorMessage = string.Empty;
            if (!SteamManager.Initialized)
            {
                errorMessage = "SteamManager is not initialized. " +
                    "Ensure that you have a SteamManager MonoBehaviour instance in the scene and that this code is not called before SteamManager.Awake(). " +
                    "Also ensure that your Steam client is running and that you are logged in.";
                return false;
            }

            var steamTicketData = new byte[1024];
            var hAuthTicket = Steamworks.SteamUser.GetAuthSessionTicket(steamTicketData, 1024, out var steamTicketLength);
            if (hAuthTicket == HAuthTicket.Invalid)
            {
                errorMessage = "SteamUser.GetAuthSessionTicket returned with invalid result.";
                return false;
            }

            steamTicket = System.BitConverter.ToString(steamTicketData, 0, (int) steamTicketLength).Replace("-", "");
            return true;
            */
        }

        /// <summary>
        ///     Retrieves a Steam auth session token (referred to as Steam ticket in the SpatialOS worker SDK) for
        ///     authenticating a Steam user trying to join a SpatialOS deployment using Facepunch.Steamworks.
        /// </summary>
        /// <param name="steamTicket">Steam auth session token in case true is returned.</param>
        /// <param name="errorMessage">Reason for failing the retrieval in case false is returned.</param>
        /// <returns>True, if a Steam auth session token was successfully retrieved.</returns>
        private bool GetSteamAuthSessionTicketFacepunch(out string steamTicket, out string errorMessage)
        {
            steamTicket = string.Empty;
            errorMessage = "Method not implemented.";
            return false;

            // Uncomment this code and remove the preceding method stub after installing Facepunch.Steamworks. (https://github.com/Facepunch/Facepunch.Steamworks)
            /*
            steamTicket = string.Empty;
            errorMessage = string.Empty;
            if (Facepunch.Steamworks.Client.Instance == null)
            {
                errorMessage = "Facepunch client not initialized. " +
                    "Ensure that you have a SteamClient MonoBehaviour instance (Facepunch.Steamworks.Utility.SteamClient) in the scene and that this code is not called before SteamClient.Start(). " +
                    "Also ensure that your Steam client is running and that you are logged in.";
                return false;
            }

            var response = Facepunch.Steamworks.Client.Instance.Auth.GetAuthSessionTicket();
            if (response == null)
            {
                errorMessage = "SteamUser.GetAuthSessionTicket returned with invalid result.";
                return false;
            }

            try
            {
                steamTicket = BitConverter.ToString(response.Data, 0, response.Data.Length).Replace("-", "");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                errorMessage = e.Message;
                return false;
            }

            return true;
            */
        }
    }
}
