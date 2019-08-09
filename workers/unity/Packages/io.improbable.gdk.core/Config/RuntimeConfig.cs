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
        public const string LocalEnvironment = "local";
        public const string CloudEnvironment = "cloud";
        public const ushort ReceptionistPort = 7777;
        public const ushort LocatorPort = 444;
    }

    /// <summary>
    ///     Command line argument names for worker and connection configuration.
    /// </summary>
    public static class RuntimeConfigNames
    {
        public const string LinkProtocol = "linkProtocol";
        public const string LocatorHost = "locatorHost";
        public const string LoginToken = "loginToken";
        public const string DevAuthTokenKey = "devAuthTokenSecret";
        public const string PlayerIdentityToken = "playerIdentityToken";
        public const string ProjectName = "projectName";
        public const string ReceptionistHost = "receptionistHost";
        public const string ReceptionistPort = "receptionistPort";
        public const string SteamDeploymentTag = "steamDeploymentTag";
        public const string SteamTicket = "steamTicket";
        public const string WorkerId = "workerId";
        public const string Environment = "environment";
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
