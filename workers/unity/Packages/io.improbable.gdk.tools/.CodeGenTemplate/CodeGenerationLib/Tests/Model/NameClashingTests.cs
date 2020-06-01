using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Improbable.Gdk.CodeGeneration.Model;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NUnit.Framework;

namespace Improbable.Gdk.CodeGeneration.Tests.Model
{
    [TestFixture]
    public class NameClashingTests
    {
        [Test]
        public void Clashing_command_in_component()
        {
            var bundleName = "clash_command_in_component";
            var qualifiedName = "improbable.gdk.test.IllegalComponent";

            var store = GetDetailsFromBundle(bundleName);

            Assert.AreEqual(store.Components.Count, 1);
            Assert.IsTrue(store.Components.ContainsKey(qualifiedName));
            Assert.IsFalse(store.Components[qualifiedName].CommandDetails.Count == 0);
        }

        [Test]
        public void Clashing_event_in_component()
        {
            var bundleName = "clash_event_in_component";
            var qualifiedName = "improbable.gdk.test.IllegalComponent";

            var store = GetDetailsFromBundle(bundleName);

            Assert.AreEqual(store.Components.Count, 1);
            Assert.IsTrue(store.Components.ContainsKey(qualifiedName));
            Assert.IsFalse(store.Components[qualifiedName].EventDetails.Count == 0);
        }

        [Test]
        public void Clashing_enum_in_type()
        {
            var bundleName = "clash_enum_in_type";
            var qualifiedName = "improbable.gdk.test.IllegalType";

            var store = GetDetailsFromBundle(bundleName);

            Assert.AreEqual(store.Types.Count, 1);
            Assert.IsTrue(store.Types.ContainsKey(qualifiedName));
            Assert.IsTrue(store.Types[qualifiedName].ChildEnums.Count == 1);
            Assert.IsTrue(store.Types[qualifiedName].FieldDetails.Count == 0);
        }

        [Test]
        public void Clashing_type_in_type()
        {
            var bundleName = "clash_type_in_type";
            var qualifiedName = "improbable.gdk.test.IllegalType";

            var store = GetDetailsFromBundle(bundleName);

            Assert.AreEqual(store.Types.Count, 2);
            Assert.IsTrue(store.Types.ContainsKey(qualifiedName));
            Assert.IsTrue(store.Types.ContainsKey($"{qualifiedName}.NestedType"));
            Assert.IsTrue(store.Types[qualifiedName].ChildTypes.Count == 1);
            Assert.IsTrue(store.Types[qualifiedName].FieldDetails.Count == 0);
        }

        private static DetailsStore GetDetailsFromBundle(string bundleName)
        {
            var bundleResourceName =
                $"CodeGenerationLib.Tests.Model.SchemaBundleV1.Resources.{bundleName}.json";

            var assembly = Assembly.GetExecutingAssembly();
            var resource = assembly.GetManifestResourceStream(bundleResourceName);
            var json = new StreamReader(resource).ReadToEnd();

            return new DetailsStore(SchemaBundle.LoadBundle(json), new List<string>(), null);
        }
    }
}
