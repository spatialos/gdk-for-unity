using Improbable.Gdk.Core;
using Improbable.Worker;

namespace Improbable.Gdk.Mobile
{
    public abstract class MobileWorkerConnector : WorkerConnector
    {
        protected abstract string GetHostIp();

        protected override ReceptionistConfig GetReceptionistConfig(string workerType)
        {
            return new ReceptionistConfig
            {
                ReceptionistHost = GetHostIp(),
                WorkerType = workerType,
                WorkerId = CreateNewWorkerId(workerType),
                UseExternalIp = true,
                LinkProtocol = NetworkConnectionType.Tcp,
            };
        }

        protected override LocatorConfig GetLocatorConfig(string workerType)
        {
            throw new System.NotImplementedException("The locator flow is currently not available for mobile workers.");
        }

        protected override bool ShouldUseLocator()
        {
            return false;
        }
    }
}
