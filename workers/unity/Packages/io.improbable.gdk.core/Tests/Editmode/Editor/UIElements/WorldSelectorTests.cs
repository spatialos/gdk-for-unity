using Improbable.Gdk.Core.Editor.UIElements;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests.Editor.UIElements
{
    [TestFixture]
    public class WorldSelectorTests
    {
        [Test]
        public void WorldSelector_ActiveWorld_will_have_null_if_no_worlds()
        {
            var element = new WorldSelector();
            element.UpdateWorldSelection();
            Assert.IsNull(element.ActiveWorld);
        }

        [Test]
        public void WorldSelector_ActiveWorld_will_have_null_if_no_spatial_worlds()
        {
            using (new World("some-world"))
            {
                var element = new WorldSelector();
                element.UpdateWorldSelection();
                Assert.IsNull(element.ActiveWorld);
            }
        }

        [Test]
        public void WorldSelector_ActiveWorld_will_be_non_null_if_spatial_worlds()
        {
            using (var mockWorld = MockWorld.Create(new MockWorld.Options()))
            {
                var element = new WorldSelector();
                element.UpdateWorldSelection();

                Assert.IsNotNull(element.ActiveWorld);
                Assert.AreEqual(mockWorld.Worker.World, element.ActiveWorld);
            }
        }

        [Test]
        public void WorldSelector_OnWorldChanged_called_when_world_changed()
        {
            using (MockWorld.Create(new MockWorld.Options()))
            {
                var element = new WorldSelector();

                var changed = false;
                element.OnWorldChanged += world => changed = true;

                element.UpdateWorldSelection();

                Assert.IsTrue(changed);
            }
        }

        [Test]
        public void WorldSelector_OnWorldChanged_not_called_if_no_change()
        {
            using (MockWorld.Create(new MockWorld.Options()))
            {
                var element = new WorldSelector();
                element.UpdateWorldSelection();

                var changed = false;
                element.OnWorldChanged += world => changed = true;

                element.UpdateWorldSelection();


                Assert.IsFalse(changed);
            }
        }
    }
}
