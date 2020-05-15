using Improbable.Gdk.Core;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Assert = Unity.Assertions.Assert;

namespace Improbable.Gdk.EditmodeTests.Collections
{
    [TestFixture]
    public class ReferenceProviderTests
    {
        private readonly long entityId = 100;

        [Test]
        public void Disposed_world_cleans_provider()
        {
            using (var mockWorld = MockWorld.Create(new MockWorld.Options()))
            {
                mockWorld.Step(world =>
                {
                    world.Connection.CreateEntity(entityId, new EntityTemplate());

                    world.Connection.AddComponent(entityId, Position.ComponentId,
                        new Position.Update {Coords = Coordinates.Zero});

                    world.Connection.AddComponent(entityId, Metadata.ComponentId,
                        new Metadata.Update {EntityType = "EntityWithReferenceComponent"});
                });

                Assert.AreEqual(1, ReferenceProvider<string>.Count);
            }

            Assert.AreEqual(0, ReferenceProvider<string>.Count);
        }

        [Test]
        public void Removed_component_disposes_reference()
        {
            using (var mockWorld = MockWorld.Create(new MockWorld.Options()))
            {
                mockWorld.Step(world =>
                {
                    world.Connection.CreateEntity(entityId, new EntityTemplate());

                    world.Connection.AddComponent(entityId, Position.ComponentId,
                        new Position.Update {Coords = Coordinates.Zero});

                    world.Connection.AddComponent(entityId, Metadata.ComponentId,
                        new Metadata.Update {EntityType = "EntityWithReferenceComponent"});
                }).Step(world =>
                {
                    Assert.AreEqual(1, ReferenceProvider<string>.Count);
                    world.Connection.RemoveComponent(entityId, Metadata.ComponentId);
                });

                Assert.AreEqual(0, ReferenceProvider<string>.Count);
            }
        }
    }
}
