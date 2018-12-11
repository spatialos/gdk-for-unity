using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Mobile
{
    public abstract class MobileWorkerConnector : WorkerConnector
    {
        protected abstract string GetHostIp();

        protected override ConnectionParameters GetConnectionParameters(string workerType, ConnectionService service)
        {
            return new ConnectionParameters
            {
                WorkerType = workerType,
                Network =
                {
                    ConnectionType = NetworkConnectionType.Tcp,
                    UseExternalIp = true,
                },
                EnableProtocolLoggingAtStartup = false,
                DefaultComponentVtable = new ComponentVtable(),
            };
        }

        protected override ReceptionistConfig GetReceptionistConfig(string workerType)
        {
            return new ReceptionistConfig
            {
                ReceptionistHost = GetHostIp(),
                ReceptionistPort = RuntimeConfigDefaults.ReceptionistPort,
                WorkerId = CreateNewWorkerId(workerType)
            };
        }

        protected override LocatorConfig GetLocatorConfig()
        {
            throw new System.NotImplementedException("The locator flow is currently not available for mobile workers.");
        }

        protected override AlphaLocatorConfig GetAlphaLocatorConfig()
        {
            throw new System.NotImplementedException("The alpha locator flow is currently not available for mobile workers.");
        }

        protected override ConnectionService GetConnectionService()
        {
            return ConnectionService.Receptionist;
        }
    }
}
