using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
#if UNITY_IOS
using Improbable.Gdk.Mobile.iOS;
#endif
using System;
using System.Collections.Generic;
using Improbable.Worker.CInterop.Alpha;
using UnityEngine;

namespace Playground
{
    public class iOSClientWorkerConnector : MobileWorkerConnector, IMobileConnectionController
    {
        public string IpAddress { get; set; }
        public ConnectionScreenController ConnectionScreenController { get; set; }

        [SerializeField] private GameObject level;

        private GameObject levelInstance;
        private ConnectionService connectionService;

        public async void TryConnectAsync(ConnectionService connectionService)
        {
            this.connectionService = connectionService;
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
            if (!string.IsNullOrEmpty(IpAddress))
            {
                return IpAddress;
            }

            return RuntimeConfigDefaults.ReceptionistHost;
#else
            throw new PlatformNotSupportedException(
                $"{nameof(iOSClientWorkerConnector)} can only be used for the iOS platform. Please check your build settings.");
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
