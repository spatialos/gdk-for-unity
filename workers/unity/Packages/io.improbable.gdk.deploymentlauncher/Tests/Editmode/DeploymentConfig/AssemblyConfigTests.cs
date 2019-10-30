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
        public void AssemblyConfig_does_not_throw_error_when_valid()
        {
            Assert.IsNull(GetValidConfig().GetError());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("gdk", DeploymentConfigErrorTypes.Invalid)]
        [TestCase("siebenhundertsiebenundsiebzigtausendsiebenhundertsiebenundsiebzig", DeploymentConfigErrorTypes.Invalid)]
        public void AssemblyConfig_throws_error_for_invalid_assembly_name(string assemblyName, string errorType)
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
