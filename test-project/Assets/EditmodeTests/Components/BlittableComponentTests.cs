using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Ecs
{
    [TestFixture]
    public class BlittableComponentTests
    {
        private const int IntValue = 123;
        private const long LongValue = 5678L;
        private const float FloatValue = 1.2345f;
        private const double DoubleValue = 3.14159;
        private const bool BoolValue = true;

        [Test]
        public void component_should_implement_ISpatialComponentData()
        {
            var component = new BlittableComponent.Component();
            Assert.IsInstanceOf<ISpatialComponentData>(component,
                "BlittableComponent.Component implements ISpatialComponentData");
        }

        [Test]
        public void getters_should_return_values_set_in_constructor()
        {
            var component = new BlittableComponent.Component
            {
                BoolField = true,
                DoubleField = DoubleValue,
                FloatField = FloatValue,
                IntField = IntValue,
                LongField = LongValue
            };

            Assert.AreEqual(DoubleValue, component.DoubleField, 0.001, "Double Field");
            Assert.AreEqual(FloatValue, component.FloatField, 0.001, "Float Field");
            Assert.AreEqual(IntValue, component.IntField, "Int Field");
            Assert.AreEqual(LongValue, component.LongField, "Long Field");
            Assert.AreEqual(BoolValue, component.BoolField, "Bool Field");
        }

        [Test]
        public void setters_should_set_dirty_bit_to_true()
        {
            var component = new BlittableComponent.Component();

            Assert.AreEqual(false, component.IsDataDirty(), "Component is not dirty initially.");

            component.BoolField = true;
            Assert.AreEqual(true, component.IsDataDirty(), "Component is dirty after setting bool field.");
            Assert.AreEqual(true, component.IsDataDirty(0), "Component property 0 is dirty after setting bool field.");

            component.MarkDataClean();
            component.IntField = IntValue;
            Assert.AreEqual(true, component.IsDataDirty(), "Component is dirty after setting int field.");
            Assert.AreEqual(true, component.IsDataDirty(1), "Component property 1 is dirty after setting int field.");

            component.MarkDataClean();
            component.LongField = LongValue;
            Assert.AreEqual(true, component.IsDataDirty(), "Component is dirty after setting long field.");
            Assert.AreEqual(true, component.IsDataDirty(2), "Component property 2 is dirty after setting long field.");

            component.MarkDataClean();
            component.FloatField = FloatValue;
            Assert.AreEqual(true, component.IsDataDirty(), "Component is dirty after setting float field.");
            Assert.AreEqual(true, component.IsDataDirty(3), "Component property 3 is dirty after setting float field.");

            component.MarkDataClean();
            component.DoubleField = DoubleValue;
            Assert.AreEqual(true, component.IsDataDirty(), "Component is dirty after setting double field.");
            Assert.AreEqual(true, component.IsDataDirty(4), "Component property 4 is dirty after setting double field.");
        }
    }
}
