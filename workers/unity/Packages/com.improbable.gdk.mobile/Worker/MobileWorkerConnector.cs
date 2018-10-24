using Improbable.Gdk.Core;
using Improbable.Worker;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

        protected override bool ShouldUseLocator()
        {
            // We currently only support local deployments for Mobile
            return false;
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
