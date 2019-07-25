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
            var q1 = Quaternion.Euler(10, 20, 30).ToCompressedQuaternion();
            var q2 = Quaternion.Euler(10, 20, 30).ToCompressedQuaternion();

            Assert.IsTrue(q1 == q2);
        }

        [Test]
        public void Equals_operator_false_for_different_CompressedQuaternion()
        {
            var q1 = Quaternion.Euler(10, 20, 30).ToCompressedQuaternion();
            var q2 = Quaternion.Euler(40, 50, 60).ToCompressedQuaternion();

            Assert.IsFalse(q1 == q2);
        }

        [Test]
        public void Not_equals_operator_true_for_different_CompressedQuaternion()
        {
            var q1 = Quaternion.Euler(10, 20, 30).ToCompressedQuaternion();
            var q2 = Quaternion.Euler(40, 50, 60).ToCompressedQuaternion();

            Assert.IsTrue(q1 != q2);
        }

        [Test] public void Not_equals_operator_false_for_identical_CompressedQuaternion()
        {
            var q1 = Quaternion.Euler(10, 20, 30).ToCompressedQuaternion();
            var q2 = Quaternion.Euler(10, 20, 30).ToCompressedQuaternion();

            Assert.IsFalse(q1 != q2);
        }
    }
}
