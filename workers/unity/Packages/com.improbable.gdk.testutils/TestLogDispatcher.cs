using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Worker.Core;
using UnityEngine;
using Assert = NUnit.Framework.Assert;

namespace Improbable.Gdk.TestUtils
{
    /// <summary>
    ///     A ILogDispatcher implementation designed to be used in testing. This replaces the LogAssert approach with
    ///     a more specialised one.
    ///
    ///     Using the TestLogDispatcher allows you to expect a certain structure of message and asserts against it. Any
    ///     unexpected error or exceptions will cause an assertion failure.
    ///
    ///     You can expect against a series of logs which are asserted against in order. I.e. - out of order messages
    ///     will cause an assertion failure.
    /// </summary>
    public class TestLogDispatcher : ILogDispatcher
    {
        private static readonly string AttemptToAccessConnectionError =
            $"Cannot access or set the Connection in the {nameof(TestLogDispatcher)}";

        private ExpectingScope currentExpectingScope;

        // The connection will never be valid - so any attempt to get or set will throw.
        public Connection Connection
        {
            get => throw new InvalidOperationException(AttemptToAccessConnectionError);
            set => throw new InvalidOperationException(AttemptToAccessConnectionError);
        }

        public void HandleLog(LogType type, LogEvent logEvent)
        {
            if (currentExpectingScope != null)
            {
                currentExpectingScope.CheckIncomingLog(type, logEvent);
                return;
            }

            if (type == LogType.Error || type == LogType.Exception)
            {
                Assert.Fail($"Encountered error log outside of an expecting scope: [{type}] - {logEvent}");
            }
        }

        public ExpectingScope EnterExpectingScope()
        {
            if (currentExpectingScope != null)
            {
                throw new InvalidOperationException("Cannot ");
            }

            currentExpectingScope = new ExpectingScope(this);
            return currentExpectingScope;
        }

        public void Dispose()
        {
            if (currentExpectingScope != null)
            {
                throw new InvalidOperationException(
                    "Cannot Dispose a TestLogDispatcher while there is an outstanding ExpectingScope");
            }
        }

        public class ExpectingScope : IDisposable
        {
            private readonly Queue<LogStructure> expectedLogs = new Queue<LogStructure>();
            private readonly Queue<(LogType, LogEvent)> unexpectedLogs = new Queue<(LogType, LogEvent)>();

            private TestLogDispatcher dispatcher;

            internal ExpectingScope(TestLogDispatcher dispatcher)
            {
                this.dispatcher = dispatcher;
            }

            public void Expect(LogType type, params string[] dataKeys)
            {
                expectedLogs.Enqueue(new LogStructure(type, dataKeys));
            }

            public void Dispose()
            {
                dispatcher.currentExpectingScope = null;

                Assert.AreEqual(0, expectedLogs.Count);

                var unexpectedLogsString = string.Join("\n", unexpectedLogs.Select(log =>
                {
                    var (logType, logEvent) = log;
                    return $"[{logType}] - {logEvent}";
                }));
                Assert.AreEqual(0, unexpectedLogs.Count,
                    $"Received unexpected errors or exceptions : {unexpectedLogsString}");
            }

            internal void CheckIncomingLog(LogType type, LogEvent logEvent)
            {
                if (expectedLogs.Count > 0)
                {
                    if (expectedLogs.Peek().DoesMatchLog(type, logEvent))
                    {
                        expectedLogs.Dequeue();
                        return;
                    }
                }

                if (type == LogType.Error || type == LogType.Exception)
                {
                    unexpectedLogs.Enqueue((type, logEvent));
                }
            }

            /// <summary>
            ///     A struct which defines the shape of an expected log.
            /// </summary>
            private struct LogStructure
            {
                public LogType Type;
                public string[] DataKeys;

                public LogStructure(LogType type, params string[] dataKeys)
                {
                    Type = type;
                    DataKeys = dataKeys;
                }

                public bool DoesMatchLog(LogType type, LogEvent logEvent)
                {
                    if (type != Type)
                    {
                        return false;
                    }

                    return !logEvent.Data.Keys.Except(DataKeys).Any();
                }
            }
        }
    }
}
