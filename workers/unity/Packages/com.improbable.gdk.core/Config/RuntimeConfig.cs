using System;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Default values for connection parameters.
    /// </summary>
    public static class RuntimeConfigDefaults
    {
        public const NetworkConnectionType LinkProtocol = NetworkConnectionType.RakNet;
        public const string LocatorHost = "locator.improbable.io";
        public const string ReceptionistHost = "127.0.0.1";
        public const ushort ReceptionistPort = 7777;
        public const ushort AnonymousAuthenticationPort = 444;
    }

    /// <summary>
    ///     Command line argument names for worker and connection configuration.
    /// </summary>
    public static class RuntimeConfigNames
    {
        public const string LinkProtocol = "linkProtocol";
        public const string LocatorHost = "locatorHost";
        public const string LoginToken = "loginToken";
        public const string PlayerIdentityToken = "playerIdentityToken";
        public const string ProjectName = "projectName";
        public const string ReceptionistHost = "receptionistHost";
        public const string ReceptionistPort = "receptionistPort";
        public const string SteamDeploymentTag = "steamDeploymentTag";
        public const string SteamTicket = "steamTicket";
        public const string WorkerId = "workerId";
    }

    /// <summary>
    ///     Stores the configuration needed to connect via the Lcoator.
    /// </summary>
    public struct LocatorConfig
    {
        /// <summary>
        ///     The host to connect to the Locator.
        /// </summary>
        public string LocatorHost;

        /// <summary>
        ///     The parameters needed to connect to the Locator.
        /// </summary>
        public LocatorParameters LocatorParameters;

        /// <summary>
        /// A function that takes as input a list of available deployments and
        /// returns the name of the deployment that we want to connect to via the Locator.
        /// </summary>
        public Func<DeploymentList, string> DeploymentListCallback;
    }

    /// <summary>
    ///     Stores the configuration needed to connect via the Alpha Locator.
    /// </summary>
    public struct AlphaLocatorConfig
    {
        /// <summary>
        ///     The host used to connect to the Locator.
        /// </summary>
        public string LocatorHost;

        /// <summary>
        ///     The parameters needed to connect to the Alpha Locator.
        /// </summary>
        public Improbable.Worker.CInterop.Alpha.LocatorParameters LocatorParameters;
    }

    /// <summary>
    ///     Stores the configuration needed to connect via the Receptionist.
    /// </summary>
    public struct ReceptionistConfig
    {
        /// <summary>
        ///     The host used to connect to the Receptionist.
        /// </summary>
        public string ReceptionistHost;

        /// <summary>
        ///     The port used to connect to the Receptionist.
        /// </summary>
        public ushort ReceptionistPort;

        /// <summary>
        ///     The Id of the worker that we want to connect via the Receptionist.
        /// </summary>
        public string WorkerId;
    }

    /// <summary>
    ///     An enum listing the available connection services.
    /// </summary>
    public enum ConnectionService
    {
        Receptionist,
        Locator,
        AlphaLocator,
    }
}
