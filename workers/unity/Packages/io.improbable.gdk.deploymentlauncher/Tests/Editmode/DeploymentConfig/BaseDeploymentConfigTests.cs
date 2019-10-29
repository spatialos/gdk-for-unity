using System.Linq;
using Improbable.Gdk.DeploymentLauncher.EditmodeTests.Utils;
using NUnit.Framework;

namespace Improbable.Gdk.DeploymentLauncher.EditmodeTests
{
    [TestFixture]
    public class BaseDeploymentConfigTests
    {
        [Test]
        public void Valid_config_gives_no_errors()
        {
            Assert.IsEmpty(DeploymentConfigTestUtils.GetValidBaseConfig().GetErrors());
        }

        [TestCase("demo_dpl_final")]
        [TestCase("demo_dpl_final2")]
        public void Valid_deployment_name_test(string deploymentName)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.Name = deploymentName;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("#badname!", DeploymentConfigErrorTypes.Invalid)]
        public void Invalid_deployment_name_test(string invalidName, string errorType)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.Name = invalidName;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors[0].Contains("Deployment Name"));
            Assert.IsTrue(errors[0].Contains(errorType));
        }

        [TestCase("default_launch.json")]
        [TestCase("cloud_launch.json")]
        public void Valid_launch_json_path_test(string launchJson)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.LaunchJson = launchJson;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("this/launch/config/does/not/exist.json", DeploymentConfigErrorTypes.NotFound)]
        public void Invalid_launch_json_test(string launchJson, string errorType)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.LaunchJson = launchJson;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors[0].Contains("Launch Config"));
            Assert.IsTrue(errors[0].Contains(errorType));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("snapshots/default.snapshot")]
        public void Valid_snapshot_path_test(string snapshotPath)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.SnapshotPath = snapshotPath;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase("this/snapshot/path/does/not/exist.snapshot")]
        public void Invalid_snapshot_path_test(string snapshotPath)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.SnapshotPath = snapshotPath;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors[0].Contains("Snapshot"));
            Assert.IsTrue(errors[0].Contains(DeploymentConfigErrorTypes.NotFound));
        }

        [TestCase("dev_login")]
        [TestCase("quinquagintaquadringentillionths")]
        public void Valid_tag_test(string validTag)
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
        public void Invalid_tag_test(string invalidTag)
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();
            config.Tags.Add(invalidTag);
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors[0].Contains("Tag"));
            Assert.IsTrue(errors[0].Contains(DeploymentConfigErrorTypes.Invalid));
        }

        [Test]
        public void N_invalid_tags_gives_n_errors()
        {
            var config = DeploymentConfigTestUtils.GetValidBaseConfig();

            //5 invalid tags
            config.Tags.Add("_invalidtag");
            config.Tags.Add("f");
            config.Tags.Add("gg");
            config.Tags.Add("quinquagintaquadringentillionthss");
            config.Tags.Add("supercalifragilisticexpialidocious");

            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 5);

            foreach (var error in errors)
            {
                Assert.IsTrue(error.Contains("Tag"));
                Assert.IsTrue(error.Contains("invalid"));
            }
        }
    }
}
