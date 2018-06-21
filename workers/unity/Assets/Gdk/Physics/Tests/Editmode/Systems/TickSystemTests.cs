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
            // Construct a world
            // The "using" ensures the world will get destroyed (along with its systems)
            using (var world = new World("test-world"))
            {
                // Add system
                var tickSystem = world.GetOrCreateManager<TickSystem>();

                // Make assertion against initial state of system
                Assert.AreEqual(0, tickSystem.GlobalTick);

                // Execute system
                tickSystem.Update();

                // Assert the system behaves correctly
                Assert.AreEqual(1, tickSystem.GlobalTick);

                tickSystem.Update();

                Assert.AreEqual(2, tickSystem.GlobalTick);
            }
        }
    }
}
