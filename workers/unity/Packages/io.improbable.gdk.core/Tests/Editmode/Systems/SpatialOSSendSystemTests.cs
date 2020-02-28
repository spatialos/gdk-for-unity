using System;
using Improbable.Gdk.Core.CodegenAdapters;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests.Systems
{
    [TestFixture]
    public class SpatialOSSendSystemTests
    {
        internal const uint TestComponentId = 1;

        private WorkerInWorld worker;
        private ComponentSendSystem sendSystem;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var mockConnectionBuilder = new MockConnectionHandlerBuilder();
            worker = WorkerInWorld.CreateWorkerInWorldAsync(mockConnectionBuilder, "TestWorkerType",
                new TestLogDispatcher(), Vector3.zero).Result;

            var world = worker.World;

            sendSystem = world.GetExistingSystem<ComponentSendSystem>();

            var testHandler = new TestComponentReplicationHandler();
            sendSystem.AddComponentReplicator(testHandler);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            worker.Dispose();
        }
    }

    [DisableAutoRegister]
    public class TestComponentReplicationHandler : IComponentReplicationHandler
    {
        public uint ComponentId => SpatialOSSendSystemTests.TestComponentId;

        public EntityQueryDesc ComponentUpdateQuery => new EntityQueryDesc
        {
            All = Array.Empty<ComponentType>(),
            Any = Array.Empty<ComponentType>(),
            None = Array.Empty<ComponentType>(),
        };

        public void SendUpdates(NativeArray<ArchetypeChunk> chunkArray, ComponentSystemBase system,
            EntityManager entityManager, ComponentUpdateSystem componentUpdateSystem)
        {
            throw new NotImplementedException();
        }
    }
}
