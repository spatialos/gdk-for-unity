using Improbable.Gdk.Core;
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
        protected override bool ShouldUseLocator()
        {
            return true;
        }

        /// <summary>
        ///     Creates a Locator configuration where the Steam ticket field is populated through an API call.
        /// </summary>
        /// <remarks>
        ///     All other configuration parameters are populated according to the logic of the parent class implementation.
        ///     (Based on the command line arguments)
        /// </remarks>
        /// <param name="workerType">The type of the worker to create.</param>
        /// <returns>The Locator connection configuration</returns>
        protected override LocatorConfig GetLocatorConfig(string workerType)
        {
            var config = base.GetLocatorConfig(workerType);
            var result = GetSteamAuthSessionTicketSteamworksNET(out var steamTicket, out var errorMessage);
            // var result = GetSteamAuthSessionTicketFacepunch(out var steamTicket, out var errorMessage); // Comment/un-comment this line depending on your Steamworks library of choice.
            if (!result)
            {
                Debug.LogError("Error getting Steam auth session ticket: " + errorMessage);
            }
            else
            {
                Debug.LogError("Jonas success " + steamTicket);
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

            steamTicket = System.BitConverter.ToString(response.Data, 0, response.Data.Length).Replace("-", "");
            return true;
            */
        }
    }
}
