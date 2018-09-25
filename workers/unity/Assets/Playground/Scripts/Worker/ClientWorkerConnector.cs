using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using UnityEngine;

namespace Playground
{
    public class ClientWorkerConnector : WorkerConnector
    {
        public GameObject Level;

        private GameObject levelInstance;

        private async void Start()
        {
            await Connect(WorkerUtils.UnityClient, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override string SelectDeploymentName(DeploymentList deployments)
        {
            // This could be replaced with a splash screen asking to select a deployment or some other user-defined logic.
            return deployments.Deployments[0].DeploymentName;
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            WorkerUtils.AddClientSystems(Worker.World);
            if (Level == null)
            {
                return;
            }

            levelInstance = Instantiate(Level, transform);
            levelInstance.transform.SetParent(null);
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
