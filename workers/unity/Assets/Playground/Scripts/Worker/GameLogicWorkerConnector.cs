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

            var connParams = CreateConnectionParameters(WorkerUtils.UnityGameLogic);
            connParams.Network.UseExternalIp = UseExternalIp;

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            if (Application.isEditor)
            {
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityGameLogic)));
            }
            else
            {
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerUtils.UnityGameLogic), new CommandLineConnectionFlowInitializer()));
            }

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
