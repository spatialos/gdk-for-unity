using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
#if UNITY_IOS
using Improbable.Gdk.Mobile.iOS;
#endif
using System;
using UnityEngine;

namespace Playground
{
    public class iOSClientWorkerConnector : MobileWorkerConnector, IMobileConnectionController
    {
        public string IpAddress { get; set; }
        public ConnectionScreenController ConnectionScreenController { get; set; }

        [SerializeField] private GameObject level;

        private GameObject levelInstance;

        public async void TryConnect()
        {
            await Connect(WorkerUtils.iOSClient, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            ConnectionScreenController.OnConnectionSucceeded();
            WorkerUtils.AddClientSystems(Worker.World);

            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        protected override void HandleWorkerConnectionFailure(string errorMessage)
        {
            ConnectionScreenController.OnConnectionFailed(errorMessage);
        }

        protected override string GetHostIp()
        {
#if UNITY_IOS
            if (Application.isEditor)
            {
                return RuntimeConfigDefaults.ReceptionistHost;
            }

            if (Application.isMobilePlatform && iOSDeviceInfo.ActiveDeviceType == MobileDeviceType.Virtual && string.Empty.Equals(IpAddress))
            {
                return RuntimeConfigDefaults.ReceptionistHost;
            }

            return IpAddress;
#else
            throw new PlatformNotSupportedException(
                $"{nameof(iOSClientWorkerConnector)} can only be used for the iOS platform. Please check your build settings.");
#endif
        }

        public override void Dispose()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance);
            }

            base.Dispose();
        }
    }
}
