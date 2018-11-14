using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.TestUtils
{
    /// <summary>
    ///     A ILogDispatcher implementation designed to be used in testing. This replaces the LogAssert approach with
    ///     a more specialised one.
    ///
    ///     The expected usage is to use EnterExpectingScope() with a using block. This methods returns a Disposable
    ///     object which you can mark logs as expected. When the object is disposed - it will assert against any logs.
    /// </summary>
    public class TestLogDispatcher : ILogDispatcher
    {
        private ExpectingScope currentExpectingScope;

        public Connection Connection { get; set; }
        public string WorkerType { get; set; }

        public void HandleLog(LogType type, LogEvent logEvent)
        {
            if (currentExpectingScope != null)
            {
                currentExpectingScope.CheckIncomingLog(type, logEvent);
            }
            else if (type == LogType.Error || type == LogType.Exception)
            {
                Assert.Fail($"Encountered error log outside of an expecting scope: [{type}] - {logEvent}");
            }
        }

        /// <summary>
        ///     Creates and returns an disposable ExpectingScope object. This is intended to be used with a using block.
        /// </summary>
        /// <returns>An ExpectingScope instance.</returns>
        /// <exception cref="InvalidOperationException">
        ///     Throws if you already have an un-disposed ExpectingScope from this logger.
        /// </exception>
        public ExpectingScope EnterExpectingScope()
        {
            if (currentExpectingScope != null)
            {
                throw new InvalidOperationException(
                    "Cannot enter an ExpectingScope while there is an outstanding ExpectingScope");
            }

            currentExpectingScope = new ExpectingScope(this);
            return currentExpectingScope;
        }

        public void ExitExpectingScope()
        {
            if (currentExpectingScope == null)
            {
                throw new InvalidOperationException(
                    "Cannot exit an ExpectingScope if you have not entered one!");
            }

            currentExpectingScope.Dispose();
        }

        public void Dispose()
        {
            currentExpectingScope = null;
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
                if (expectedLogs.Count > 0 && expectedLogs.Peek().DoesMatchLog(type, logEvent))
                {
                    expectedLogs.Dequeue();
                    return;
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
