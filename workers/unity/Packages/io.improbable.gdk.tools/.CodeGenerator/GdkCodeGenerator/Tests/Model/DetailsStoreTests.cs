using System.Linq;
using Improbable.Gdk.CodeGeneration.Model.SchemaBundleV1;
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
            var bundle = JsonParsingTests.GetBundleContents();
            store = new DetailsStore(SchemaBundle.FromJson(bundle));
        }

        [Test]
        public void BlittableComponent_is_marked_as_blittable()
        {
            var identifier = CommonDetailsUtils.CreateIdentifier("improbable.gdk.tests.blittable_types.BlittableComponent");
            Assert.IsTrue(store.BlittableMap.Contains(identifier));

            foreach (var field in store.Components[identifier].FieldDetails)
            {
                Assert.IsTrue(field.IsBlittable);
            }
        }

        [Test]
        public void NonBlittableComponent_is_marked_as_nonblittable()
        {
            var identifier = CommonDetailsUtils.CreateIdentifier("improbable.gdk.tests.nonblittable_types.NonBlittableComponent");
            Assert.IsFalse(store.BlittableMap.Contains(identifier));
            Assert.IsTrue(store.Components[identifier].FieldDetails.Any(field => !field.IsBlittable));
        }

        [Test]
        public void GetNestedTypes_returns_direct_children_only()
        {
            var parentIdentifier = CommonDetailsUtils.CreateIdentifier("improbable.gdk.tests.TypeName");

            var nestedTypes = store.GetNestedTypes(parentIdentifier);

            Assert.AreEqual(1, nestedTypes.Count);
            Assert.IsTrue(nestedTypes.Contains(CommonDetailsUtils.CreateIdentifier("improbable.gdk.tests.TypeName.Other")));
        }

        [Test]
        public void GetNestedTypes_returns_child_enums_and_child_types()
        {
            var parentIdentifier =
                CommonDetailsUtils.CreateIdentifier("improbable.gdk.tests.TypeName.Other.NestedTypeName");

            var nestedTypes = store.GetNestedTypes(parentIdentifier);

            Assert.AreEqual(2, nestedTypes.Count);
            Assert.IsTrue(nestedTypes.Contains(CommonDetailsUtils.CreateIdentifier("improbable.gdk.tests.TypeName.Other.NestedTypeName.Other0")));
            Assert.IsTrue(nestedTypes.Contains(CommonDetailsUtils.CreateIdentifier("improbable.gdk.tests.TypeName.Other.NestedTypeName.NestedEnum")));
        }

        [Test]
        public void GetNestedTypes_returns_empty_set_if_no_children()
        {
            var identifier = CommonDetailsUtils.CreateIdentifier("improbable.gdk.tests.SomeType");
            var nestedTypes = store.GetNestedTypes(identifier);

            Assert.AreEqual(0, nestedTypes.Count);
        }
    }
}
