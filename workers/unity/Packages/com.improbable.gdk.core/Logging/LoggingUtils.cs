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
            if (!data.TryGetValue(EntityId, out var dataEntityId))
            {
                return new Collections.Option<EntityId>();
            }

            switch (dataEntityId)
            {
                case EntityId asEntityId:
                    return asEntityId;
                case long asLong:
                    return new EntityId(asLong);
            }

            return new Collections.Option<EntityId>();
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
