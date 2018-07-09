using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class EntityGameObjectLifecycleSystemTests : HybridGdkSystemTestBase
    {
        private World world;
        private EntityManager entityManager;
        private MutableView mutableView;
        private EntityGameObjectLifecycleSystem entityGameObjectLifecycleSystem;
        private Entity testEntity;
        private GameObject testGameObject;

        [SetUp]
        public void Setup()
        {
            world = new World("TestWorld");
            entityManager = world.GetOrCreateManager<EntityManager>();
            entityGameObjectLifecycleSystem = world.GetOrCreateManager<EntityGameObjectLifecycleSystem>();
            testEntity = entityManager.CreateEntity();
            testGameObject = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(testGameObject);
            world.Dispose();
        }

        [Test]
        public void OnUpdate_adds_GameObjectReferenceHandle_to_new_entities()
        {
            var gameObjectReference = new GameObjectReference { GameObject = testGameObject };
            ComponentObjectSetter.AddAndSetComponentObject(testEntity, gameObjectReference, entityManager);
            entityGameObjectLifecycleSystem.Update();
            Assert.IsTrue(entityManager.HasComponent<GameObjectReferenceHandle>(testEntity));
        }

        [Test]
        public void OnUpdate_removes_GameObject_and_GameObjectReferenceHandle_from_obsolete_entities()
        {
            var gameObjectReference = new GameObjectReference { GameObject = testGameObject };
            ComponentObjectSetter.AddAndSetComponentObject(testEntity, gameObjectReference, entityManager);
            entityGameObjectLifecycleSystem.Update();

            entityManager.DestroyEntity(testEntity);
            Assert.NotNull(testGameObject);
            Assert.IsTrue(entityManager.Exists(testEntity));
            Assert.IsTrue(entityManager.HasComponent<GameObjectReferenceHandle>(testEntity));

            entityGameObjectLifecycleSystem.Update();
            Assert.IsTrue(testGameObject == null);
            Assert.IsFalse(entityManager.Exists(testEntity));
            Assert.IsFalse(entityManager.HasComponent<GameObjectReferenceHandle>(testEntity));
        }
    }
}
