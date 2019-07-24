using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration;
using Improbable.Gdk.CodeGeneration.Tests.Model.SchemaBundleV1;
using Improbable.Gdk.CodeGenerator;
using NUnit.Framework;

namespace GdkCodeGenerator.Tests.Model
{
    [TestFixture]
    public class DetailsStoreTests
    {
        private DetailsStore store;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var json = JsonParsingTests.GetBundleContents();
            var overrides = new List<string>
            {
                "global::Improbable.Gdk.Tests.SomeType;global::UserCode.SerializationExtensions.Type"
            };

            store = new DetailsStore(SchemaBundle.LoadBundle(json), overrides);
        }

        [Test]
        public void BlittableComponent_is_marked_as_blittable()
        {
            var fqn = "improbable.gdk.tests.blittable_types.BlittableComponent";
            Assert.IsTrue(store.BlittableSet.Contains(fqn));

            foreach (var field in store.Components[fqn].FieldDetails)
            {
                Assert.IsTrue(field.IsBlittable);
            }
        }

        [Test]
        public void NonBlittableComponent_is_marked_as_nonblittable()
        {
            var fqn = "improbable.gdk.tests.nonblittable_types.NonBlittableComponent";
            Assert.IsFalse(store.BlittableSet.Contains(fqn));
        }

        [Test]
        public void GetNestedTypes_returns_direct_children_only()
        {
            var parentFqn = "improbable.gdk.tests.TypeName";

            var nestedTypes = store.GetNestedTypes(parentFqn);

            Assert.AreEqual(1, nestedTypes.Count);
            Assert.IsTrue(nestedTypes.Contains("improbable.gdk.tests.TypeName.Other"));
        }

        [Test]
        public void GetNestedTypes_returns_child_enums_and_child_types()
        {
            var parentFqn =
                "improbable.gdk.tests.TypeName.Other.NestedTypeName";

            var nestedTypes = store.GetNestedTypes(parentFqn);

            Assert.AreEqual(2, nestedTypes.Count);
            Assert.IsTrue(nestedTypes.Contains("improbable.gdk.tests.TypeName.Other.NestedTypeName.Other0"));
            Assert.IsTrue(nestedTypes.Contains("improbable.gdk.tests.TypeName.Other.NestedTypeName.NestedEnum"));
        }

        [Test]
        public void GetNestedTypes_returns_empty_set_if_no_children()
        {
            var fqn = "improbable.gdk.tests.SomeType";
            var nestedTypes = store.GetNestedTypes(fqn);

            Assert.AreEqual(0, nestedTypes.Count);
        }

        [Test]
        public void Serialization_overrides_are_correctly_propagated()
        {
            var fqn = "improbable.gdk.tests.SomeType";
            var details = store.Types[fqn];

            Assert.IsTrue(details.HasSerializationOverride);
        }
    }
}
