using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class GameObjectDispatcherSystemTests
    {
        private UnityTestWorker worker;

        [SetUp]
        public void Setup()
        {
            worker = new UnityTestWorker("worker-id", new Vector3());
        }

        [TearDown]
        public void TearDown()
        {
            worker.Dispose();
        }

        [Test]
        public void OnCreateManager_initializes_GameObjectComponentDispatcher_ComponentGroups()
        {
            var gameObjectDispatcherSystem = worker.World.GetOrCreateManager<GameObjectDispatcherSystem>();

            foreach (var gameObjectComponentDispatcher in gameObjectDispatcherSystem.GameObjectComponentDispatchers)
            {
                Assert.IsNotNull(gameObjectComponentDispatcher.ComponentAddedComponentGroup);
                Assert.IsNotNull(gameObjectComponentDispatcher.ComponentRemovedComponentGroup);
                Assert.IsNotNull(gameObjectComponentDispatcher.AuthoritiesChangedComponentGroup);
                if (gameObjectComponentDispatcher.ComponentsUpdatedComponentTypes.Length > 0)
                {
                    Assert.IsNotNull(gameObjectComponentDispatcher.ComponentsUpdatedComponentGroup);
                }
                else
                {
                    Assert.IsNull(gameObjectComponentDispatcher.ComponentsUpdatedComponentGroup);
                }
                if (gameObjectComponentDispatcher.EventsReceivedComponentTypeArrays.Length > 0)
                {
                    Assert.IsNotNull(gameObjectComponentDispatcher.EventsReceivedComponentGroups);
                }
                else
                {
                    Assert.IsNull(gameObjectComponentDispatcher.EventsReceivedComponentGroups);
                }
                if (gameObjectComponentDispatcher.CommandRequestsComponentTypeArrays.Length > 0)
                {
                    Assert.IsNotNull(gameObjectComponentDispatcher.CommandRequestsComponentGroups);
                }
                else
                {
                    Assert.IsNull(gameObjectComponentDispatcher.CommandRequestsComponentGroups);
                }
            }
        }
    }
}
