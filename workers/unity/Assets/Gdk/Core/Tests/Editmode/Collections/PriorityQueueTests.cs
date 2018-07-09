using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class PriorityQueueTests
    {
        [Test]
        public void Pop_values_added_in_ascending_order_in_correct_order()
        {
            var queue = new PriorityQueue<int>();

            queue.Push(1);
            Assert.AreEqual(1, queue.Peak());

            queue.Push(2);
            Assert.AreEqual(2, queue.Peak());

            queue.Push(3);
            Assert.AreEqual(3, queue.Peak());

            Assert.AreEqual(3, queue.Pop());
            Assert.AreEqual(2, queue.Pop());
            Assert.AreEqual(1, queue.Pop());
        }

        [Test]
        public void Pop_values_added_in_descending_order_in_correct_order()
        {
            var queue = new PriorityQueue<int>();

            queue.Push(3);
            Assert.AreEqual(3, queue.Peak());

            queue.Push(2);
            Assert.AreEqual(3, queue.Peak());

            queue.Push(1);
            Assert.AreEqual(3, queue.Peak());

            Assert.AreEqual(3, queue.Pop());
            Assert.AreEqual(2, queue.Pop());
            Assert.AreEqual(1, queue.Pop());
        }
    }
}

