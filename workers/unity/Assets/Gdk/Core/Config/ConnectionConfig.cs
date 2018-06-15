using Improbable.Worker;

namespace Improbable.Gdk.Core
{
    public abstract class ConnectionConfig
    {
        public NetworkConnectionType LinkProtocol = NetworkConnectionType.RakNet;
        public bool EnableProtocolLoggingAtStartup = false;
        public bool UseExternalIp = false;

        public abstract void Validate();

        protected void ValidateConfig(string configValue, string configName)
        {
            if (string.IsNullOrEmpty(configValue))
            {
                throw new System.ArgumentException($"No valid {configName} has been provided");
            }
        }
    }
}
