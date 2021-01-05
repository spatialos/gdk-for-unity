using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;
using Playground.LoadBalancing;
using UnityEngine;

namespace Playground
{
    public class GameLogicWorkerConnector : WorkerConnector
    {
        public const string UnityGameLogic = "UnityGameLogic";

#pragma warning disable 649
        [SerializeField] private EntityRepresentationMapping entityRepresentationMapping;
        [SerializeField] private bool UseExternalIp;
        [SerializeField] private GameObject level;
#pragma warning restore 649

        private GameObject levelInstance;

        private async void Start()
        {
            Application.targetFrameRate = 60;

            IConnectionFlow flow;
            ConnectionParameters connectionParameters;

            if (Application.isEditor)
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(UnityGameLogic));
                connectionParameters = CreateConnectionParameters(UnityGameLogic);
                connectionParameters.Network.Kcp.SecurityType = NetworkSecurityType.Insecure;
            }
            else
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(UnityGameLogic),
                    new CommandLineConnectionFlowInitializer());
                connectionParameters = CreateConnectionParameters(UnityGameLogic,
                    new CommandLineConnectionParameterInitializer());
            }

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionFlow(flow)
                .SetConnectionParameters(connectionParameters);

            await Connect(builder, new ForwardingDispatcher());

            if (level == null)
            {
                return;
            }

            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            TransformSynchronizationHelper.AddServerSystems(Worker.World);
            PlayerLifecycleHelper.AddServerSystems(Worker.World);
            GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, entityRepresentationMapping, gameObject);

            Worker.World.GetOrCreateSystem<DisconnectSystem>();

            // Game logic systems
            Worker.World.GetOrCreateSystem<TriggerColorChangeSystem>();
            Worker.World.GetOrCreateSystem<ProcessLaunchCommandSystem>();
            Worker.World.GetOrCreateSystem<ProcessRechargeSystem>();
            Worker.World.GetOrCreateSystem<MetricSendSystem>();
            Worker.World.GetOrCreateSystem<ProcessScoresSystem>();
            Worker.World.GetOrCreateSystem<CubeMovementSystem>();

            // Load balancing systems
            Worker.World.GetOrCreateSystem<ClientPartitionsSystem>();
            Worker.World.GetOrCreateSystem<AssignEntitiesSystem>();
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
