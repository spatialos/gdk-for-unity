using System.IO;
using System.Reflection;
using Improbable.Gdk.CodeGeneration.Model;
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
            "CodeGenerationLib.Tests.Model.SchemaBundleV1.Resources.exhaustive_bundle.json";

        [Test]
        public void ParsingSchemaBundle_does_not_throw()
        {
            Assert.DoesNotThrow(() => SchemaBundle.LoadBundle(GetBundleContents()));
        }
    }
}
