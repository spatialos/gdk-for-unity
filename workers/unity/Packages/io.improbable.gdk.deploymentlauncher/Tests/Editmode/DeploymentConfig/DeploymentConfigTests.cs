using System.Linq;
using Improbable.Gdk.DeploymentLauncher.EditmodeTests.Utils;
using NUnit.Framework;

namespace Improbable.Gdk.DeploymentLauncher.EditmodeTests
{
    [TestFixture]
    public class DeploymentConfigTests
    {
        [Test]
        public void Valid_config_gives_no_errors()
        {
            Assert.IsNull(DeploymentConfigTestUtils.GetValidDeploymentConfig().GetErrors());
        }

        [TestCase("gdk_test_assembly")]
        public void Valid_assembly_name_test(string assemblyName)
        {
            var config = DeploymentConfigTestUtils.GetValidDeploymentConfig();
            config.AssemblyName = assemblyName;

            Assert.IsNull(config.GetErrors());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("gdk", DeploymentConfigErrorTypes.Invalid)]
        [TestCase("siebenhundertsiebenundsiebzigtausendsiebenhundertsiebenundsiebzig", DeploymentConfigErrorTypes.Invalid)]
        public void Invalid_assembly_name_test(string assemblyName, string errorType)
        {
            var config = DeploymentConfigTestUtils.GetValidDeploymentConfig();
            config.AssemblyName = assemblyName;
            var errors = config.GetErrors().DeplErrors.Values.ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors[0].Contains("Assembly Name"));
            Assert.IsTrue(errors[0].Contains(DeploymentConfigErrorTypes.NullOrEmpty));
        }
    }
}
