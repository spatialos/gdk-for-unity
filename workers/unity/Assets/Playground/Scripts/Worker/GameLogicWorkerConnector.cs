using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace Playground
{
    public class GameLogicWorkerConnector : WorkerConnector
    {
#pragma warning disable 649
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
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityGameLogic));
                connectionParameters = CreateConnectionParameters(WorkerUtils.UnityGameLogic);
            }
            else
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityGameLogic),
                    new CommandLineConnectionFlowInitializer());
                connectionParameters = CreateConnectionParameters(WorkerUtils.UnityGameLogic,
                    new CommandLineConnectionParameterInitializer());
            }

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionFlow(flow)
                .SetConnectionParameters(connectionParameters);

            await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            WorkerUtils.AddGameLogicSystems(Worker.World);
            
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
