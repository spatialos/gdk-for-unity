using System;
using System.Threading.Tasks;
using Improbable.SpatialOS.Deployment.V1Alpha1;
using Improbable.SpatialOS.Platform.Common;

namespace SpotShim
{
    class Program
    {
        private const ushort SpatialdPort = 9876;

        public static async Task Main(string[] args)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("Expected usage: SpotShim.exe <deployment_id> <deployment_name> <project_name>");
            }

            var localApiEndpoint = new PlatformApiEndpoint("localhost", SpatialdPort, insecure: true);
            var deploymentServiceClient = DeploymentServiceClient.Create(localApiEndpoint);

            var request = new UpdateDeploymentRequest
            {
                Deployment = new Deployment
                {
                    Id = args[0],
                    Name = args[1],
                    ProjectName = args[2],
                    Tag = { "dev_login" }
                }
            };

            await deploymentServiceClient.UpdateDeploymentAsync(request);
        }
    }
}
