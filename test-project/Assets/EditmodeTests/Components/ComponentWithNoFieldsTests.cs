using System;
using Improbable.Gdk.Test;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Ecs
{
    [TestFixture]
    public class ComponentWithNoFieldsTests
    {
        [Test]
        public void IsDataDirty_returns_false_for_component_with_no_fields()
        {
            var component = new Empty.Component();
            Assert.IsFalse(component.IsDataDirty());
        }

        [Test]
        public void IsDataDirty_with_propertyIndex_throws_for_component_with_no_fields()
        {
            var component = new Empty.Component();
            Assert.Throws<InvalidOperationException>(() =>
            {
                component.IsDataDirty(0);
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                component.IsDataDirty(100);
            });
        }

        [Test]
        public void MarkDataDirty_throws_for_component_with_no_fields()
        {
            var component = new Empty.Component();
            Assert.Throws<InvalidOperationException>(() =>
            {
                component.IsDataDirty(0);
            });
            Assert.Throws<InvalidOperationException>(() =>
            {
                component.IsDataDirty(100);
            });
        }
    }
}
