using System.Linq;
using Improbable.Gdk.DeploymentLauncher.EditmodeTests.Utils;
using NUnit.Framework;

namespace Improbable.Gdk.DeploymentLauncher.EditmodeTests
{
    [TestFixture]
    public class SimulatedPlayerDeploymentConfigTests
    {
        [Test]
        public void Valid_config_gives_no_errors()
        {
            Assert.IsEmpty(DeploymentConfigTestUtils.GetValidSimPlayerConfig().GetErrors());
        }

        [TestCase("test_worker_flag")]
        public void Valid_flag_prefix_test(string flagPrefix)
        {
            var config = DeploymentConfigTestUtils.GetValidSimPlayerConfig();
            config.FlagPrefix = flagPrefix;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("not.good.", DeploymentConfigErrorTypes.FullStops)]
        [TestCase("ha ha ha", DeploymentConfigErrorTypes.Spaces)]
        public void Invalid_flag_prefix_test(string flagPrefix, string errorType)
        {
            var config = DeploymentConfigTestUtils.GetValidSimPlayerConfig();
            config.FlagPrefix = flagPrefix;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors[0].Contains("Flag Prefix"));
            Assert.IsTrue(errors[0].Contains(errorType));
        }

        [TestCase("TestSimPlayerCoordinator")]
        public void Valid_worker_type_test(string workerType)
        {
            var config = DeploymentConfigTestUtils.GetValidSimPlayerConfig();
            config.WorkerType = workerType;

            Assert.IsEmpty(config.GetErrors());
        }

        [TestCase(null)]
        [TestCase("")]
        public void Invalid_worker_type_test(string workerType)
        {
            var config = DeploymentConfigTestUtils.GetValidSimPlayerConfig();
            config.WorkerType = workerType;
            var errors = config.GetErrors().ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors[0].Contains("Worker Type"));
            Assert.IsTrue(errors[0].Contains(DeploymentConfigErrorTypes.NullOrEmpty));
        }
    }
}
