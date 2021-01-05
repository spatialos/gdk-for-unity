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
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(UnityClient)));
                connParams.Network.Kcp.SecurityType = NetworkSecurityType.Insecure;
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
