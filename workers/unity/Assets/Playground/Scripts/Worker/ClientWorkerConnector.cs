using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using Playground;

public class ClientWorkerConnector : WorkerConnectorBase
{
    private async void Start()
    {
        await Connect(WorkerUtils.UnityClient, new ForwardingDispatcher()).ConfigureAwait(false);
    }

    protected override void AddWorkerSystems()
    {
        WorkerUtils.AddClientSystems(Worker.World);
    }

    protected override string SelectDeploymentName(DeploymentList deployments)
    {
        // This could be replaced with a splash screen asking to select a deployment or some other user-defined logic.
        return deployments.Deployments[0].DeploymentName;
    }
}
