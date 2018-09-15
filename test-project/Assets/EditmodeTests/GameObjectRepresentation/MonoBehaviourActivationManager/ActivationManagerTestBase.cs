using Improbable.Gdk.Core;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    public abstract class ActivationManagerTestBase<TBehaviour> where TBehaviour : Component
    {
        private World world;
        protected GameObject TestGameObject;
        private EntityManager entityManager;
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
            Object.DestroyImmediate(TestGameObject);

            world?.Dispose();
            world = null;
        }
    }
}