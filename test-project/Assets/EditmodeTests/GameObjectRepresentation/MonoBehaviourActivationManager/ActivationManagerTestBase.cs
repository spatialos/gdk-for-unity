using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectRepresentation;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.MonoBehaviourActivationManagerTests
{
    public abstract class ActivationManagerTestBase
    {
        private World world;
        private EntityManager entityManager;
        protected GameObject TestGameObject;
        protected MonoBehaviourActivationManager ActivationManager;
        protected virtual string WorkerType => string.Empty;

        [SetUp]
        public void SetUp()
        {
            world = new World("test-world");
            entityManager = world.GetOrCreateManager<EntityManager>();
            TestGameObject = new GameObject();
            var loggingDispatcher = new LoggingDispatcher();
            var workerSystem = world.CreateManager<WorkerSystem>(
                null,               // Connection connection
                loggingDispatcher,  // ILogDispatcher logDispatcher
                WorkerType,         // string workerType
                Vector3.zero        // Vector3 origin
            );

            PopulateBehaviours();

            var spatialOSComponent = TestGameObject.AddComponent<SpatialOSComponent>();
            spatialOSComponent.Worker = workerSystem;
            spatialOSComponent.Entity = entityManager.CreateEntity();

            var injectableStore = new InjectableStore();
            var requiredFieldInjector = new RequiredFieldInjector(entityManager, loggingDispatcher);

            ActivationManager = new MonoBehaviourActivationManager(TestGameObject,
                requiredFieldInjector, injectableStore, loggingDispatcher);
        }

        protected abstract void PopulateBehaviours();

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
