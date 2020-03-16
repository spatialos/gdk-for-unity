using System;
using System.Linq;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Collections
{
    [TestFixture]
    public class EntityRangeCollectionTests
    {
        private const int ValidRangeCount = 10;
        private readonly EntityRangeCollection.EntityIdRange validRange = new EntityRangeCollection.EntityIdRange(new EntityId(1), ValidRangeCount);
        private readonly EntityRangeCollection.EntityIdRange invalidRange = new EntityRangeCollection.EntityIdRange(new EntityId(0), 1);
        private readonly EntityRangeCollection.EntityIdRange emptyRange = new EntityRangeCollection.EntityIdRange(new EntityId(1), 0);

        [Test]
        public void Add_single_valid_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);

            Assert.AreEqual(ValidRangeCount, collection.Count);
        }

        [Test]
        public void Add_multiple_valid_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            collection.Add(validRange);
            collection.Add(validRange);

            Assert.AreEqual(ValidRangeCount * 3, collection.Count);
        }

        [Test]
        public void Add_invalid_range_throws()
        {
            var collection = new EntityRangeCollection();

            Assert.Throws<ArgumentException>(() => collection.Add(invalidRange));
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Add_empty_range_throws()
        {
            var collection = new EntityRangeCollection();

            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Add(emptyRange));
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Dequeue_when_not_empty()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            var result = collection.Dequeue();

            Assert.AreEqual(validRange.FirstEntityId, result);
            Assert.AreEqual(ValidRangeCount - 1, collection.Count);
        }

        [Test]
        public void Dequeue_when_empty_throws()
        {
            var collection = new EntityRangeCollection();

            Assert.Throws<InvalidOperationException>(() => collection.Dequeue());
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Dequeue_pops_ranges()
        {
            var collection = new EntityRangeCollection();
            collection.Add(new EntityRangeCollection.EntityIdRange(new EntityId(1), 1));
            collection.Add(new EntityRangeCollection.EntityIdRange(new EntityId(2), 1));

            collection.Dequeue();
            collection.Dequeue();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Take_single_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            var result = collection.Take(ValidRangeCount);

            Assert.AreEqual(ValidRangeCount, result.Length);
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Take_partial_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            var result = collection.Take(validRange.Count / 2);

            Assert.AreEqual(ValidRangeCount / 2, result.Length);
            Assert.AreEqual(ValidRangeCount / 2, collection.Count);
        }

        [Test]
        public void Take_multiple_ranges()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            collection.Add(validRange);

            collection.Take(ValidRangeCount / 2);
            collection.Take(ValidRangeCount + ValidRangeCount / 2);
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Take_too_much_throws()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);

            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Take(ValidRangeCount * 2));
            Assert.AreEqual(ValidRangeCount, collection.Count);
        }

        [Test]
        public void Clear_resets_collection()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            collection.Clear();

            Assert.Throws<InvalidOperationException>(() => collection.Dequeue());
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Take(1));
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void Take_returns_valid_entity_ids()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            collection.Add(new EntityRangeCollection.EntityIdRange(new EntityId(ValidRangeCount + 1), ValidRangeCount));

            var idCollection = Enumerable.Range(1, ValidRangeCount * 2).Select(x => (long) x).ToList();

            var check = 0;
            foreach (var entityId in collection.Take(collection.Count))
            {
                Assert.AreEqual(idCollection[check], entityId.Id);
                check++;
            }

            Assert.AreEqual(ValidRangeCount * 2, check);
            Assert.AreEqual(0, collection.Count);
        }
    }
}
