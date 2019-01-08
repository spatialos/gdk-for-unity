using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
#if UNITY_ANDROID
using Improbable.Gdk.Mobile.Android;
#endif
using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop.Alpha;
using UnityEngine;


namespace Playground
{
    public class AndroidClientWorkerConnector : MobileWorkerConnector, IMobileConnectionController
    {
        public string IpAddress { get; set; }
        public ConnectionScreenController ConnectionScreenController { get; set; }

        [SerializeField] private GameObject level;

        private GameObject levelInstance;
        private ConnectionService connectionService;

        public async void TryConnectAsync(ConnectionService connectionService)
        {
            this.connectionService = connectionService;
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

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        protected override void HandleWorkerConnectionFailure(string errorMessage)
        {
            ConnectionScreenController.OnConnectionFailed(errorMessage);
        }

        protected override string GetHostIp()
        {
#if UNITY_ANDROID
            if (!string.IsNullOrEmpty(IpAddress))
            {
                return IpAddress;
            }

            if (Application.isMobilePlatform && AndroidDeviceInfo.ActiveDeviceType == MobileDeviceType.Virtual)
            {
                return AndroidDeviceInfo.EmulatorDefaultCallbackIp;
            }

            return RuntimeConfigDefaults.ReceptionistHost;
#else
            throw new PlatformNotSupportedException(
                $"{nameof(AndroidClientWorkerConnector)} can only be used for the Android platform. Please check your build settings.");
#endif
        }

        protected override string GetPlayerId()
        {
            return $"Player-{Guid.NewGuid()}";
        }

        protected override string GetDisplayName()
        {
            return string.Empty;
        }

        protected override ConnectionService GetConnectionService()
        {
            return connectionService;
        }

        protected override string SelectLoginToken(List<LoginTokenDetails> loginTokens)
        {
            return loginTokens[0].LoginToken;
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
