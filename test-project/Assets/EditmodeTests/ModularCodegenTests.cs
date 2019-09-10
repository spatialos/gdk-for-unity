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
            // Assembly.FullName == "<name> <version> <culture> <publickeytoken>", so we just .Contains() it.
            generatedCodeAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .First(assembly => assembly.FullName.Contains("Improbable.Gdk.Generated"));
        }

        // This checks that the codegen module was correctly picked up.
        [Test]
        public void Improbable_Gdk_ModularCodegenTests_Test_exists()
        {
            Assert.IsTrue(generatedCodeAssembly.GetTypes().Any(type => type.FullName == "Improbable.Gdk.ModularCodegenTests.Test"));
        }

        [Test]
        public void Improbable_Tests_ModularTarget_has_partial_injected()
        {
            var modularTargetType = generatedCodeAssembly.GetTypes()
                .First(type => type.FullName == "Improbable.Tests.ModularTarget");

            modularTargetType.GetNestedTypes().Any(type => type.Name == "InteriorClass");
        }
    }
}
