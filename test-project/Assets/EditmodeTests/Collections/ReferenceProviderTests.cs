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
                    world.Connection.CreateEntity(entityId, GetTemplate());
                });

                Assert.AreEqual(1, ReferenceProvider<string>.Count);
            }

            Assert.AreEqual(0, ReferenceProvider<string>.Count);
        }

        [Test]
        public void Removed_component_disposes_reference()
        {
            var template = GetTemplate();
            using (var mockWorld = MockWorld.Create(new MockWorld.Options()))
            {
                mockWorld
                    .Step(world =>
                    {
                        world.Connection.CreateEntity(entityId, template);
                    })
                    .Step(world =>
                    {
                        Assert.AreEqual(1, ReferenceProvider<string>.Count);
                        world.Connection.RemoveEntityAndComponents(entityId, template);
                    });

                Assert.AreEqual(0, ReferenceProvider<string>.Count);
            }
        }

        private EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "some-worker");
            template.AddComponent(new Metadata.Snapshot("AReferenceType"), "some-worker");
            return template;
        }
    }
}
