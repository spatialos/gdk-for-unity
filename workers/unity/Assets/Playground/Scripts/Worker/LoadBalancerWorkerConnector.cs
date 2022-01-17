using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.LoadBalancing;
using Improbable.Generated;
using Improbable.Restricted;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground
{
    public class LoadBalancerWorkerConnector : WorkerConnector
    {
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

            BootstrapLoadbalacingPartition();
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            Worker.World.GetOrCreateSystem<DisconnectSystem>();

            Worker.AddLoadBalancingSystems(configuration =>
            {
                configuration.AddPartitionManagement(WorkerTypes.UnityClient, WorkerTypes.MobileClient, WorkerTypes.UnityGameLogic);

                var pointsOfInterest = new[]
                {
                    Coordinates.Zero
                };

                configuration.SetPointOfInterestLoadBalancing(WorkerTypes.UnityGameLogic, pointsOfInterest, new[]
                {
                    ComponentSets.SpinnerServerSet.ComponentSetId,
                    ComponentSets.CubeServerSet.ComponentSetId,
                    ComponentSets.PlayerCreatorServerSet.ComponentSetId,
                    ComponentSets.PlayerServerSet.ComponentSetId,
                });
            });
        }

        private void BootstrapLoadbalacingPartition()
        {
            Worker.World.GetExistingSystem<CommandSystem>()
                .SendCommand(new Improbable.Restricted.Worker.AssignPartition.Request(Worker.WorkerEntityId, new AssignPartitionRequest(EntityTemplates.LoadBalancerPartitionEntityId.Id
                )));
        }
    }
}
