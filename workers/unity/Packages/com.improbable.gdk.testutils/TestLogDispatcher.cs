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
        private static string AttemptToAccessConnectionError =
            $"Cannot access or set the Connection in the {nameof(TestLogDispatcher)}";

        private Queue<ExpectedLog> expectedLogs = new Queue<ExpectedLog>();
        private bool unexpectedLogsReceived;

        // The connection will never be valid - so any attempt to get or set will throw.
        public Connection Connection
        {
            get => throw new InvalidOperationException(AttemptToAccessConnectionError);
            set => throw new InvalidOperationException(AttemptToAccessConnectionError);
        }

        public void HandleLog(LogType type, LogEvent logEvent)
        {
            if (expectedLogs.Count == 0)
            {
                unexpectedLogsReceived |= type == LogType.Error || type == LogType.Exception;
                return;
            }

            if (expectedLogs.Peek().DoesMatchLog(type, logEvent))
            {
                expectedLogs.Dequeue();
            }
            else
            {
                unexpectedLogsReceived |= type == LogType.Error || type == LogType.Exception;
            }
        }

        /// <summary>
        ///     Specifies that you are expecting a log message of a certain structure.
        /// </summary>
        /// <param name="expectedLog">A struct which defines the structure of the expected log message.</param>
        public void Expect(ExpectedLog expectedLog)
        {
            expectedLogs.Enqueue(expectedLog);
        }

        /// <summary>
        ///     Assert against any queued up expected logs and asserts against unexpected error or exceptions.
        /// </summary>
        public void AssertAgainstExpectedLogs()
        {
            Assert.IsFalse(unexpectedLogsReceived, "Received unexpected errors or exceptions.");
            Assert.AreEqual(0, expectedLogs.Count, "Did not receive all expected logs");
        }

        /// <summary>
        ///     Clears the queue of expected logs. This should be called in after AssertAgainstExpectedLogs() to clean
        ///     up any leftover expected logs. A good place to call this is in a [TearDown] fixture.
        /// </summary>
        public void ClearExpectedLogs()
        {
            expectedLogs.Clear();
        }

        public void Dispose()
        {
        }
    }

    /// <summary>
    ///     A struct which defines the shape of an expected log.
    /// </summary>
    public struct ExpectedLog
    {
        public LogType Type;
        public string[] DataKeys;

        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="type">The LogType of the expected log. Warning, Error, etc.</param>
        /// <param name="dataKeys">
        ///     The set of string keys expected in the log message.
        ///     This corresponds to the WithField method on a LogEvent.
        /// </param>
        public ExpectedLog(LogType type, params string[] dataKeys)
        {
            Type = type;
            DataKeys = dataKeys;
        }

        internal bool DoesMatchLog(LogType type, LogEvent logEvent)
        {
            if (type != Type)
            {
                return false;
            }

            return !logEvent.Data.Keys.Except(DataKeys).Any();
        }
    }
}
