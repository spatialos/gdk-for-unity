using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    public class SubscriptionTestBase
    {
        protected World World;
        protected WorkerSystem Worker;
        protected SubscriptionSystem SubscriptionSystem;
        protected ILogDispatcher LogDispatcher;

        [SetUp]
        public void Setup()
        {
            // This is the minimal set required for subscriptions to work.
            // TODO: Look into untangling these!
            World = new World("test-world");
            Worker = World.CreateManager<WorkerSystem>(new MockConnectionHandler(), null, new TestLogDispatcher(), "TestWorkerType", Vector3.zero);
            World.CreateManager<SpatialOSReceiveSystem>();
            World.GetOrCreateManager<ComponentConstraintsCallbackSystem>();
            SubscriptionSystem = World.CreateManager<SubscriptionSystem>();

            LogDispatcher = Worker.LogDispatcher;
        }

        [TearDown]
        public void TearDown()
        {
            World.Dispose();
        }
    }
}
