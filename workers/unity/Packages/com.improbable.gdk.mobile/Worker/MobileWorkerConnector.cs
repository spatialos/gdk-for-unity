using Improbable.Gdk.Core;
using Improbable.Worker;
using UnityEditor;

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

        private void OnValidate()
        {
            if (UseExternalIp == false)
            {
                UseExternalIp = true;
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Invalid configuration", "Use External Ip needs to be set to true for mobile client workers.", "Ok");
#endif
            }
        }
    }
}
