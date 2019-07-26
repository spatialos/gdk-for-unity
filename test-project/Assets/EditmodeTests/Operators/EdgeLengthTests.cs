using System;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Operators
{
    [TestFixture]
    public class EdgeLengthTests
    {
        [Test]
        public void Equals_operator_true_for_identical_EdgeLength()
        {
            var e1 = new EdgeLength(10d, 20d, 30d);
            var e2 = new EdgeLength(10d, 20d, 30d);

            Assert.IsTrue(e1 == e2);
        }

        [Test]
        public void Equals_operator_false_for_different_EdgeLength()
        {
            var e1 = new EdgeLength(10d, 20d, 30d);
            var e2 = new EdgeLength(40d, 50d, 60d);

            Assert.IsFalse(e1 == e2);
        }

        [Test]
        public void Not_equals_operator_true_for_different_EdgeLength()
        {
            var e1 = new EdgeLength(10d, 20d, 30d);
            var e2 = new EdgeLength(40d, 50d, 60d);

            Assert.IsTrue(e1 != e2);
        }

        [Test] public void Not_equals_operator_false_for_identical_EdgeLength()
        {
            var e1 = new EdgeLength(10d, 20d, 30d);
            var e2 = new EdgeLength(10d, 20d, 30d);

            Assert.IsFalse(e1 != e2);
        }

        [Test]
        public void Addition_operator_returns_correct_result()
        {
            var e1 = new EdgeLength(10d, 20d, 30d);
            var e2 = new EdgeLength(40d, 50d, 60d);

            var e3 = e1 + e2;

            Assert.IsTrue(Math.Abs(e3.X - 50) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e3.Y - 70) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e3.Z - 90) < float.Epsilon);
        }

        [Test]
        public void Subtraction_operator_returns_correct_result()
        {
            var e1 = new EdgeLength(10d, 20d, 30d);
            var e2 = new EdgeLength(40d, 50d, 60d);

            var e3 = e2 - e1;

            Assert.IsTrue(Math.Abs(e3.X - 30) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e3.Y - 30) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e3.Z - 30) < float.Epsilon);
        }

        [Test]
        public void Multiplication_operator_returns_correct_result()
        {
            var e1 = new EdgeLength(10d, 20d, 30d);
            var e2 = e1 * 10d;

            Assert.IsTrue(Math.Abs(e2.X - 100) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e2.Y - 200) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e2.Z - 300) < float.Epsilon);
        }

        [Test]
        public void Division_operator_returns_correct_result()
        {
            var e1 = new EdgeLength(10d, 20d, 30d);
            var e2 = e1 / 10d;

            Assert.IsTrue(Math.Abs(e2.X - 1) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e2.Y - 2) < float.Epsilon);
            Assert.IsTrue(Math.Abs(e2.Z - 3) < float.Epsilon);
        }
    }
}
