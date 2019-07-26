using System;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Operators
{
    [TestFixture]
    public class CoordinatesTests
    {
        [Test]
        public void Equals_operator_true_for_identical_Coordinates()
        {
            var c1 = new Coordinates(10d, 20d, 30d);
            var c2 = new Coordinates(10d, 20d, 30d);

            Assert.IsTrue(c1 == c2);
        }

        [Test]
        public void Equals_operator_false_for_different_Coordinates()
        {
            var c1 = new Coordinates(10d, 20d, 30d);
            var c2 = new Coordinates(40d, 50d, 60d);

            Assert.IsFalse(c1 == c2);
        }

        [Test]
        public void Not_equals_operator_true_for_different_Coordinates()
        {
            var c1 = new Coordinates(10d, 20d, 30d);
            var c2 = new Coordinates(40d, 50d, 60d);

            Assert.IsTrue(c1 != c2);
        }

        [Test] public void Not_equals_operator_false_for_identical_Coordinates()
        {
            var c1 = new Coordinates(10d, 20d, 30d);
            var c2 = new Coordinates(10d, 20d, 30d);

            Assert.IsFalse(c1 != c2);
        }

        [Test]
        public void Addition_operator_returns_correct_result()
        {
            var c1 = new Coordinates(10d, 20d, 30d);
            var c2 = new Coordinates(40d, 50d, 60d);

            var c3 = c1 + c2;

            Assert.IsTrue(Math.Abs(c3.X - 50) < float.Epsilon);
            Assert.IsTrue(Math.Abs(c3.Y - 70) < float.Epsilon);
            Assert.IsTrue(Math.Abs(c3.Z - 90) < float.Epsilon);
        }

        [Test]
        public void Subtraction_operator_returns_correct_result()
        {
            var c1 = new Coordinates(10d, 20d, 30d);
            var c2 = new Coordinates(40d, 50d, 60d);

            var c3 = c2 - c1;

            Assert.IsTrue(Math.Abs(c3.X - 30) < float.Epsilon);
            Assert.IsTrue(Math.Abs(c3.Y - 30) < float.Epsilon);
            Assert.IsTrue(Math.Abs(c3.Z - 30) < float.Epsilon);
        }

        [Test]
        public void Multiplication_operator_returns_correct_result()
        {
            var c1 = new Coordinates(10d, 20d, 30d);
            var c2 = c1 * 10d;

            Assert.IsTrue(Math.Abs(c2.X - 100) < float.Epsilon);
            Assert.IsTrue(Math.Abs(c2.Y - 200) < float.Epsilon);
            Assert.IsTrue(Math.Abs(c2.Z - 300) < float.Epsilon);
        }

        [Test]
        public void Division_operator_returns_correct_result()
        {
            var c1 = new Coordinates(10d, 20d, 30d);
            var c2 = c1 / 10d;

            Assert.IsTrue(Math.Abs(c2.X - 1) < float.Epsilon);
            Assert.IsTrue(Math.Abs(c2.Y - 2) < float.Epsilon);
            Assert.IsTrue(Math.Abs(c2.Z - 3) < float.Epsilon);
        }
    }
}
