using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.Mobile;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground
{
    public class MobileClientWorkerConnector : WorkerConnector, MobileConnectionFlowInitializer.IMobileSettingsProvider
    {
        public const string MobileClient = "MobileClient";

#pragma warning disable 649
        [SerializeField] private EntityRepresentationMapping entityRepresentationMapping;
        [SerializeField] private GameObject level;
        [SerializeField] private string ipAddress;
#pragma warning restore 649

        private GameObject levelInstance;

        public async void Start()
        {
            var connParams = CreateConnectionParameters(MobileClient, new MobileConnectionParametersInitializer());

            var flowInitializer = new MobileConnectionFlowInitializer(
                new MobileConnectionFlowInitializer.CommandLineSettingsProvider(),
                new MobileConnectionFlowInitializer.PlayerPrefsSettingsProvider(),
                this);

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            switch (flowInitializer.GetConnectionService())
            {
                case ConnectionService.Receptionist:
                    connParams.Network.Kcp.SecurityType = NetworkSecurityType.Insecure;
                    builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(MobileClient),
                        flowInitializer));
                    break;
                case ConnectionService.Locator:
                    builder.SetConnectionFlow(new LocatorFlow(flowInitializer));
                    break;
                default:
                    throw new ArgumentException("Received unsupported connection service.");
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
