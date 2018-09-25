using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.BlittableTypes;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Ecs
{
    [TestFixture]
    public class BlittableComponentTests
    {
        private static readonly BlittableBool BlittableBoolFalse = false;
        private static readonly BlittableBool BlittableBoolTrue = true;

        private const int IntValue = 123;
        private const long LongValue = 5678L;
        private const float FloatValue = 1.2345f;
        private const double DoubleValue = 3.14159;
        private static readonly BlittableBool BoolValue = BlittableBoolTrue;

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

            Assert.AreEqual(BlittableBoolFalse, component.DirtyBit, "Dirty bit is initially false.");

            component.BoolField = true;
            Assert.AreEqual(BlittableBoolTrue, component.DirtyBit, "Dirty bit true after setting bool field.");

            component.DirtyBit = false;
            component.DoubleField = DoubleValue;
            Assert.AreEqual(BlittableBoolTrue, component.DirtyBit, "Dirty bit true after setting double field.");

            component.DirtyBit = false;
            component.FloatField = FloatValue;
            Assert.AreEqual(BlittableBoolTrue, component.DirtyBit, "Dirty bit true after setting float field.");

            component.DirtyBit = false;
            component.IntField = IntValue;
            Assert.AreEqual(BlittableBoolTrue, component.DirtyBit, "Dirty bit true after setting int field.");

            component.DirtyBit = false;
            component.LongField = LongValue;
            Assert.AreEqual(BlittableBoolTrue, component.DirtyBit, "Dirty bit true after setting long field.");
        }
    }
}
