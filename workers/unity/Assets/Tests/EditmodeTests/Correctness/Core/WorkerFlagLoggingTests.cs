using Improbable.Gdk.Core;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests
{
    [TestFixture]
    public class WorkerFlagLoggingTests : MockBase
    {
        private const string ProtocolLoggingFlag = "protocol_logging_enabled";

        [Test]
        public void ProtocolLogging_disabled_when_ProtocolLoggingFlag_does_not_exist()
        {
            World
                .Step(world =>
                {
                    Assert.IsTrue(string.IsNullOrWhiteSpace(world.GetSystem<WorkerSystem>()
                        .GetWorkerFlag(ProtocolLoggingFlag)));
                    Assert.IsFalse(world.ProtocolLog.Enabled);
                });
        }

        [Test]
        public void ProtocolLogging_disabled_when_ProtocolLoggingFlag_is_empty()
        {
            World
                .Step(world =>
                {
                    world.Connection.SetWorkerFlag((ProtocolLoggingFlag, ""));
                })
                .Step(world =>
                {
                    Assert.IsFalse(world.ProtocolLog.Enabled);
                });
        }

        [Test]
        public void ProtocolLogging_disabled_when_ProtocolLoggingFlag_does_not_contain_workerId()
        {
            World
                .Step(world =>
                {
                    world.Connection.SetWorkerFlag((ProtocolLoggingFlag, $"{world.Connection.GetWorkerId()}1,89j0345t"));
                })
                .Step(world =>
                {
                    Assert.IsFalse(world.ProtocolLog.Enabled);
                });
        }

        [Test]
        public void Logging_enabled_when_ProtocolLoggingFlag_contains_workerId()
        {
            World
                .Step(world =>
                {
                    world.Connection.SetWorkerFlag((ProtocolLoggingFlag, $"{world.Connection.GetWorkerId()}1,{world.Connection.GetWorkerId()},{world.Connection.GetWorkerId()}2"));
                })
                .Step(world =>
                {
                    Assert.IsTrue(world.ProtocolLog.Enabled && world.Connection.LoggingEnabled);
                });
        }
    }
}
