using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Subscriptions
{
    [TestFixture]
    public class WorkerTypeCriteriaTests
    {
        private const string WorkerType = "TestWorkerType";
        private const long EntityId = 100;

        private WorkerInWorld workerInWorld;
        private SpatialOSReceiveSystem receiveSystem;
        private RequireLifecycleSystem requireLifecycleSystem;

        private MockConnectionHandler connectionHandler;
        private EntityGameObjectLinker linker;

        private GameObject createdGameObject;

        [SetUp]
        public void Setup()
        {
            var connectionBuilder = new MockConnectionHandlerBuilder();
            connectionHandler = connectionBuilder.ConnectionHandler;

            workerInWorld = WorkerInWorld
                .CreateWorkerInWorldAsync(connectionBuilder, WorkerType, new LoggingDispatcher(), Vector3.zero)
                .Result;

            var world = workerInWorld.World;
            receiveSystem = world.GetExistingSystem<SpatialOSReceiveSystem>();
            requireLifecycleSystem = world.GetExistingSystem<RequireLifecycleSystem>();
            linker = new EntityGameObjectLinker(world);

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            connectionHandler.CreateEntity(EntityId, template);
            receiveSystem.Update();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(createdGameObject);
            workerInWorld.Dispose();
        }

        [Test]
        public void Monobehaviour_is_enabled_with_matching_WorkerType()
        {
            var go = CreateAndLinkGameObjectWithComponent<MatchingWorkerType>(EntityId);
            var behaviour = go.GetComponent<MatchingWorkerType>();

            Assert.IsTrue(behaviour.enabled);
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType()
        {
            var go = CreateAndLinkGameObjectWithComponent<NonMatchingWorkerType>(EntityId);
            var behaviour = go.GetComponent<NonMatchingWorkerType>();

            Assert.IsFalse(behaviour.enabled);
        }

        [Test]
        public void Monobehaviour_is_disabled_with_matching_WorkerType_but_unsatisfied_requireable()
        {
            var go = CreateAndLinkGameObjectWithComponent<MatchingWorkerTypeWithRequireable>(EntityId);
            var behaviour = go.GetComponent<MatchingWorkerTypeWithRequireable>();

            Assert.IsFalse(behaviour.enabled);
            Assert.IsNull(behaviour.Writer);
        }

        [Test]
        public void Monobehaviour_is_enabled_with_matching_WorkerType_and_satisfied_requireable()
        {
            connectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            receiveSystem.Update();

            var go = CreateAndLinkGameObjectWithComponent<MatchingWorkerTypeWithRequireable>(EntityId);
            var behaviour = go.GetComponent<MatchingWorkerTypeWithRequireable>();

            Assert.IsTrue(behaviour.enabled);
            Assert.IsNotNull(behaviour.Writer);
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType_and_unsatisfied_requireable()
        {
            var go = CreateAndLinkGameObjectWithComponent<NonMatchingWorkerTypeWithRequireable>(EntityId);
            var behaviour = go.GetComponent<NonMatchingWorkerTypeWithRequireable>();

            Assert.IsFalse(behaviour.enabled);
            Assert.IsNull(behaviour.Writer);
        }

        [Test]
        public void Monobehaviour_is_disabled_with_non_matching_WorkerType_and_satisfied_requireable()
        {
            connectionHandler.ChangeAuthority(EntityId, Position.ComponentId, Authority.Authoritative);
            receiveSystem.Update();

            var go = CreateAndLinkGameObjectWithComponent<NonMatchingWorkerTypeWithRequireable>(EntityId);
            var behaviour = go.GetComponent<NonMatchingWorkerTypeWithRequireable>();

            Assert.IsFalse(behaviour.enabled);
            Assert.IsNull(behaviour.Writer);
        }

        private GameObject CreateAndLinkGameObjectWithComponent<T>(long entityId) where T : MonoBehaviour
        {
            var gameObject = new GameObject("TestGameObject");
            createdGameObject = gameObject;
            gameObject.AddComponent<T>();
            gameObject.GetComponent<T>().enabled = false;

            linker.LinkGameObjectToSpatialOSEntity(new EntityId(entityId), gameObject);
            requireLifecycleSystem.Update();

            return gameObject;
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
