using System;
using System.Linq;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests
{
    [TestFixture]
    public class ModularCodegenTests
    {
        // This checks that the codegen module was correctly picked up.
        [Test]
        public void Improbable_Gdk_ModularCodegenTests_Test_exists()
        {
            // Assembly.FullName == "<name> <version> <culture> <publickeytoken>", so we just .Contains() it.
            var generatedCodeAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .First(assembly => assembly.FullName.Contains("Improbable.Gdk.Generated"));

            Assert.IsTrue(generatedCodeAssembly.GetTypes().Any(type => type.FullName == "Improbable.Gdk.ModularCodegenTests.Test"));
        }
    }
}
