using System;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground
{
    public class ClientWorkerConnector : WorkerConnector
    {
#pragma warning disable 649
        [SerializeField] private bool UseExternalIp;
        [SerializeField] private GameObject level;
#pragma warning restore 649

        private GameObject levelInstance;

        private async void Start()
        {
            Application.targetFrameRate = 60;

            var connParams = CreateConnectionParameters(WorkerUtils.UnityClient);
            connParams.Network.UseExternalIp = UseExternalIp;
            connParams.Network.ConnectionType = NetworkConnectionType.RakNet;

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            if (!Application.isEditor)
            {
                var initializer = new CommandLineConnectionFlowInitializer();
                switch (initializer.GetConnectionService())
                {
                    case ConnectionService.Receptionist:
                        builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityClient), initializer));
                        break;
                    case ConnectionService.Locator:
                        builder.SetConnectionFlow(new LocatorFlow(initializer));
                        break;
                    case ConnectionService.AlphaLocator:
                        builder.SetConnectionFlow(new AlphaLocatorFlow(initializer));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityClient)));
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
    }
}
