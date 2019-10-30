using System.Linq;
using Improbable.Gdk.DeploymentLauncher.EditmodeTests.Utils;
using NUnit.Framework;

namespace Improbable.Gdk.DeploymentLauncher.EditmodeTests
{
    [TestFixture]
    public class SimulatedPlayerDeploymentConfigTests
    {
        [Test]
        public void SimulatedPlayerDeploymentConfig_does_not_throw_error_when_valid()
        {
            Assert.IsEmpty(DeploymentConfigTestUtils.GetValidSimPlayerConfig().GetErrors());
        }

        [TestCase("test_worker_flag")]
        public void SimulatedPlayerDeploymentConfig_does_not_throw_error_for_valid_flag_prefix(string flagPrefix)
        {
            var config = DeploymentConfigTestUtils.GetValidSimPlayerConfig();
            config.FlagPrefix = flagPrefix;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("not.good.", DeploymentConfigErrorTypes.FullStops)]
        [TestCase("ha ha ha", DeploymentConfigErrorTypes.Spaces)]
        public void SimulatedPlayerDeploymentConfig_throws_error_for_invalid_flag_prefix(string flagPrefix, string errorType)
        {
            var config = DeploymentConfigTestUtils.GetValidSimPlayerConfig();
            config.FlagPrefix = flagPrefix;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1, "Failed: did not throw exactly one error.");
            Assert.IsTrue(errors[0].Contains("Flag Prefix"));
            Assert.IsTrue(errors[0].Contains(errorType));
        }

        [TestCase("TestSimPlayerCoordinator")]
        public void SimulatedPlayerDeploymentConfig_does_not_throw_error_for_valid_worker_type(string workerType)
        {
            var config = DeploymentConfigTestUtils.GetValidSimPlayerConfig();
            config.WorkerType = workerType;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase(null)]
        [TestCase("")]
        public void SimulatedPlayerDeploymentConfig_throws_error_for_invalid_worker_type(string workerType)
        {
            var config = DeploymentConfigTestUtils.GetValidSimPlayerConfig();
            config.WorkerType = workerType;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1, "Failed: did not throw exactly one error.");
            Assert.IsTrue(errors[0].Contains("Worker Type"));
            Assert.IsTrue(errors[0].Contains(DeploymentConfigErrorTypes.NullOrEmpty));
        }
    }
}
