using System;
using System.Collections.Generic;
using System.Text;

namespace Improbable.Gdk.Core
{
    /// <summary>
    ///     Represents a single log. Can contain data used for structured logging.
    /// </summary>
    public struct LogEvent
    {
        /// <summary>
        ///     The main content of the log.
        /// </summary>
        public readonly string Message;

        /// <summary>
        ///     The data used for structured logging.
        /// </summary>
        public readonly Dictionary<string, object> Data;

        /// <summary>
        ///     Optional context object used with Unity logging.
        /// </summary>
        public UnityEngine.Object Context;

        /// <summary>
        ///     An exception if the LogEvent is associated with an exception.
        /// </summary>
        public Exception Exception;

        /// <summary>
        ///     Constructor for the log event
        /// </summary>
        /// <param name="message">The log content.</param>
        public LogEvent(string message)
        {
            Message = message;
            Data = new Dictionary<string, object>();
            Exception = null;
            Context = null;
        }

        /// <summary>
        ///     Sets additional information to be displayed with the log message.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Itself</returns>
        public LogEvent WithField(string key, object value)
        {
            Data[key] = value;
            return this;
        }

        /// <summary>
        ///     Adds a context object to be passed as the second parameter into
        ///     <see cref="UnityEngine.Debug.Log(object, UnityEngine.Object)" />
        /// </summary>
        /// <param name="context">The context object</param>
        /// <returns>Itself</returns>
        public LogEvent WithContext(UnityEngine.Object context)
        {
            Context = context;
            return this;
        }

        /// <summary>
        ///     Associates an exception to the LogEvent.
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <returns>Itself</returns>
        public LogEvent WithException(Exception exception)
        {
            Exception = exception;
            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Data.TryGetValue(LoggingUtils.WorkerType, out var workerType))
            {
                builder.Append($"[{workerType}] ");
            }

            builder.Append(Message);

            foreach (var entry in Data)
            {
                builder.AppendLine();
                builder.Append($"'{entry.Key}': '{entry.Value}'");
            }

            return builder.ToString();
        }
    }
}
