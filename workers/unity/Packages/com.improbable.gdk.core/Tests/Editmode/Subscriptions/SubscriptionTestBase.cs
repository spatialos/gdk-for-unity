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
        protected WorkerInWorld WorkerInWorld;
        protected SubscriptionSystem SubscriptionSystem;
        protected ILogDispatcher LogDispatcher;

        [SetUp]
        public void Setup()
        {
            // This is the minimal set required for subscriptions to work.

            var mockConnectionBuilder = new MockConnectionHandlerBuilder();
            WorkerInWorld = WorkerInWorld.CreateWorkerInWorldAsync(mockConnectionBuilder, "TestWorkerType",
                new TestLogDispatcher(), Vector3.zero).Result;

            World = WorkerInWorld.World;
            Worker = World.GetExistingSystem<WorkerSystem>();
            SubscriptionSystem = World.GetExistingSystem<SubscriptionSystem>();
            LogDispatcher = Worker.LogDispatcher;
        }

        [TearDown]
        public void TearDown()
        {
            WorkerInWorld.Dispose();
        }
    }
}
