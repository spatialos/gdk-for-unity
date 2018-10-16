using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
#if UNITY_IOS
using Improbable.Gdk.Mobile.Ios;
#endif
using System;
using UnityEngine;

namespace Playground
{
    public class iOSClientWorkerConnector : MobileWorkerConnector
    {
        [NonSerialized] public string IpAddress;
        [NonSerialized] public ConnectionScreenController ConnectionScreenController;

        [SerializeField] private GameObject level;

        private GameObject levelInstance;

        public async void TryConnect()
        {
            await Connect(WorkerUtils.iOSClient, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            ConnectionScreenController.OnSuccess();

            WorkerUtils.AddClientSystems(Worker.World);
            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform);
            levelInstance.transform.SetParent(null);
        }

        protected override void HandleWorkerConnectionFailure()
        {
            ConnectionScreenController.OnConnectionFailed();
        }

        protected override string GetHostIp()
        {
#if UNITY_IOS
            if ((Application.isEditor || DeviceInfo.IsIosSimulator()) && IpAddress.Equals(string.Empty))
            {
                return RuntimeConfigDefaults.ReceptionistHost;
            }

            return IpAddress;
#else
            throw new PlatformNotSupportedException(
                "This method is only defined for the iOS platform. Please check your build settings.");
#endif
        }
    }
}
