using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    [TestFixture]
    public class WorkerTypeCriteriaTests : SubscriptionsTestBase
    {
        private const string WorkerType = "TestWorkerType";
        private const long EntityId = 100;

        private GameObject createdGameObject;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            ConnectionHandler.CreateEntity(EntityId, template);
            ReceiveSystem.Update();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            Object.DestroyImmediate(createdGameObject);
        }

        [Test]
        public void Monobehaviour_is_enabled_with_matching_WorkerType()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<MatchingWorkerType>(EntityId);
            var behaviour = createdGameObject.GetComponent<MatchingWorkerType>();

            Assert.IsTrue(behaviour.enabled);
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<NonMatchingWorkerType>(EntityId);
            var behaviour = createdGameObject.GetComponent<NonMatchingWorkerType>();

            Assert.IsFalse(behaviour.enabled);
        }

        [Test]
        public void Monobehaviour_is_disabled_with_matching_WorkerType_but_unsatisfied_requireable()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<MatchingWorkerTypeWithRequireable>(EntityId);
            var behaviour = createdGameObject.GetComponent<MatchingWorkerTypeWithRequireable>();

            Assert.IsFalse(behaviour.enabled);
            Assert.IsNull(behaviour.Writer);
        }

        [Test]
        public void Monobehaviour_is_enabled_with_matching_WorkerType_and_satisfied_requireable()
        {
            ConnectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            ReceiveSystem.Update();

            createdGameObject = CreateAndLinkGameObjectWithComponent<MatchingWorkerTypeWithRequireable>(EntityId);
            var behaviour = createdGameObject.GetComponent<MatchingWorkerTypeWithRequireable>();

            Assert.IsTrue(behaviour.enabled);
            Assert.IsNotNull(behaviour.Writer);
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType_and_unsatisfied_requireable()
        {
            createdGameObject = CreateAndLinkGameObjectWithComponent<NonMatchingWorkerTypeWithRequireable>(EntityId);
            var behaviour = createdGameObject.GetComponent<NonMatchingWorkerTypeWithRequireable>();

            Assert.IsFalse(behaviour.enabled);
            Assert.IsNull(behaviour.Writer);
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType_and_satisfied_requireable()
        {
            ConnectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            ReceiveSystem.Update();

            createdGameObject = CreateAndLinkGameObjectWithComponent<NonMatchingWorkerTypeWithRequireable>(EntityId);
            var behaviour = createdGameObject.GetComponent<NonMatchingWorkerTypeWithRequireable>();

            Assert.IsFalse(behaviour.enabled);
            Assert.IsNull(behaviour.Writer);
        }

#pragma warning disable 649

        [WorkerType(WorkerType)]
        private class MatchingWorkerType : MonoBehaviour
        {
        }

        [WorkerType("NotMyWorker")]
        private class NonMatchingWorkerType : MonoBehaviour
        {
        }

        [WorkerType(WorkerType)]
        private class MatchingWorkerTypeWithRequireable : MonoBehaviour
        {
            [Require] public PositionWriter Writer;
        }

        [WorkerType("NotMyWorker")]
        private class NonMatchingWorkerTypeWithRequireable : MonoBehaviour
        {
            [Require] public PositionWriter Writer;
        }

#pragma warning restore 649
    }
}
