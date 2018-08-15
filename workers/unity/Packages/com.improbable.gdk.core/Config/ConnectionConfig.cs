using Improbable.Worker;

namespace Improbable.Gdk.Core
{
    public abstract class ConnectionConfig
    {
        public NetworkConnectionType LinkProtocol = NetworkConnectionType.RakNet;
        public bool EnableProtocolLoggingAtStartup = false;
        public bool UseExternalIp = false;
        public string WorkerId;
        public string WorkerType;

        public abstract void Validate();

        protected void ValidateConfig(string configValue, string configName)
        {
            if (string.IsNullOrEmpty(configValue))
            {
                throw new ConnectionFailedException(
                    $"Config validation failed with: No valid {configName} has been provided",
                    ConnectionErrorReason.InvalidConfig);
            }
        }
    }
}
