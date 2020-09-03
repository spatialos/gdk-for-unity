using Improbable.Gdk.Core;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests
{
    [TestFixture]
    public class WorkerFlagLoggingTests : MockBase
    {
        private const string ProtocolLoggingFlag = "protocol_logging_enabled";
        private const long EntityId = 1;

        private static EntityTemplate GetTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot());
            return template;
        }

        [Test]
        public void Logging_disabled_when_ProtocolLoggingFlag_does_not_exist()
        {
            World
                .Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world =>
                {
                    Assert.IsTrue(string.IsNullOrWhiteSpace(world.GetSystem<WorkerSystem>()
                        .GetWorkerFlag(ProtocolLoggingFlag)));
                    Assert.IsFalse(world.Connection.LoggingEnabled);
                });
        }

        [Test]
        public void Logging_disabled_when_ProtocolLoggingFlag_is_empty()
        {
            World
                .Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world =>
                {
                    world.Connection.SetWorkerFlag((ProtocolLoggingFlag, ""));
                })
                .Step(world =>
                {
                    Assert.IsFalse(world.Connection.LoggingEnabled);
                });
        }

        [Test]
        public void Logging_disabled_when_ProtocolLoggingFlag_does_not_contain_workerId()
        {
            World
                .Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world =>
                {
                    world.Connection.SetWorkerFlag((ProtocolLoggingFlag, $"{world.Connection.GetWorkerId()}1,89j0345t"));
                })
                .Step(world =>
                {
                    Assert.IsFalse(world.Connection.LoggingEnabled);
                });
        }

        [Test]
        public void Logging_enabled_when_ProtocolLoggingFlag_contains_workerId()
        {
            World
                .Step(world => world.Connection.CreateEntity(EntityId, GetTemplate()))
                .Step(world =>
                {
                    world.Connection.SetWorkerFlag((ProtocolLoggingFlag, $"{world.Connection.GetWorkerId()}1,{world.Connection.GetWorkerId()},{world.Connection.GetWorkerId()}2"));
                })
                .Step(world =>
                {
                    Assert.IsTrue(world.Connection.LoggingEnabled);
                });
        }
    }
}
