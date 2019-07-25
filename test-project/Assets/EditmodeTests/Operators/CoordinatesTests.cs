using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Operators
{
    [TestFixture]
    public class CoordinatesTests
    {
        [Test]
        public void Equals_operator_true_for_identical_Coordinates()
        {
            var c1 = new Coordinates(10, 20, 30);
            var c2 = new Coordinates(10, 20, 30);

            Assert.IsTrue(c1 == c2);
        }

        [Test]
        public void Equals_operator_false_for_different_Coordinates()
        {
            var c1 = new Coordinates(10, 20, 30);
            var c2 = new Coordinates(40, 50, 60);

            Assert.IsFalse(c1 == c2);
        }

        [Test]
        public void Not_equals_operator_true_for_different_Coordinates()
        {
            var c1 = new Coordinates(10, 20, 30);
            var c2 = new Coordinates(40, 50, 60);

            Assert.IsTrue(c1 != c2);
        }

        [Test] public void Not_equals_operator_false_for_identical_Coordinates()
        {
            var c1 = new Coordinates(10, 20, 30);
            var c2 = new Coordinates(10, 20, 30);

            Assert.IsFalse(c1 != c2);
        }

        [Test]
        public void Addition_operator_returns_correct_result()
        {
            var c1 = new Coordinates(10, 20, 30);
            var c2 = new Coordinates(40, 50, 60);

            var e3 = c1 + c2;

            Assert.IsTrue(e3.X == 50);
            Assert.IsTrue(e3.Y == 70);
            Assert.IsTrue(e3.Z == 90);
        }

        [Test]
        public void Subtraction_operator_returns_correct_result()
        {
            var c1 = new Coordinates(10, 20, 30);
            var c2 = new Coordinates(40, 50, 60);

            var e3 = c2 - c1;

            Assert.IsTrue(e3.X == 30);
            Assert.IsTrue(e3.Y == 30);
            Assert.IsTrue(e3.Z == 30);
        }

        [Test]
        public void Multiplication_operator_returns_correct_result()
        {
            var c1 = new Coordinates(10, 20, 30);
            var c2 = c1 * 10;

            Assert.IsTrue(c2.X == 100);
            Assert.IsTrue(c2.Y == 200);
            Assert.IsTrue(c2.Z == 300);
        }

        [Test]
        public void Division_operator_returns_correct_result()
        {
            var c1 = new Coordinates(10, 20, 30);
            var c2 = c1 / 10;

            Assert.IsTrue(c2.X == 1);
            Assert.IsTrue(c2.Y == 2);
            Assert.IsTrue(c2.Z == 3);
        }
    }
}
