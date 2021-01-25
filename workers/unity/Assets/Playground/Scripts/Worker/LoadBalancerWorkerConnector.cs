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
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerTypes.UnityLoadBalancer));
                connectionParameters = CreateConnectionParameters(WorkerTypes.UnityLoadBalancer);

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
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerTypes.UnityLoadBalancer),
                    new CommandLineConnectionFlowInitializer());
                connectionParameters = CreateConnectionParameters(WorkerTypes.UnityLoadBalancer,
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
                configuration.AddPartitionManagement(WorkerTypes.UnityClient, WorkerTypes.MobileClient, WorkerTypes.UnityGameLogic);
                configuration.AddClientLoadBalancing("Character", ComponentSets.PlayerClientSet);

                var loadBalancingMap = new EntityLoadBalancingMap(ComponentSets.DefaultServerSet)
                    .AddOverride("Character", ComponentSets.PlayerServerSet);

                configuration.SetPointOfInterestLoadBalancing(WorkerTypes.UnityGameLogic, new[] { new Coordinates(-1, 0, 0), new Coordinates(1, 0, 0) }, loadBalancingMap);
            });
        }
    }
}
