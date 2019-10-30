using System.Collections.Generic;
using System.IO;

namespace Improbable.Gdk.DeploymentLauncher.EditmodeTests.Utils
{
    internal static class DeploymentConfigTestUtils
    {
        internal static string ValidProjectName = "gdk_for_unity";
        internal static string ValidAssemblyName = "gdk_for_unity_test_assembly";

        private static string ValidDeploymentName = "valid_deployment_name";
        private static DeploymentRegionCode ValidRegionCode = DeploymentRegionCode.EU;
        private static List<string> ValidTags => new List<string> { "dev_login", "another_random_tag", "hunter2" };
        private static string ValidLaunchJson = "default_launch.json";
        private static string ValidSnapshotPath = Path.Combine("snapshots", "default.snapshot");

        internal static BaseDeploymentConfig GetValidBaseConfig()
        {
            return new BaseDeploymentConfig
            {
                Name = ValidDeploymentName,
                Region = ValidRegionCode,
                Tags = ValidTags,
                LaunchJson = ValidLaunchJson,
                SnapshotPath = ValidSnapshotPath
            };
        }

        internal static SimulatedPlayerDeploymentConfig GetValidSimPlayerConfig()
        {
            return new SimulatedPlayerDeploymentConfig
            {
                // sim player dpl config details
                TargetDeploymentName = ValidDeploymentName,
                FlagPrefix = "test",
                WorkerType = "TestSimPlayerCoordinator",
                WorkerTypeId = 0,

                // base dpl config details
                Name = $"{ValidDeploymentName}_simplayers",
                Region = ValidRegionCode,
                Tags = ValidTags,
                LaunchJson = ValidLaunchJson,
                SnapshotPath = ValidSnapshotPath
            };
        }

        internal static DeploymentConfig GetValidDeploymentConfig()
        {
            return new DeploymentConfig
            {
                ProjectName = ValidProjectName,
                AssemblyName = ValidAssemblyName,
                Deployment = GetValidBaseConfig(),
                SimulatedPlayerDeploymentConfigs = new List<SimulatedPlayerDeploymentConfig>
                {
                    GetValidSimPlayerConfig()
                }
            };
        }
    }
}
