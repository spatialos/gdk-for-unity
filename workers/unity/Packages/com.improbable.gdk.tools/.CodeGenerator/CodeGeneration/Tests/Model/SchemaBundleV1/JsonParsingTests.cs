using System.IO;
using System.Reflection;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
using NUnit.Framework;

namespace Improbable.Gdk.CodeGeneration.Tests.Model.SchemaBundleV1
{
    [TestFixture]
    public class JsonParsingTests
    {
        public static string GetBundleContents()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return new StreamReader(assembly.GetManifestResourceStream(BundleResourceName)).ReadToEnd();
        }


        private const string BundleResourceName =
            "CodeGeneration.Tests.Model.SchemaBundleV1.Resources.exhaustive_bundle.json";

        [Test]
        public void ParsingSchemaBundleV1_does_not_throw()
        {
            Assert.DoesNotThrow(() => SchemaBundle.FromJson(GetBundleContents()));
        }
    }
}
