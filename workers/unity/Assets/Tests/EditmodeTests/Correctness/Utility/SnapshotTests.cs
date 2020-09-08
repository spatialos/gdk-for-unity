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
        public void IsValid_returns_false_if_entityId_exists()
        {
            var entity = snapshot.GetNextEntityId();
            var template = GetTemplate();
            snapshot.AddEntity(entity, template);
            Assert.IsFalse(snapshot.IsValid(entity));
        }

        [Test]
        public void IsValid_returns_true_if_entityId_does_not_exists()
        {
            var entity = snapshot.GetNextEntityId();
            Assert.IsTrue(snapshot.IsValid(entity));
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

            Assert.IsTrue(snapshot.IsValid(snapshot.GetNextEntityId()));
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(40)]
        public void Count_increases_on_adding_new_entity(int size)
        {
            var entities = Enumerable.Range(1, size).Select(i => new EntityId(i));
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            foreach (var entity in entities)
            {
                snapshot.AddEntity(entity, template);
            }

            Assert.AreEqual(size, snapshot.Count);
        }

        [Test]
        public void Count_does_not_increase_if_overwriting_entity()
        {
            var entity = new EntityId(1);
            var template = GetTemplate();

            var template2 = new EntityTemplate();
            template2.AddComponent(new Position.Snapshot(new Coordinates(1, 2, 3)));
            snapshot.AddEntity(entity, template);
            snapshot.AddEntity(entity, template2);

            Assert.AreEqual(1, snapshot.Count);
        }

        private static EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            return template;
        }
    }
}
