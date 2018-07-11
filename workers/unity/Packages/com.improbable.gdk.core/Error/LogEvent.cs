using System.Collections.Generic;
using System.Text;

namespace Improbable.Gdk.Core
{
    public struct LogEvent
    {
        public string Message;
        public Dictionary<string, object> Data;

        public LogEvent(string message)
        {
            Message = message;
            Data = new Dictionary<string, object>();
        }

        public LogEvent WithField(string key, object value)
        {
            Data.Add(key, value);
            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine(Message);

            if (Data.Count > 0)
            {
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
