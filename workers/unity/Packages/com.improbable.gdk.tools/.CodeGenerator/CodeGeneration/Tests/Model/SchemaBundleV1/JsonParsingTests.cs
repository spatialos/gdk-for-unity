using System.IO;
using System.Reflection;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using NUnit.Framework;

namespace Improbable.Gdk.CodeGeneration.Tests.Model.SchemaBundleV1
{
    [TestFixture]
    public class JsonParsingTests
    {
        private const string BundleResourceName =
            "CodeGeneration.Tests.Model.SchemaBundleV1.Resources.exhaustive_bundle.json";

        private string bundleContents;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var assembly = Assembly.GetExecutingAssembly();
            bundleContents = new StreamReader(assembly.GetManifestResourceStream(BundleResourceName)).ReadToEnd();
        }

        [Test]
        public void ParsingSchemaBundleV1_does_not_throw()
        {
            Assert.DoesNotThrow(() => SchemaBundle.FromJson(bundleContents));
        }
    }
}
