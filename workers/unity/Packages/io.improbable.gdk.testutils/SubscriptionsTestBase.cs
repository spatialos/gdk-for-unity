using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.TestBases
{
    public class SubscriptionsTestBase
    {
        protected WorkerInWorld WorkerInWorld;
        protected SpatialOSReceiveSystem ReceiveSystem;
        protected RequireLifecycleSystem RequireLifecycleSystem;
        protected MockConnectionHandler ConnectionHandler;
        protected EntityGameObjectLinker Linker;

        [SetUp]
        public virtual void Setup()
        {
            var connectionBuilder = new MockConnectionHandlerBuilder();
            ConnectionHandler = connectionBuilder.ConnectionHandler;

            WorkerInWorld = WorkerInWorld
                .CreateWorkerInWorldAsync(connectionBuilder, "TestWorkerType", new LoggingDispatcher(), Vector3.zero)
                .Result;

            var world = WorkerInWorld.World;
            ReceiveSystem = world.GetExistingSystem<SpatialOSReceiveSystem>();
            RequireLifecycleSystem = world.GetExistingSystem<RequireLifecycleSystem>();
            Linker = new EntityGameObjectLinker(world);
        }

        [TearDown]
        public virtual void TearDown()
        {
            WorkerInWorld.Dispose();
        }

        protected GameObject CreateAndLinkGameObjectWithComponent<T>(long entityId) where T : MonoBehaviour
        {
            var gameObject = new GameObject("TestGameObject");
            var component = gameObject.AddComponent<T>();
            component.enabled = false;

            Linker.LinkGameObjectToSpatialOSEntity(new EntityId(entityId), gameObject);
            RequireLifecycleSystem.Update();

            return gameObject;
        }

        protected GameObject CreateAndLinkGameObject(long entityId, GameObject prefab, Vector3 position = default, Quaternion rotation = default)
        {
            var gameObject = Object.Instantiate(prefab, position, rotation);

            Linker.LinkGameObjectToSpatialOSEntity(new EntityId(entityId), gameObject);
            RequireLifecycleSystem.Update();

            return gameObject;
        }
    }
}
