using System;
using System.Reflection;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.Subscriptions;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.PlaymodeTests.Subscriptions
{
    public class SubscriptionAggregateCouplingTests
    {
        private MockConnectionHandler connectionHandler;
        private WorkerInWorld workerInWorld;
        private EntityGameObjectLinker linker;

        private SpatialOSReceiveSystem receiveSystem;
        private RequireLifecycleSystem requireLifecycleSystem;

        private const string WorkerType = "TestWorkerType";

        [SetUp]
        public void Setup()
        {
            var logDispatcher = new IgnoreLogsDispatcher();

            var connectionBuilder = new MockConnectionHandlerBuilder();
            connectionHandler = connectionBuilder.ConnectionHandler;
            workerInWorld = WorkerInWorld
                .CreateWorkerInWorldAsync(connectionBuilder, WorkerType, logDispatcher, Vector3.zero)
                .Result;
            receiveSystem = workerInWorld.World.GetExistingSystem<SpatialOSReceiveSystem>();
            requireLifecycleSystem = workerInWorld.World.GetExistingSystem<RequireLifecycleSystem>();

            var goInitSystem = workerInWorld.World
                .CreateSystem<GameObjectInitializationSystem>(
                    new GameObjectCreatorFromMetadata(WorkerType, Vector3.zero, logDispatcher), null);
            linker = goInitSystem.Linker;
        }

        [TearDown]
        public void TearDown()
        {
            workerInWorld.Dispose();
        }

        // Workaround due to the lack of support for generic MonoBehaviours. Instead have a concrete implementation
        // that we use. Little bit more boilerplate, but still less than manually writing _everything_.
        // Alternative would be to use Reflection.Emit, but this would fail on IL2CPP platforms (notably iOS).
        private static class TestMonoBehaviours
        {
            public class Base<T> : MonoBehaviour
            {
                [Require] public T Requireable;
                [Require] public PositionReader Reader;
            }

            public class EntityId : Base<Improbable.Gdk.Core.EntityId>
            {
            }

            public class Entity : Base<Unity.Entities.Entity>
            {
            }

            public class LinkedGameObjectMap : Base<Improbable.Gdk.Subscriptions.LinkedGameObjectMap>
            {
            }

            public class LogDispatcher : Base<ILogDispatcher>
            {
            }

            public class WorkerFlagReader : Base<Improbable.Gdk.Core.WorkerFlagReader>
            {
            }

            public class WorkerId : Base<Improbable.Gdk.Subscriptions.WorkerId>
            {
            }

            public class WorldCommandSender : Base<Improbable.Gdk.Core.WorldCommandSender>
            {
            }

            public class World : Base<Unity.Entities.World>
            {
            }
        }

        [TestCase(typeof(TestMonoBehaviours.EntityId))]
        [TestCase(typeof(TestMonoBehaviours.Entity))]
        [TestCase(typeof(TestMonoBehaviours.LinkedGameObjectMap))]
        [TestCase(typeof(TestMonoBehaviours.LogDispatcher))]
        [TestCase(typeof(TestMonoBehaviours.WorkerFlagReader))]
        [TestCase(typeof(TestMonoBehaviours.WorkerId))]
        [TestCase(typeof(TestMonoBehaviours.WorldCommandSender))]
        [TestCase(typeof(TestMonoBehaviours.World))]
        public void DifferentAggregateSubscriptions_should_not_couple_together(Type requireableType)
        {
            var method = typeof(SubscriptionAggregateCouplingTests)
                .GetMethod("DifferentAggregateSubscriptions_should_not_couple_together_impl",
                    BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(method);

            method.MakeGenericMethod(requireableType).Invoke(this, new object[] { });
        }

        // This tests a bug we encountered where two different GameObjects that both had subscriptions that
        // were cached incorrectly would cause the subscriptions to become coupled.
        private void DifferentAggregateSubscriptions_should_not_couple_together_impl<T>() where T : MonoBehaviour
        {
            var gameObject1 = CreateAndLinkGameObject<T>(1);
            var gameObject2 = CreateAndLinkGameObject<T>(2);

            RemoveAndUnlinkGameObject(1, gameObject1);
            RemoveAndUnlinkGameObject(2, gameObject2);

            var gameObject3 = CreateAndLinkGameObject<T>(3);
            var gameObject4 = CreateAndLinkGameObject<T>(4);

            RemoveAndUnlinkGameObject(3, gameObject3);

            Assert.IsTrue(gameObject4.GetComponent<T>().enabled, "gameObject4.GetComponent<T>().enabled");
        }

        private GameObject CreateAndLinkGameObject<T>(long entityId) where T : MonoBehaviour
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "worker");
            connectionHandler.CreateEntity(entityId, template);
            receiveSystem.Update();

            var gameObject = new GameObject("TestGameObject");
            gameObject.AddComponent<T>();
            gameObject.GetComponent<T>().enabled = false;

            linker.LinkGameObjectToSpatialOSEntity(new EntityId(entityId), gameObject);
            requireLifecycleSystem.Update();

            return gameObject;
        }

        private void RemoveAndUnlinkGameObject(long entityId, GameObject gameObject)
        {
            connectionHandler.RemoveComponent(entityId, Position.ComponentId);
            connectionHandler.RemoveEntity(entityId);
            receiveSystem.Update();
            requireLifecycleSystem.Update();

            Object.DestroyImmediate(gameObject);
        }

        // Drop all logs, we don't care about them for this test.
        private class IgnoreLogsDispatcher : ILogDispatcher
        {
            public void Dispose()
            {
            }

            public Core.Worker Worker { get; set; }
            public string WorkerType { get; set; }

            public void HandleLog(LogType type, LogEvent logEvent)
            {
            }
        }
    }
}
