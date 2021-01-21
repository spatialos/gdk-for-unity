using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.LoadBalancing;
using Improbable.Generated;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground
{
    public class LoadBalancerWorkerConnector : WorkerConnector
    {
        public const string LoadBalancer = "UnityLoadBalancer";

#pragma warning disable 649
        [SerializeField] private EntityRepresentationMapping entityRepresentationMapping;
#pragma warning restore 649

        private async void Start()
        {
            Application.targetFrameRate = 60;

            IConnectionFlow flow;
            ConnectionParameters connectionParameters;

            if (Application.isEditor)
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(LoadBalancer));
                connectionParameters = CreateConnectionParameters(LoadBalancer);

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
                flow = new ReceptionistFlow(CreateNewWorkerId(LoadBalancer),
                    new CommandLineConnectionFlowInitializer());
                connectionParameters = CreateConnectionParameters(LoadBalancer,
                    new CommandLineConnectionParameterInitializer());
            }

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionFlow(flow)
                .SetConnectionParameters(connectionParameters);

            await Connect(builder, new ForwardingDispatcher());
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, entityRepresentationMapping, gameObject);

            Worker.World.GetOrCreateSystem<DisconnectSystem>();

            Worker.AddLoadBalancingSystems(configuration =>
            {
                configuration.AddPartitionManagement(ClientWorkerConnector.UnityClient, MobileClientWorkerConnector.MobileClient, GameLogicWorkerConnector.UnityGameLogic);
                configuration.AddClientLoadBalancing("Character", ComponentSets.PlayerClientSet);

                var loadBalancingMap = new EntityLoadBalancingMap(ComponentSets.DefaultServerSet)
                    .AddOverride("Character", ComponentSets.PlayerServerSet);

                configuration.SetPointOfInterestLoadBalancing(GameLogicWorkerConnector.UnityGameLogic, new[] { new Coordinates(-1, 0, 0), new Coordinates(1, 0, 0) }, loadBalancingMap);
            });
        }
    }
}
