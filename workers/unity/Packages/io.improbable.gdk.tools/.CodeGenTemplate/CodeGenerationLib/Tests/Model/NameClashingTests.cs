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
            var qualifiedName = "improbable.gdk.test.IllegalComponentCommand";

            var store = GetDetailsFromBundle(bundleName);

            Assert.IsTrue(store.Components.ContainsKey(qualifiedName));
            Assert.AreEqual(0, store.Components[qualifiedName].CommandDetails.Count);
        }

        [Test]
        public void Clashing_event_in_component()
        {
            var bundleName = "clash_event_in_component";
            var qualifiedName = "improbable.gdk.test.IllegalComponentEvent";

            var store = GetDetailsFromBundle(bundleName);

            Assert.IsTrue(store.Components.ContainsKey(qualifiedName));
            Assert.AreEqual(0, store.Components[qualifiedName].EventDetails.Count);
        }

        [Test]
        public void Clashing_enum_in_type()
        {
            var bundleName = "clash_enum_in_type";
            var qualifiedName = "improbable.gdk.test.IllegalType";

            var store = GetDetailsFromBundle(bundleName);

            Assert.IsTrue(store.Types.ContainsKey(qualifiedName));
            Assert.IsTrue(store.Enums.ContainsKey($"{qualifiedName}.NestedEnum"));
            Assert.AreEqual(1, store.Types[qualifiedName].ChildEnums.Count);
            Assert.AreEqual(0, store.Types[qualifiedName].FieldDetails.Count);
        }

        [Test]
        public void Clashing_type_in_type()
        {
            var bundleName = "clash_type_in_type";
            var qualifiedName = "improbable.gdk.test.IllegalType";

            var store = GetDetailsFromBundle(bundleName);

            Assert.IsTrue(store.Types.ContainsKey(qualifiedName));
            Assert.IsTrue(store.Types.ContainsKey($"{qualifiedName}.NestedType"));
            Assert.AreEqual(1, store.Types[qualifiedName].ChildTypes.Count);
            Assert.AreEqual(0, store.Types[qualifiedName].FieldDetails.Count);
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
