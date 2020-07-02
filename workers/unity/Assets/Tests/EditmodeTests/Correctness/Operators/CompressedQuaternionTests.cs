using Improbable.Gdk.TransformSynchronization;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Operators
{
    public class CompressedQuaternionTests
    {
        [Test]
        public void Equals_operator_true_for_identical_CompressedQuaternion()
        {
            var q1 = Quaternion.Euler(10f, 20f, 30f).ToCompressedQuaternion();
            var q2 = Quaternion.Euler(10f, 20f, 30f).ToCompressedQuaternion();

            Assert.IsTrue(q1 == q2);
        }

        [Test]
        public void Equals_operator_false_for_different_CompressedQuaternion()
        {
            var q1 = Quaternion.Euler(10f, 20f, 30f).ToCompressedQuaternion();
            var q2 = Quaternion.Euler(40f, 50f, 60f).ToCompressedQuaternion();

            Assert.IsFalse(q1 == q2);
        }

        [Test]
        public void Not_equals_operator_true_for_different_CompressedQuaternion()
        {
            var q1 = Quaternion.Euler(10f, 20f, 30f).ToCompressedQuaternion();
            var q2 = Quaternion.Euler(40f, 50f, 60f).ToCompressedQuaternion();

            Assert.IsTrue(q1 != q2);
        }

        [Test]
        public void Not_equals_operator_false_for_identical_CompressedQuaternion()
        {
            var q1 = Quaternion.Euler(10f, 20f, 30f).ToCompressedQuaternion();
            var q2 = Quaternion.Euler(10f, 20f, 30f).ToCompressedQuaternion();

            Assert.IsFalse(q1 != q2);
        }
    }
}
