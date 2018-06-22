using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generated.Improbable.TestSchema;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Unity.Mathematics;

namespace End2End
{
    [TestFixture]
    public class BlittableComponentTests
    {
        private static readonly bool1 Bool1False = new bool1(false);
        private static readonly bool1 Bool1True = new bool1(true);

        private const int IntValue = 123;
        private const long LongValue = 5678L;
        private const float FloatValue = 1.2345f;
        private const double DoubleValue = 3.14159;
        private static readonly bool1 BoolValue = Bool1True;

        [Test]
        public void TestFieldGetters()
        {
            var component = new SpatialOSBlittableComponent
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
        public void TestFieldSettersAndDirtyBit()
        {
            var component = new SpatialOSBlittableComponent();

            Assert.AreEqual(Bool1False, component.DirtyBit, "Dirty bit is initially false.");

            component.BoolField = true;
            Assert.AreEqual(Bool1True, component.DirtyBit, "Dirty bit true after setting bool field.");

            component.DirtyBit = false;
            component.DoubleField = DoubleValue;
            Assert.AreEqual(Bool1True, component.DirtyBit, "Dirty bit true after setting double field.");

            component.DirtyBit = false;
            component.FloatField = FloatValue;
            Assert.AreEqual(Bool1True, component.DirtyBit, "Dirty bit true after setting float field.");

            component.DirtyBit = false;
            component.IntField = IntValue;
            Assert.AreEqual(Bool1True, component.DirtyBit, "Dirty bit true after setting int field.");

            component.DirtyBit = false;
            component.LongField = LongValue;
            Assert.AreEqual(Bool1False, component.DirtyBit, "Dirty bit true after setting long field.");
        }
    }
}
