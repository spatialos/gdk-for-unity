using Improbable.Gdk.Core;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    public abstract class ActivationManagerTestBase<TBehaviour> where TBehaviour : MonoBehaviour
    {
        private World world;
        private EntityManager entityManager;
        protected GameObject TestGameObject;
        protected MonoBehaviourActivationManager ActivationManager;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            entityManager = world.GetOrCreateManager<EntityManager>();
            TestGameObject = new GameObject();

            TestGameObject.AddComponent<TBehaviour>();
            TestGameObject.AddComponent<SpatialOSComponent>().Entity = entityManager.CreateEntity();

            var loggingDispatcher = new LoggingDispatcher();
            var injectableStore = new InjectableStore();
            var requiredFieldInjector = new RequiredFieldInjector(entityManager, loggingDispatcher);

            ActivationManager = new MonoBehaviourActivationManager(TestGameObject,
                requiredFieldInjector, injectableStore, loggingDispatcher);
        }

        [TearDown]
        public void TearDown()
        {
            ActivationManager?.Dispose();
            ActivationManager = null;
            UnityObjectDestroyer.Destroy(TestGameObject);
            world?.Dispose();
            world = null;
        }
    }
}
