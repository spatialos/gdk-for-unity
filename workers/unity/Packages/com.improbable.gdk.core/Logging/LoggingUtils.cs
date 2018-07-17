using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public class LoggingUtils
    {
        public const string UnknownLogger = "UnknownLogger";
        public const string EntityId = "EntityId";
        public const string LoggerName = "LoggerName";

        public static Collections.Option<EntityId> ExtractEntityId(Dictionary<string, object> data)
        {
            object dataEntityId;
            if (data.TryGetValue(EntityId, out dataEntityId))
            {
                if (dataEntityId is EntityId)
                {
                    return (EntityId)dataEntityId;
                }

                if (dataEntityId is long)
                {
                    return new EntityId((long)dataEntityId);
                }
            }

            return new Collections.Option<EntityId>();
        }

        public static string ExtractLoggerName(Dictionary<string, object> data)
        {
            if (data.ContainsKey(LoggerName))
            {
                var loggerName = data[LoggerName];
                var name = loggerName as string;
                if (name != null)
                {
                    return name;
                }
            }

            return UnknownLogger;
        }
    }
}
