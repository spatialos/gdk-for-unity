using System.Linq;
using Improbable.Gdk.DeploymentLauncher.EditmodeTests.Utils;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.DeploymentLauncher.EditmodeTests
{
    [TestFixture]
    public class DeploymentConfigTests
    {
        [Test]
        public void DeploymentConfig_does_not_throw_error_when_valid()
        {
            Assert.IsFalse(DeploymentConfigTestUtils.GetValidDeploymentConfig().GetErrors().Any());
        }

        [TestCase("gdk_test_assembly")]
        public void DeploymentConfig_does_not_throw_error_for_valid_assembly_name(string assemblyName)
        {
            var config = DeploymentConfigTestUtils.GetValidDeploymentConfig();
            config.AssemblyName = assemblyName;

            Assert.IsFalse(config.GetErrors().Any());
        }

        [TestCase(null, DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("", DeploymentConfigErrorTypes.NullOrEmpty)]
        [TestCase("gdk", DeploymentConfigErrorTypes.Invalid)]
        [TestCase("siebenhundertsiebenundsiebzigtausendsiebenhundertsiebenundsiebzig", DeploymentConfigErrorTypes.Invalid)]
        public void DeploymentConfig_throws_error_for_invalid_assembly_name(string assemblyName, string errorType)
        {
            var config = DeploymentConfigTestUtils.GetValidDeploymentConfig();
            config.AssemblyName = assemblyName;
            var errors = config.GetErrors().DeplErrors.Values.SelectMany(err => err).ToArray();

            Assert.AreEqual(errors.Length, 1);
            Assert.IsTrue(errors[0].Contains("Assembly Name"));
            Assert.IsTrue(errors[0].Contains(errorType));
        }
    }
}
