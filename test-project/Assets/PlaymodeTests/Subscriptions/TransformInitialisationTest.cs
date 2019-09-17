using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Improbable.Gdk.Core;
using Improbable.Gdk.TransformSynchronization;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.PlaymodeTests.Subscriptions
{
    /// <summary>
    ///     This doesn't test anything at the moment kek.
    /// </summary>
    [TestFixture]
    public class TransformInitialisationTests : SubscriptionsTestBase
    {
        private const long EntityId = 100;

        private GameObject createdGameObject;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "TestWorkerType");
            template.AddComponent(new TransformInternal.Snapshot(), "TestWorkerType");
            ConnectionHandler.CreateEntity(EntityId, template);
            ReceiveSystem.Update();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            Object.DestroyImmediate(createdGameObject);
        }

        [UnityTest]
        public IEnumerator some_random_test_ignore_the_name_for_now()
        {
            // Load up prefab with TransformSynchronization behaviour
            createdGameObject = CreateAndLinkGameObjectWithTransformSync(EntityId);
            var transformSyncBehaviour = createdGameObject.GetComponent<TransformSynchronization.TransformSynchronization>();

            // Wait two frames for the strategies to be applied
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            // Check that the behaviour is still enabled, `entityManager` is set, and `initialised` is true
            Assert.IsTrue(transformSyncBehaviour.enabled);
            Assert.IsNotNull(GetPrivateField<EntityManager>(transformSyncBehaviour, "entityManager"));
            Assert.IsTrue(GetPrivateField<bool>(transformSyncBehaviour, "initialised"));

            // Remove TransformInternal component from entity and run an update of the receive system
            ConnectionHandler.RemoveComponent(EntityId, TransformInternal.ComponentId);
            ReceiveSystem.Update();

            // Run an update of the [Require] lifecycle system
            RequireLifecycleSystem.Update();

            // Wait a frame and then check that behaviour is disabled, `entityManager` is null, and `initialised` is false
            Assert.IsFalse(transformSyncBehaviour.enabled);
            Assert.IsNull(GetPrivateField<EntityManager>(transformSyncBehaviour, "entityManager"));
            Assert.IsFalse(GetPrivateField<bool>(transformSyncBehaviour, "initialised"));
        }

        private GameObject CreateAndLinkGameObjectWithTransformSync(long entityId)
        {
            var prefab = Resources.Load<GameObject>("TransformTestObject");
            var gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            Linker.LinkGameObjectToSpatialOSEntity(new EntityId(entityId), gameObject);
            RequireLifecycleSystem.Update();

            return gameObject;
        }

        private T GetPrivateField<T>(TransformSynchronization.TransformSynchronization transformSyncBehaviour, string fieldName)
        {
            var typeOfTransformSyncBehaviour = typeof(TransformSynchronization.TransformSynchronization);
            var fieldInfo = typeOfTransformSyncBehaviour.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var fieldValue = fieldInfo?.GetValue(transformSyncBehaviour);

            return (T) fieldValue;
        }
    }
}
