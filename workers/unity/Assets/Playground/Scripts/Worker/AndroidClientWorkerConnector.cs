using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
#if UNITY_ANDROID
using Improbable.Gdk.Mobile.Android;
#endif
using System;
using UnityEngine;


namespace Playground
{
    public class AndroidClientWorkerConnector : MobileWorkerConnector, IMobileClient
    {
        public string IpAddress { get; set; }
        public ConnectionScreenController ConnectionScreenController { get; set; }

        [SerializeField] private GameObject level;

        private GameObject levelInstance;

        public async void TryConnect()
        {
            await Connect(WorkerUtils.AndroidClient, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            ConnectionScreenController.OnConnectionSucceeded();
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
#if UNITY_ANDROID
            if (Application.isMobilePlatform && DeviceInfo.IsAndroidStudioEmulator() && IpAddress.Equals(string.Empty))
            {
                return DeviceInfo.AndroidStudioEmulatorDefaultCallbackIp;
            }

            return IpAddress;
#else
            throw new PlatformNotSupportedException(
                "AndroidClientWorkerConnector can only be used for the Android platform. Please check your build settings.");
#endif
        }
    }
}
