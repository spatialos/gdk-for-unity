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

        private const string TestWorkerType = "TestWorker";
        private const uint UnknownComponentId = 0;

        private World world;
        private ComponentSendSystem sendSystem;

        private ILogDispatcher logDispatcher;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            world = new World("test-world");
            logDispatcher = new TestLogDispatcher();
            world.CreateManager<WorkerSystem>(new MockConnectionHandler(), null, logDispatcher, TestWorkerType, Vector3.zero);

            sendSystem = world.GetOrCreateManager<ComponentSendSystem>();

            var testHandler = new TestComponentReplicationHandler();
            sendSystem.AddComponentReplicator(testHandler);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            logDispatcher.Dispose();
            world.Dispose();
        }

        [Test]
        public void TryRegisterCustomReplicationSystem_should_return_false_if_there_isnt_an_enabled_default_replicator()
        {
            Assert.IsFalse(sendSystem.TryRegisterCustomReplicationSystem(UnknownComponentId));
        }

        [Test]
        public void TryRegisterCustomReplicationSystem_should_return_true_the_first_time_for_a_component_id()
        {
            Assert.IsTrue(sendSystem.TryRegisterCustomReplicationSystem(TestComponentId));
            Assert.IsFalse(sendSystem.TryRegisterCustomReplicationSystem(TestComponentId));
        }
    }

    [DisableAutoRegister]
    public class TestComponentReplicationHandler : IComponentReplicationHandler
    {
        public uint ComponentId => SpatialOSSendSystemTests.TestComponentId;

        public EntityArchetypeQuery ComponentUpdateQuery => new EntityArchetypeQuery
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
