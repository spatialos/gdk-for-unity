using Improbable.Gdk.GameObjectRepresentation;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class WorkerTypeActivationTests : ActivationManagerTestBase
    {
        private const string OtherWorkerType = nameof(OtherWorkerType);
        private const string CurrentWorkerType = nameof(CurrentWorkerType);
        private static readonly uint PositionComponentId = new Position.Component().ComponentId;
        protected override string WorkerType => CurrentWorkerType;

        [WorkerType(CurrentWorkerType)]
        public class TestBehaviourForCurrentWorker : MonoBehaviour
        {
        }

        [WorkerType(CurrentWorkerType)]
        public class TestBehaviourForCurrentWorkerWithReader : MonoBehaviour
        {
            [Require] public Position.Requirable.Reader PositionReader;
        }

        [WorkerType(OtherWorkerType)]
        public class TestBehaviourForOtherWorker : MonoBehaviour
        {
        }

        [WorkerType(OtherWorkerType)]
        public class TestBehaviourForOtherWorkerWithReader : MonoBehaviour
        {
            [Require] public Position.Requirable.Reader PositionReader;
        }

        protected override void PopulateBehaviours()
        {
            TestGameObject.AddComponent<TestBehaviourForCurrentWorker>();
            TestGameObject.AddComponent<TestBehaviourForCurrentWorkerWithReader>();
            TestGameObject.AddComponent<TestBehaviourForOtherWorker>();
            TestGameObject.AddComponent<TestBehaviourForOtherWorkerWithReader>();
        }

        [Test]
        public void Does_not_cause_activation_before_EnableSpatialOSBehaviours()
        {
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourForCurrentWorker>().enabled);
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourForOtherWorker>().enabled);
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourForCurrentWorkerWithReader>().enabled);
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourForOtherWorkerWithReader>().enabled);
        }

        [Test]
        public void Does_not_alter_component_dependent_activation()
        {
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourForCurrentWorker>().enabled,
                "TestBehaviourForCurrentWorker should be enabled after EnableSpatialOSBehaviours is called" +
                " because it matches the worker type");
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourForCurrentWorkerWithReader>().enabled,
                "TestBehaviourForCurrentWorkerWithReader should not be enabled after EnableSpatialOSBehaviours is called" +
                " even if it matches the worker type because it has no components");
            Assert.IsNull(TestGameObject.GetComponent<TestBehaviourForCurrentWorkerWithReader>().PositionReader);
            ActivationManager.AddComponent(PositionComponentId);
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsTrue(TestGameObject.GetComponent<TestBehaviourForCurrentWorkerWithReader>().enabled,
                "TestBehaviourForCurrentWorkerWithReader should be enabled after EnableSpatialOSBehaviours is called" +
                " because it has the right components");
            Assert.IsNotNull(TestGameObject.GetComponent<TestBehaviourForCurrentWorkerWithReader>().PositionReader);
        }

        [Test]
        public void Activates_behaviours_only_for_current_worker()
        {
            ActivationManager.AddComponent(PositionComponentId);
            ActivationManager.EnableSpatialOSBehaviours();
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourForOtherWorker>().enabled,
                "TestBehaviourForOtherWorker should not be enabled after EnableSpatialOSBehaviours is called" +
                " because it does not match the worker type");
            Assert.IsFalse(TestGameObject.GetComponent<TestBehaviourForOtherWorkerWithReader>().enabled,
                "TestBehaviourForOtherWorkerWithReader should not be enabled after EnableSpatialOSBehaviours is called" +
                " even if it has the components because it does not match the worker type");
            Assert.IsNull(TestGameObject.GetComponent<TestBehaviourForOtherWorkerWithReader>().PositionReader);
        }
    }
}
