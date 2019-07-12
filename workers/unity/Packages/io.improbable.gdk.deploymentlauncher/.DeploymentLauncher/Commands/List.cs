using System.Linq;
using Improbable.SpatialOS.Deployment.V1Alpha1;

namespace Improbable.Gdk.DeploymentLauncher.Commands
{
    public static class List
    {
        public static int ListDeployments(Options.List options)
        {
            var deploymentServiceClient = DeploymentServiceClient.Create();
            var listDeploymentsResult = deploymentServiceClient.ListDeployments(new ListDeploymentsRequest
            {
                ProjectName = options.ProjectName
            });

            Ipc.WriteDeploymentInfo(listDeploymentsResult.Where(deployment =>
                deployment.Status == Deployment.Types.Status.Running));

            return Program.SuccessExitCode;
        }
    }
}
