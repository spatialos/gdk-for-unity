using Improbable.SpatialOS.Deployment.V1Alpha1;

namespace Improbable.Gdk.DeploymentLauncher.Commands
{
    public static class Stop
    {
        public static int StopDeployment(Options.Stop options)
        {
            var deploymentServiceClient = DeploymentServiceClient.Create();

            try
            {
                deploymentServiceClient.StopDeployment(new StopDeploymentRequest
                {
                    Id = options.DeploymentId,
                    ProjectName = options.ProjectName
                });
            }
            catch (Grpc.Core.RpcException e)
            {
                if (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
                {
                    Ipc.WriteError(Ipc.ErrorCode.NotFound,
                        $"Could not find deployment with ID {options.DeploymentId} in project {options.ProjectName}");
                    return Program.ErrorExitCode;
                }

                throw;
            }

            return Program.SuccessExitCode;
        }
    }
}
