using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Playground;

public class ClientWorkerConnector : WorkerConnectorBase
{
    private async void Start()
    {
        // Array covariance means the explicit cast isn't strictly needed but it is slightly safer
        RequiredWorkerConnection = FindObjectsOfType<GameLogicWorkerConnector>()
            .Select(c => (WorkerConnectorBase) c).ToArray();
        await Connect(WorkerUtils.UnityClient, new ForwardingDispatcher()).ConfigureAwait(false);
    }

    protected override void AddWorkerSystems()
    {
        WorkerUtils.AddClientSystems(Worker.World);
    }

    protected override string SelectDeploymentName(DeploymentList deployments)
    {
        // This could be replaced with a splash screen asking to select a deployment or some other user defined logic
        return deployments.Deployments[0].DeploymentName;
    }
}
