using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Operators
{
    [TestFixture]
    public class EdgeLengthTests
    {
        [Test]
        public void Equals_operator_true_for_identical_EdgeLength()
        {
            var e1 = new EdgeLength(10, 20, 30);
            var e2 = new EdgeLength(10, 20, 30);

            Assert.IsTrue(e1 == e2);
        }

        [Test]
        public void Equals_operator_false_for_different_EdgeLength()
        {
            var e1 = new EdgeLength(10, 20, 30);
            var e2 = new EdgeLength(40, 50, 60);

            Assert.IsFalse(e1 == e2);
        }

        [Test]
        public void Not_equals_operator_true_for_different_EdgeLength()
        {
            var e1 = new EdgeLength(10, 20, 30);
            var e2 = new EdgeLength(40, 50, 60);

            Assert.IsTrue(e1 != e2);
        }

        [Test] public void Not_equals_operator_false_for_identical_EdgeLength()
        {
            var e1 = new EdgeLength(10, 20, 30);
            var e2 = new EdgeLength(10, 20, 30);

            Assert.IsFalse(e1 != e2);
        }

        [Test]
        public void Addition_operator_returns_correct_result()
        {
            var e1 = new EdgeLength(10, 20, 30);
            var e2 = new EdgeLength(40, 50, 60);

            var e3 = e1 + e2;

            Assert.IsTrue(e3.X == 50);
            Assert.IsTrue(e3.Y == 70);
            Assert.IsTrue(e3.Z == 90);
        }

        [Test]
        public void Subtraction_operator_returns_correct_result()
        {
            var e1 = new EdgeLength(10, 20, 30);
            var e2 = new EdgeLength(40, 50, 60);

            var e3 = e2 - e1;

            Assert.IsTrue(e3.X == 30);
            Assert.IsTrue(e3.Y == 30);
            Assert.IsTrue(e3.Z == 30);
        }

        [Test]
        public void Multiplication_operator_returns_correct_result()
        {
            var e1 = new EdgeLength(10, 20, 30);
            var e2 = e1 * 10;

            Assert.IsTrue(e2.X == 100);
            Assert.IsTrue(e2.Y == 200);
            Assert.IsTrue(e2.Z == 300);
        }

        [Test]
        public void Division_operator_returns_correct_result()
        {
            var e1 = new EdgeLength(10, 20, 30);
            var e2 = e1 / 10;

            Assert.IsTrue(e2.X == 1);
            Assert.IsTrue(e2.Y == 2);
            Assert.IsTrue(e2.Z == 3);
        }
    }
}
