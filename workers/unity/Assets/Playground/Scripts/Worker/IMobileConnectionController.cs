using Improbable.Gdk.Core;

namespace Playground
{
    public interface IMobileConnectionController
    {
        string IpAddress { get; set; }

        ConnectionScreenController ConnectionScreenController { get; set; }

        void TryConnectAsync(ConnectionService connectionService);
    }
}
