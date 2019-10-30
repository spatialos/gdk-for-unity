using System.Linq;
using Improbable.Gdk.DeploymentLauncher.EditmodeTests.Utils;
using NUnit.Framework;

namespace Improbable.Gdk.DeploymentLauncher.EditmodeTests
{
    [TestFixture]
    public class BaseDeploymentConfigTests
    {
        [Test]
        public void BaseDeploymentConfig_does_not_throw_error_when_valid()
        {
            Assert.IsEmpty(DeploymentConfigTestUtils.GetValidBaseConfig().GetErrors());
        }

        [TestCase("demo_dpl_final")]
        [TestCase("demo_dpl_final2")]
        public void BaseDeploymentConfig_does_not_throw_error_for_valid_deployment_name(string deploymentName)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.Name = deploymentName;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("#badname!", DeploymentConfigErrorTypes.Invalid)]
        public void BaseDeploymentConfig_throws_error_for_invalid_deployment_name(string invalidName, string errorType)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.Name = invalidName;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1, "Failed: did not throw exactly one error.");
            Assert.IsTrue(errors[0].Contains("Deployment Name"));
            Assert.IsTrue(errors[0].Contains(errorType));
        }

        [TestCase("default_launch.json")]
        [TestCase("cloud_launch.json")]
        public void BaseDeploymentConfig_does_not_throw_error_for_valid_launch_json_path(string launchJson)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.LaunchJson = launchJson;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("this/launch/config/does/not/exist.json", DeploymentConfigErrorTypes.NotFound)]
        public void BaseDeploymentConfig_throws_error_for_invalid_launch_json_path(string launchJson, string errorType)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.LaunchJson = launchJson;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1, "Failed: did not throw exactly one error.");
            Assert.IsTrue(errors[0].Contains("Launch Config"));
            Assert.IsTrue(errors[0].Contains(errorType));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("snapshots/default.snapshot")]
        public void BaseDeploymentConfig_does_not_throw_error_for_valid_snapshot_path(string snapshotPath)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.SnapshotPath = snapshotPath;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase("this/snapshot/path/does/not/exist.snapshot")]
        public void BaseDeploymentConfig_throws_error_for_invalid_snapshot_path(string snapshotPath)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.SnapshotPath = snapshotPath;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1, "Failed: did not throw exactly one error.");
            Assert.IsTrue(errors[0].Contains("Snapshot"));
            Assert.IsTrue(errors[0].Contains(DeploymentConfigErrorTypes.NotFound));
        }

        [TestCase("dev_login")]
        [TestCase("quinquagintaquadringentillionths")]
        public void BaseDeploymentConfig_does_not_throw_error_for_valid_tag(string validTag)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.Tags.Add(validTag);

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase("_invalidtag")]
        [TestCase("f")]
        [TestCase("gg")]
        [TestCase("quinquagintaquadringentillionthss")]
        [TestCase("supercalifragilisticexpialidocious")]
        public void BaseDeploymentConfig_throws_error_for_invalid_tag(string invalidTag)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.Tags.Add(invalidTag);
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1, "Failed: did not throw exactly one error.");
            Assert.IsTrue(errors[0].Contains("Tag"));
            Assert.IsTrue(errors[0].Contains(DeploymentConfigErrorTypes.Invalid));
        }

        [Test]
        public void BaseDeploymentConfig_throws_N_errors_for_N_invalid_tags()
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();

            //5 invalid tags
            config.Tags.AddRange(new[]
            {
                "_invalidtag",
                "f",
                "gg",
                "quinquagintaquadringentillionthss",
                "supercalifragilisticexpialidocious"
            });

            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 5, "Failed: did not throw exactly five errors.");

            foreach (var error in errors)
            {
                Assert.IsTrue(error.Contains("Tag"));
                Assert.IsTrue(error.Contains("invalid"));
            }
        }
    }
}
