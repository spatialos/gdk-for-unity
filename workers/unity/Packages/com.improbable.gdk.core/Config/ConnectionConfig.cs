using Improbable.Worker;
using Improbable.Worker.Core;

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

        public ConnectionParameters CreateConnectionParameters()
        {
            var connectionParameters = new ConnectionParameters
            {
                WorkerType = WorkerType,
                Network =
                {
                    ConnectionType = LinkProtocol,
                    UseExternalIp = UseExternalIp,
                },
                EnableProtocolLoggingAtStartup = EnableProtocolLoggingAtStartup,
                DefaultComponentVtable = new PassthroughComponentVtable()
            };
            return connectionParameters;
        }

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
