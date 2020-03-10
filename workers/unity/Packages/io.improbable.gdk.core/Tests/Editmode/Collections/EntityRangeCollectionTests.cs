using System;
using System.Linq;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.Collections
{
    [TestFixture]
    public class EntityRangeCollectionTests
    {
        private const int ValidRangeCount = 10;
        private EntityRangeCollection.EntityIdRange validRange = new EntityRangeCollection.EntityIdRange(new EntityId(1), ValidRangeCount);
        private EntityRangeCollection.EntityIdRange invalidRange = new EntityRangeCollection.EntityIdRange(new EntityId(0), 1);
        private EntityRangeCollection.EntityIdRange emptyRange = new EntityRangeCollection.EntityIdRange(new EntityId(1), 0);

        [Test]
        public void EntityRangeCollection_add_single_valid_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);

            Assert.AreEqual(ValidRangeCount, collection.Count);
        }

        [Test]
        public void EntityRangeCollection_add_multiple_valid_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            collection.Add(validRange);
            collection.Add(validRange);

            Assert.AreEqual(ValidRangeCount * 3, collection.Count);
        }

        [Test]
        public void EntityRangeCollection_add_invalid_range_throws()
        {
            var collection = new EntityRangeCollection();
            Assert.Throws<ArgumentException>(() => collection.Add(invalidRange));
        }

        [Test]
        public void EntityRangeCollection_add_empty_range_throws()
        {
            var collection = new EntityRangeCollection();
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Add(emptyRange));
        }

        [Test]
        public void EntityRangeCollection_dequeue_valid()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            var result = collection.Dequeue();

            Assert.AreEqual(validRange.FirstEntityId, result);
            Assert.AreEqual(ValidRangeCount - 1, collection.Count);
        }

        [Test]
        public void EntityRangeCollection_dequeue_empty_throws()
        {
            var collection = new EntityRangeCollection();
            Assert.Throws<InvalidOperationException>(() => collection.Dequeue());
        }

        [Test]
        public void EntityRangeCollection_taking_whole_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            var result = collection.Take(ValidRangeCount);

            Assert.AreEqual(ValidRangeCount, result.Length);
        }

        [Test]
        public void EntityRangeCollection_taking_split_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            var result = collection.Take(validRange.Count / 2);

            Assert.AreEqual(ValidRangeCount / 2, result.Length);
        }

        [Test]
        public void EntityRangeCollection_taking_multiple()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            collection.Add(validRange);

            collection.Take(ValidRangeCount / 2);
            collection.Take(ValidRangeCount + ValidRangeCount / 2);
        }

        [Test]
        public void EntityRangeCollection_taking_too_much_throws()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Take(ValidRangeCount * 2));
        }

        [Test]
        public void EntityRangeCollection_clear_resets_collection()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void EntityRangeCollection_enumerator_single_range()
        {
            var collection = new EntityRangeCollection();
            collection.Add(validRange);

            var idCollection = Enumerable.Range(1, ValidRangeCount).Select(x => (long) x).ToList();

            var check = 0;
            foreach (var entityId in collection.Take(collection.Count))
            {
                Assert.AreEqual(idCollection[check], entityId.Id);
                check++;
            }

            Assert.AreEqual(collection.Count, check);
        }

        [Test]
        public void EntityRangeCollection_enumerator_multiple_range()
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

            Assert.AreEqual(collection.Count, check);
        }
    }
}
