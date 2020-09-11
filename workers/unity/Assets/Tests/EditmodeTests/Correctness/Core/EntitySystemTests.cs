using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Core
{
    [TestFixture]
    public class EntitySystemTests : MockBase
    {
        [TestCase(1)]
        [TestCase(4)]
        [TestCase(7)]
        public void EntitiesAdded_contains_all_entities_added(int count)
        {
            var toAdd = Enumerable.Range(1, count).Select(i => (long) i);
            var system = World.GetSystem<EntitySystem>();
            World.Step(world =>
            {
                foreach (var id in toAdd)
                {
                    world.Connection.CreateEntity(id, GetTemplate());
                }
            }).Step(world =>
            {
                var slice = system.EntitiesAdded;
                Assert.AreEqual(count, slice.Length);
                var set = new HashSet<long>(toAdd);
                foreach (var entityId in slice)
                {
                    Assert.IsTrue(set.Contains(entityId.Id));
                }
            });
        }

        [TestCase(1)]
        [TestCase(4)]
        [TestCase(7)]
        public void EntitiesRemoved_contains_all_entities_removed(int count)
        {
            var toRemove = Enumerable.Range(1, count).Select(i => (long) i);
            var system = World.GetSystem<EntitySystem>();
            World.Step(world =>
            {
                foreach (var id in toRemove)
                {
                    world.Connection.CreateEntity(id, GetTemplate());
                }
            }).Step(world =>
            {
                foreach (var id in toRemove)
                {
                    world.Connection.RemoveEntity(id);
                }
            }).Step(world =>
            {
                var slice = system.EntitiesRemoved;
                Assert.AreEqual(count, slice.Length);
                var set = new HashSet<long>(toRemove);
                foreach (var entityId in slice)
                {
                    Assert.IsTrue(set.Contains(entityId.Id));
                }
            });
        }

        [Test]
        public void Collections_are_empty_when_no_entities_added_or_removed()
        {
            var system = World.GetSystem<EntitySystem>();
            World.Step(world =>
            {
                Assert.AreEqual(0, system.EntitiesRemoved.Length);
                Assert.AreEqual(0, system.EntitiesAdded.Length);
            });
        }

        [TestCase(1)]
        [TestCase(4)]
        [TestCase(7)]
        public void Collections_only_last_for_single_frame(int count)
        {
            var toAdd = Enumerable.Range(1, count).Select(i => (long) i);
            var toRemove = Enumerable.Range(count, count).Select(i => (long) i);
            var system = World.GetSystem<EntitySystem>();
            World.Step(world =>
            {
                foreach (var id in toAdd)
                {
                    world.Connection.CreateEntity(id, GetTemplate());
                }

                foreach (var id in toRemove)
                {
                    world.Connection.CreateEntity(id, GetTemplate());
                }
            }).Step(world =>
            {
                foreach (var id in toRemove)
                {
                    world.Connection.RemoveEntity(id);
                }
            }).Step(world =>
            {
                var slice = system.EntitiesAdded;
                Assert.AreEqual(0, slice.Length);
            }).Step(world =>
            {
                var slice = system.EntitiesRemoved;
                Assert.AreEqual(0, slice.Length);
            });
        }

        private static EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            return template;
        }
    }
}
