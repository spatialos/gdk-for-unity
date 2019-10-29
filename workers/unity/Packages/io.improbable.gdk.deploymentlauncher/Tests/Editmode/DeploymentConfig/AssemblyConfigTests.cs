using Improbable.Gdk.DeploymentLauncher.EditmodeTests.Utils;
using NUnit.Framework;

namespace Improbable.Gdk.DeploymentLauncher.EditmodeTests
{
    [TestFixture]
    public class AssemblyConfigTests
    {
        private AssemblyConfig GetValidConfig()
        {
            return new AssemblyConfig
            {
                ProjectName = DeploymentConfigTestUtils.ValidProjectName,
                AssemblyName = DeploymentConfigTestUtils.ValidAssemblyName
            };
        }

        [Test]
        public void Valid_config_gives_no_errors()
        {
            Assert.IsNull(GetValidConfig().GetError());
        }

        [TestCase("gdk_test_assembly")]
        public void Valid_assembly_name_test(string assemblyName)
        {
            var config = GetValidConfig();
            config.AssemblyName = assemblyName;

            Assert.IsNull(config.GetError());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("gdk", DeploymentConfigErrorTypes.Invalid)]
        [TestCase("siebenhundertsiebenundsiebzigtausendsiebenhundertsiebenundsiebzig", DeploymentConfigErrorTypes.Invalid)]
        public void Invalid_assembly_name_test(string assemblyName, string errorType)
        {
            var config = GetValidConfig();
            config.AssemblyName = assemblyName;
            var error = config.GetError();

            Assert.IsNotNull(error);
            Assert.IsTrue(error.Contains("Assembly Name"));
            Assert.IsTrue(error.Contains(errorType));
        }
    }
}
