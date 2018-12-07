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
        public const string WorkerType = "workerType";
    }

    /// <summary>
    ///     Stores the configuration needed to connect via the Lcoator.
    /// </summary>
    public struct LocatorConfig
    {
        public string LocatorHost;
        public LocatorParameters LocatorParameters;
        public Func<DeploymentList, string> DeploymentListCallback;
    }

    /// <summary>
    ///     Stores the configuration needed to connect via the Alpha Locator.
    /// </summary>
    public struct AlphaLocatorConfig
    {
        public string LocatorHost;
        public Improbable.Worker.CInterop.Alpha.LocatorParameters LocatorParameters;
    }

    /// <summary>
    ///     Stores the configuration needed to connect via the Receptionist.
    /// </summary>
    public struct ReceptionistConfig
    {
        public string ReceptionistHost;
        public ushort ReceptionistPort;
        public string WorkerId;
    }

    public enum ConnectionService
    {
        Receptionist,
        Locator,
        AlphaLocator,
    }
}
