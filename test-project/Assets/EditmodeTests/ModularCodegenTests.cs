using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests
{
    [TestFixture]
    public class ModularCodegenTests
    {
        private Assembly generatedCodeAssembly;

        [OneTimeSetUp]
        public void Setup()
        {
            generatedCodeAssembly = typeof(Position).Assembly;
        }

        // This checks that the codegen module was correctly picked up.
        [Test]
        public void Improbable_Gdk_ModularCodegenTests_Test_exists()
        {
            Assert.IsNotNull(generatedCodeAssembly.GetType("Improbable.Gdk.ModularCodegenTests.Test"));
        }

        // This checks that the templates are correctly compiled in
        [Test]
        public void Improbable_Gdk_ModularCodegenTests_TemplateTest_exists()
        {
            Assert.IsNotNull(generatedCodeAssembly.GetType("Improbable.Gdk.ModularCodegenTests.TemplateTest"));
        }


        [Test]
        public void Improbable_Tests_ModularTarget_has_partial_injected()
        {
            var modularTargetType = generatedCodeAssembly.GetType("Improbable.Tests.ModularTarget");

            Assert.IsNotNull(modularTargetType.GetNestedType("InteriorClass"));
        }
    }
}
