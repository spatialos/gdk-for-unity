using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Generated;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground
{
    public class GameLogicWorkerConnector : WorkerConnector
    {
#pragma warning disable 649
        [SerializeField] private EntityRepresentationMapping entityRepresentationMapping;
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
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerTypes.UnityGameLogic));
                connectionParameters = CreateConnectionParameters(WorkerTypes.UnityGameLogic);

                /*
                 * If we are in the Editor, it means we are either:
                 *      - connecting to a local deployment
                 *      - connecting to a cloud deployment via `spatial cloud connect external`
                 * in the first case, the security type must be Insecure.
                 * in the second case, its okay for the security type to be Insecure.
                */
                connectionParameters.Network.Kcp.SecurityType = NetworkSecurityType.Insecure;
                connectionParameters.Network.Tcp.SecurityType = NetworkSecurityType.Insecure;
            }
            else
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerTypes.UnityGameLogic),
                    new CommandLineConnectionFlowInitializer());
                connectionParameters = CreateConnectionParameters(WorkerTypes.UnityGameLogic,
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
