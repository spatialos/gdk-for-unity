using Improbable.Gdk.TransformSynchronization;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.EditmodeTests.TransformSynchronization.Systems
{
    [TestFixture]
    public class TickSystemTests
    {
        [Test]
        public void GlobalTick_gets_incremented_in_OnUpdate()
        {
            using (var world = new World("test-world"))
            {
                var tickSystem = world.GetOrCreateManager<TickSystem>();

                Assert.AreEqual(0, tickSystem.GlobalTick);

                tickSystem.Update();

                Assert.AreEqual(1, tickSystem.GlobalTick);

                tickSystem.Update();

                Assert.AreEqual(2, tickSystem.GlobalTick);
            }
        }
    }
}
