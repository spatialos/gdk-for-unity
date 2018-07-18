using Generated.Improbable.Gdk.Tests.NonblittableTypes;
using Improbable.Gdk.Core;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.CodeGenerator.End2EndTests
{
    [TestFixture]
    public class NonBlittableComponentTests
    {
        private static readonly BlittableBool BlittableBoolFalse = false;
        private static readonly BlittableBool BlittableBoolTrue = true;

        private const int IntValue = 123;
        private const long LongValue = 5678L;
        private const float FloatValue = 1.2345f;
        private const double DoubleValue = 3.14159;
        private static readonly BlittableBool BoolValue = BlittableBoolTrue;
        private const string StringValue = "A String value";

        [Test]
        public void component_should_subclass_unityengine_component()
        {
            var component = new SpatialOSNonBlittableComponent();
            Assert.True(component is Component,
                "SpatialOSNonBlittableComponent is subclass of UnityEngine.Component");
        }

        [Test]
        public void component_should_implement_ISpatialComponentData()
        {
            var component = new SpatialOSNonBlittableComponent();
            Assert.True(component is ISpatialComponentData,
                "SpatialOSNonBlittableComponent implements ISpatialComponentData");
        }

        [Test]
        public void getters_should_return_set_values()
        {
            var component = new SpatialOSNonBlittableComponent();
            component.BoolField = true;
            component.DoubleField = DoubleValue;
            component.FloatField = FloatValue;
            component.IntField = IntValue;
            component.LongField = LongValue;
            component.StringField = StringValue;

            Assert.AreEqual(DoubleValue, component.DoubleField, 0.001, "Double Field");
            Assert.AreEqual(FloatValue, component.FloatField, 0.001, "Float Field");
            Assert.AreEqual(IntValue, component.IntField, "Int Field");
            Assert.AreEqual(LongValue, component.LongField, "Long Field");
            Assert.AreEqual(BoolValue, component.BoolField, "Bool Field");
            Assert.AreEqual(StringValue, component.StringField, "String Field");
        }

        [Test]
        public void setters_should_set_dirty_bit_to_true()
        {
            var component = new SpatialOSNonBlittableComponent();
            Assert.AreEqual(BlittableBoolFalse, component.DirtyBit, "Dirty bit is initially false.");

            component.BoolField = BoolValue;
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

            component.DirtyBit = false;
            component.StringField = StringValue;
            Assert.AreEqual(BlittableBoolTrue, component.DirtyBit, "Dirty bit true after setting string field.");
        }
    }
}
