using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class GameObjectDispatcherCallbacksSystemTests
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
        public void OnCreateManager_initializes_GameObjectTranslation_ComponentGroups()
        {
            var world = worker.World;

            foreach (var gameObjectTranslation in worker.View.GameObjectTranslations)
            {
                Assert.IsNull(gameObjectTranslation.ComponentAddedComponentGroup);
                Assert.IsNull(gameObjectTranslation.ComponentRemovedComponentGroup);
                Assert.IsNull(gameObjectTranslation.AuthoritiesChangedComponentGroup);
                Assert.IsNull(gameObjectTranslation.ComponentsUpdatedComponentGroup);
                Assert.IsNull(gameObjectTranslation.EventsReceivedComponentGroups);
                Assert.IsNull(gameObjectTranslation.CommandRequestsComponentGroups);
            }

            world.GetOrCreateManager<GameObjectDispatcherCallbacksSystem>();

            foreach (var gameObjectTranslation in worker.View.GameObjectTranslations)
            {
                Assert.IsNotNull(gameObjectTranslation.ComponentAddedComponentGroup);
                Assert.IsNotNull(gameObjectTranslation.ComponentRemovedComponentGroup);
                Assert.IsNotNull(gameObjectTranslation.AuthoritiesChangedComponentGroup);
                if (gameObjectTranslation.ComponentsUpdatedComponentTypes.Length > 0)
                {
                    Assert.IsNotNull(gameObjectTranslation.ComponentsUpdatedComponentGroup);
                }
                else
                {
                    Assert.IsNull(gameObjectTranslation.ComponentsUpdatedComponentGroup);
                }

                Assert.IsNotNull(gameObjectTranslation.EventsReceivedComponentGroups);
                Assert.IsNotNull(gameObjectTranslation.CommandRequestsComponentGroups);
            }
        }
    }
}
