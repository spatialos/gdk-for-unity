using Improbable.Gdk.TransformSynchronization;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Operators
{
    [TestFixture]
    public class FixedPointVector3Tests
    {
        [Test]
        public void Equals_operator_true_for_identical_FixedPointVector3()
        {
            var f1 = new Vector3(10f, 20f, 30f).ToFixedPointVector3();
            var f2 = new Vector3(10f, 20f, 30f).ToFixedPointVector3();

            Assert.IsTrue(f1 == f2);
        }

        [Test]
        public void Equals_operator_false_for_different_FixedPointVector3()
        {
            var f1 = new Vector3(10f, 20f, 30f).ToFixedPointVector3();
            var f2 = new Vector3(40f, 50f, 60f).ToFixedPointVector3();

            Assert.IsFalse(f1 == f2);
        }

        [Test]
        public void Not_equals_operator_true_for_different_FixedPointVector3()
        {
            var f1 = new Vector3(10f, 20f, 30f).ToFixedPointVector3();
            var f2 = new Vector3(40f, 50f, 60f).ToFixedPointVector3();

            Assert.IsTrue(f1 != f2);
        }

        [Test]
        public void Not_equals_operator_false_for_identical_FixedPointVector3()
        {
            var f1 = new Vector3(10f, 20f, 30f).ToFixedPointVector3();
            var f2 = new Vector3(10f, 20f, 30f).ToFixedPointVector3();

            Assert.IsFalse(f1 != f2);
        }
    }
}
