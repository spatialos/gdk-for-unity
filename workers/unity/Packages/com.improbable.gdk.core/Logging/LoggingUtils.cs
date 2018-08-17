using System.Collections.Generic;
using Improbable.Worker;

namespace Improbable.Gdk.Core
{
    public class LoggingUtils
    {
        public const string UnknownLogger = "UnknownLogger";
        public const string EntityId = "EntityId";
        public const string LoggerName = "LoggerName";

        public static EntityId? ExtractEntityId(Dictionary<string, object> data)
        {
            object dataEntityId;
            if (data.TryGetValue(EntityId, out dataEntityId))
            {
                if (dataEntityId is EntityId)
                {
                    return (EntityId) dataEntityId;
                }

                if (dataEntityId is long)
                {
                    return new EntityId((long) dataEntityId);
                }
            }

            return null;
        }

        public static string ExtractLoggerName(Dictionary<string, object> data)
        {
            if (!data.ContainsKey(LoggerName))
            {
                return UnknownLogger;
            }

            var loggerName = data[LoggerName];
            if (loggerName is string name)
            {
                return name;
            }

            return UnknownLogger;
        }
    }
}
