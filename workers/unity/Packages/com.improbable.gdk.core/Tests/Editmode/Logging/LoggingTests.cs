using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Improbable.Gdk.Core.EditmodeTests.Logging
{
    [TestFixture]
    public class LoggingDispatcherTests
    {
        private LoggingDispatcher logDispatcher;

        [SetUp]
        public void SetUp()
        {
            logDispatcher = new LoggingDispatcher();
        }

        [Test]
        [TestCase(LogType.Error)]
        [TestCase(LogType.Assert)]
        [TestCase(LogType.Warning)]
        [TestCase(LogType.Log)]
        public void ForwardsCorrectMessageTypes(LogType logType)
        {
            LogAssert.Expect(logType, $"Message with type: {logType}");
            logDispatcher.HandleLog(logType, new LogEvent($"Message with type: {logType}"));
        }

        [Test]
        public void ForwardsExceptionTypes()
        {
            LogAssert.Expect(LogType.Exception, "Exception: Test Exception");
            logDispatcher.HandleLog(LogType.Exception, new LogEvent()
                .WithException(new Exception("Test Exception")));
        }

        [Test]
        public void CreatesExceptionIfNotSpecified()
        {
            LogAssert.Expect(LogType.Exception, "Exception: Test Exception");
            logDispatcher.HandleLog(LogType.Exception, new LogEvent("Test Exception"));
        }
    }
}
