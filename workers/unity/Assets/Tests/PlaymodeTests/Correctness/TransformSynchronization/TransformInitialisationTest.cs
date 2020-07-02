using System;
using System.Collections;
using System.Reflection;
using Improbable.Gdk.Core;
using Improbable.Gdk.TestBases;
using Improbable.Gdk.TransformSynchronization;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.PlaymodeTests.TransformSynchronization
{
    [TestFixture]
    public class TransformInitialisationTests : SubscriptionsTestBase
    {
        private const long EntityId = 101;

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
        public IEnumerator Transform_initialises_on_enable_and_resets_on_disable()
        {
            // Load up prefab that already has TransformSynchronization behaviour
            var testTransformPrefab = Resources.Load<GameObject>("TransformTestObject");
            createdGameObject = CreateAndLinkGameObject(EntityId, testTransformPrefab, Vector3.zero, Quaternion.identity);

            var transformSyncBehaviour = createdGameObject.GetComponent<Improbable.Gdk.TransformSynchronization.TransformSynchronization>();

            // TransformSync script applies strategies in a coroutine that yields at least one and up to two times.
            // We yield twice in the test to be extra sure that the strategies are applied and that `initialised` is set.
            yield return null;
            yield return null;

            Assert.IsTrue(transformSyncBehaviour.enabled);
            Assert.IsNotNull(GetPrivateField<EntityManager>(transformSyncBehaviour, "entityManager"));
            Assert.IsTrue(GetPrivateField<bool>(transformSyncBehaviour, "initialised"));

            ConnectionHandler.RemoveComponent(EntityId, TransformInternal.ComponentId);
            ReceiveSystem.Update();

            RequireLifecycleSystem.Update();

            Assert.IsFalse(transformSyncBehaviour.enabled);
            Assert.IsNull(GetPrivateField<EntityManager>(transformSyncBehaviour, "entityManager"));
            Assert.IsFalse(GetPrivateField<bool>(transformSyncBehaviour, "initialised"));
        }

        private T GetPrivateField<T>(Improbable.Gdk.TransformSynchronization.TransformSynchronization transformSyncBehaviour, string fieldName)
        {
            var typeOfTransformSyncBehaviour = typeof(Improbable.Gdk.TransformSynchronization.TransformSynchronization);
            var fieldInfo = typeOfTransformSyncBehaviour.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo == null)
            {
                throw new NullReferenceException($"Error getting private field {fieldName}.");
            }

            return (T) fieldInfo.GetValue(transformSyncBehaviour);
        }
    }
}
