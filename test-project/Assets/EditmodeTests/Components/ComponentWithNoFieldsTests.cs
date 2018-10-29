using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.ComponentsWithNoFields;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Ecs
{
    [TestFixture]
    public class ComponentWithNoFieldsTests
    {
        [Test]
        public void IsDataDirty_returns_false_for_component_with_no_fields()
        {
            var component = new ComponentWithNoFields.Component();
            Assert.IsFalse(component.IsDataDirty());
        }

        [Test]
        public void IsDataDirty_with_propertyIndex_returns_false_for_component_with_no_fields()
        {
            var component = new ComponentWithNoFields.Component();
            Assert.IsFalse(component.IsDataDirty(0));
            Assert.IsFalse(component.IsDataDirty(100));
        }
    }
}
