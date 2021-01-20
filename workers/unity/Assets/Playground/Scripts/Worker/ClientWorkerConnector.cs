using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground
{
    public class ClientWorkerConnector : WorkerConnector
    {
        public const string UnityClient = "UnityClient";

#pragma warning disable 649
        [SerializeField] private EntityRepresentationMapping entityRepresentationMapping;
        [SerializeField] private bool UseExternalIp;
        [SerializeField] private GameObject level;
#pragma warning restore 649

        private GameObject levelInstance;

        private async void Start()
        {
            Application.targetFrameRate = 60;

            var connParams = CreateConnectionParameters(UnityClient);
            connParams.Network.UseExternalIp = UseExternalIp;

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            if (!Application.isEditor)
            {
                var initializer = new CommandLineConnectionFlowInitializer();
                switch (initializer.GetConnectionService())
                {
                    case ConnectionService.Receptionist:
                        /*
                         * If we are connecting via the Receptionist we are either:
                         *      - connecting to a local deployment
                         *      - connecting to a cloud deployment via `spatial cloud connect external`
                         * in the first case, the security type must be Insecure.
                         * in the second case, its okay for the security type to be Insecure.
                        */
                        connParams.Network.Kcp.SecurityType = NetworkSecurityType.Insecure;
                        connParams.Network.Tcp.SecurityType = NetworkSecurityType.Insecure;
                        builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(UnityClient), initializer));
                        break;
                    case ConnectionService.Locator:
                        builder.SetConnectionFlow(new LocatorFlow(initializer));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                // We are in the Editor, so for the same reasons as above, the network security type should be Insecure.
                connParams.Network.Kcp.SecurityType = NetworkSecurityType.Insecure;
                connParams.Network.Tcp.SecurityType = NetworkSecurityType.Insecure;
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(UnityClient)));
            }

            await Connect(builder, new ForwardingDispatcher());

            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            TransformSynchronizationHelper.AddClientSystems(Worker.World);
            PlayerLifecycleHelper.AddClientSystems(Worker.World);
            GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, entityRepresentationMapping);

            Worker.World.GetOrCreateSystem<DisconnectSystem>();

            Worker.World.GetOrCreateSystem<ProcessColorChangeSystem>();
            Worker.World.GetOrCreateSystem<LocalPlayerInputSync>();
            Worker.World.GetOrCreateSystem<MoveLocalPlayerSystem>();
            Worker.World.GetOrCreateSystem<InitCameraSystem>();
            Worker.World.GetOrCreateSystem<FollowCameraSystem>();
            Worker.World.GetOrCreateSystem<UpdateUISystem>();
            Worker.World.GetOrCreateSystem<PlayerCommandsSystem>();
            Worker.World.GetOrCreateSystem<MetricSendSystem>();
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
