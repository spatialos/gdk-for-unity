using System;
using System.Linq;
using Improbable.Gdk.Core;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class SnapshotTests
    {
        private Snapshot snapshot;

        [SetUp]
        public void Initialize()
        {
            snapshot = new Snapshot();
        }

        [Test]
        public void Contains_returns_true_if_entityId_exists()
        {
            var entityId = snapshot.GetNextEntityId();
            var template = GetTemplate();
            snapshot.AddEntity(entityId, template);
            Assert.IsTrue(snapshot.Contains(entityId));
        }

        [Test]
        public void Contains_returns_false_if_entityId_does_not_exists()
        {
            var entityId = snapshot.GetNextEntityId();
            Assert.IsFalse(snapshot.Contains(entityId));
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(40)]
        public void GetNextEntityId_will_return_valid_EntityIds(int size)
        {
            var entities = Enumerable.Range(1, size).Select(i => new EntityId(i));
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            foreach (var entity in entities)
            {
                snapshot.AddEntity(entity, template);
            }

            Assert.IsFalse(snapshot.Contains(snapshot.GetNextEntityId()));
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(40)]
        public void Count_increases_on_adding_new_entity(int size)
        {
            var entityIds = Enumerable.Range(1, size).Select(i => new EntityId(i));
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            foreach (var entityId in entityIds)
            {
                snapshot.AddEntity(entityId, template);
            }

            Assert.AreEqual(size, snapshot.Count);
        }

        [Test]
        public void AddEntity_throws_if_EntityId_exists()
        {
            var entityId = new EntityId(1);
            var template = GetTemplate();
            var template2 = GetTemplate(new Coordinates(1, 2, 3));
            snapshot.AddEntity(entityId, template);
            Assert.Throws<ArgumentException>(() => snapshot.AddEntity(entityId, template2));
        }

        [TearDown]
        public void TearDown()
        {
            snapshot.Dispose();
        }

        private static EntityTemplate GetTemplate(Coordinates pos = default)
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(pos));
            return template;
        }
    }
}
