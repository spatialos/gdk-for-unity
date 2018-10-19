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
                UseExternalIp = UseExternalIp,
                LinkProtocol = NetworkConnectionType.Tcp,
            };
        }
    }
}
