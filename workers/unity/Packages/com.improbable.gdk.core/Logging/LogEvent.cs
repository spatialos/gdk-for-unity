using System;
using System.Collections.Generic;
using System.Text;

namespace Improbable.Gdk.Core
{
    public struct LogEvent
    {
        public string Message;
        public Dictionary<string, object> Data;
        public UnityEngine.Object Context;
        public Exception Exception;

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
        /// <returns></returns>
        public LogEvent WithField(string key, object value)
        {
            Data.Add(key, value);
            return this;
        }

        /// <summary>
        ///     Adds a context object to be passed as the second parameter into
        ///     <see cref="UnityEngine.Debug.Log(object, UnityEngine.Object)" />
        /// </summary>
        /// <param name="context">The context object</param>
        /// <returns>itself</returns>
        public LogEvent WithContext(UnityEngine.Object context)
        {
            Context = context;
            return this;
        }

        public LogEvent WithException(Exception exception)
        {
            Exception = exception;
            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(Message);

            if (Data.Count > 0)
            {
                builder.AppendLine();

                builder.AppendLine("Log event data:");

                foreach (var entry in Data)
                {
                    builder.AppendLine($"'{entry.Key}': '{entry.Value}'");
                }
            }

            return builder.ToString();
        }
    }
}
