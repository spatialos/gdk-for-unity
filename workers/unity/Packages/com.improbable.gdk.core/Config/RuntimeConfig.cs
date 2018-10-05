using Improbable.Worker;

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
        public const string ProjectName = "projectName";
        public const string ReceptionistHost = "receptionistHost";
        public const string ReceptionistPort = "receptionistPort";
        public const string WorkerId = "workerId";
        public const string WorkerType = "workerType";
    }
}
