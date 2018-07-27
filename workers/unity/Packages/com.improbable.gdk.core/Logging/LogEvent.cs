using System;
using System.Collections.Generic;
using System.Text;

namespace Improbable.Gdk.Core
{
    public struct LogEvent
    {
        public string Message;
        public Dictionary<string, object> Data;

        public Exception Exception;
        public UnityEngine.Object Context;

        public LogEvent(string message)
        {
            Message = message;
            Data = new Dictionary<string, object>();
            Exception = null;
            Context = null;
        }

        public LogEvent WithField(string key, object value)
        {
            Data.Add(key, value);
            return this;
        }

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
                
                builder.AppendLine("Log event context:");

                foreach (var entry in Data)
                {
                    builder.AppendLine($"'{entry.Key}': '{entry.Value}'");
                }
            }

            return builder.ToString();
        }
    }
}
