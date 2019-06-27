using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Mobile;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground
{
    public class MobileClientWorkerConnector : WorkerConnector, MobileConnectionFlowInitializer.IMobileSettingsProvider
    {
#pragma warning disable 649
        [SerializeField] private GameObject level;
        [SerializeField] private string ipAddress;
#pragma warning restore 649

        private GameObject levelInstance;

        public async void Start()
        {
            var connParams = CreateConnectionParameters(WorkerUtils.MobileClient, new MobileConnectionParametersInitializer());

            var flowInitializer = new MobileConnectionFlowInitializer(
                new MobileConnectionFlowInitializer.CommandLineSettingsProvider(),
                new MobileConnectionFlowInitializer.PlayerPrefsSettingsProvider(),
                this);

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            switch (flowInitializer.GetConnectionService())
            {
                case ConnectionService.Receptionist:
                    builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.MobileClient),
                        flowInitializer));
                    break;
                case ConnectionService.AlphaLocator:
                    builder.SetConnectionFlow(new AlphaLocatorFlow(flowInitializer));
                    break;
                default:
                    throw new ArgumentException("Received unsupported connection service.");
            }

            await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            WorkerUtils.AddClientSystems(Worker.World);

            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        public override void Dispose()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance);
            }

            base.Dispose();
        }

        public Option<string> GetReceptionistHostIp()
        {
            return string.IsNullOrEmpty(ipAddress) ? Option<string>.Empty : new Option<string>(ipAddress);
        }

        public Option<string> GetDevAuthToken()
        {
            var token = Resources.Load<TextAsset>("DevAuthToken")?.text.Trim();
            return token ?? Option<string>.Empty;
        }

        public Option<ConnectionService> GetConnectionService()
        {
            return Option<ConnectionService>.Empty;
        }
    }
}
