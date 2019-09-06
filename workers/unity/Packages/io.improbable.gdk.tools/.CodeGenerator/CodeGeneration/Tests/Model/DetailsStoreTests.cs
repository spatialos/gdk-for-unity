using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.Tests.Model.SchemaBundleV1;
using NUnit.Framework;

namespace Improbable.Gdk.CodeGeneration.Tests.Model
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
                "global::Improbable.TestSchema.SomeType;global::UserCode.SerializationExtensions.Type"
            };

            store = new DetailsStore(SchemaBundle.LoadBundle(json), overrides);
        }

        [Test]
        public void BlittableComponent_is_marked_as_blittable()
        {
            var fqn = "improbable.gdk.test.BlittableComponent";
            Assert.IsTrue(store.BlittableSet.Contains(fqn));

            foreach (var field in store.Components[fqn].FieldDetails)
            {
                Assert.IsTrue(field.IsBlittable);
            }
        }

        [Test]
        public void NonBlittableComponent_is_marked_as_nonblittable()
        {
            var fqn = "improbable.gdk.test.NonBlittableComponent";
            Assert.IsFalse(store.BlittableSet.Contains(fqn));
        }

        [Test]
        public void GetNestedTypes_returns_direct_children_only()
        {
            var parentFqn = "improbable.test_schema.NestedTypeSameName";

            var nestedTypes = store.GetNestedTypes(parentFqn);

            Assert.AreEqual(1, nestedTypes.Count);
            Assert.IsTrue(nestedTypes.Contains("improbable.test_schema.NestedTypeSameName.Other"));
        }

        [Test]
        public void GetNestedTypes_returns_child_enums_and_child_types()
        {
            var parentFqn =
                "improbable.test_schema.NestedTypeSameName.Other.NestedTypeSameName";

            var nestedTypes = store.GetNestedTypes(parentFqn);

            Assert.AreEqual(2, nestedTypes.Count);
            Assert.IsTrue(nestedTypes.Contains("improbable.test_schema.NestedTypeSameName.Other.NestedTypeSameName.Other0"));
            Assert.IsTrue(nestedTypes.Contains("improbable.test_schema.NestedTypeSameName.Other.NestedTypeSameName.Other1"));
        }

        [Test]
        public void GetNestedTypes_returns_empty_set_if_no_children()
        {
            var fqn = "improbable.test_schema.SomeType";
            var nestedTypes = store.GetNestedTypes(fqn);

            Assert.AreEqual(0, nestedTypes.Count);
        }

        [Test]
        public void Serialization_overrides_are_correctly_propagated()
        {
            var fqn = "improbable.test_schema.SomeType";
            var details = store.Types[fqn];

            Assert.IsTrue(details.HasSerializationOverride);
        }

        [TestCase("TypeA")]
        [TestCase("TypeB")]
        [TestCase("TypeC")]
        public void No_recursive_options_allowed(string schemaType)
        {
            var fqn = $"improbable.test_schema.{schemaType}";

            var type = store.Types[fqn];

            // The third one has been stripped (recursive optional)
            Assert.AreEqual(2, type.FieldDetails.Count);
        }

        [Test]
        public void Non_recursive_options_are_left()
        {
            var fqn = "improbable.test_schema.ExhaustiveOptionalData";
            var type = store.Types[fqn];

            Assert.AreEqual(18, type.FieldDetails.Count);
        }
    }
}
