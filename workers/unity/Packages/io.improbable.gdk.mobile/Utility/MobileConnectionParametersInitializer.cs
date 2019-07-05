using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Mobile
{
    public class MobileConnectionParametersInitializer : IConnectionParameterInitializer
    {
        public void Initialize(ConnectionParameters connectionParameters)
        {
            connectionParameters.Network = new NetworkParameters
            {
                UseExternalIp = true,
                ConnectionType = NetworkConnectionType.Kcp,
                Kcp = new KcpNetworkParameters
                {
                    // These are the last tested values
                    Heartbeat = new HeartbeatParameters
                    {
                        IntervalMillis = 5000,
                        TimeoutMillis = 10000
                    }
                }
            };
        }
    }
}
