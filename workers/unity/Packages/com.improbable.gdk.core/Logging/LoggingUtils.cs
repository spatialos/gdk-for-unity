using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public class LoggingUtils
    {
        public const string UnknownLogger = "UnknownLogger";
        public const string EntityId = "EntityId";
        public const string LoggerName = "LoggerName";
        public const string Component = "Component";

        public static Collections.Option<EntityId> ExtractEntityId(Dictionary<string, object> data)
        {
            if (data.TryGetValue(EntityId, out var dataEntityId))
            {
                switch (dataEntityId)
                {
                    case EntityId entityId:
                        return entityId;
                    case long id:
                        return new EntityId(id);
                }
            }

            return new Collections.Option<EntityId>();
        }

        public static string ExtractLoggerName(Dictionary<string, object> data)
        {
            if (data.ContainsKey(LoggerName))
            {
                var loggerName = data[LoggerName];
                if (loggerName is string name)
                {
                    return name;
                }
            }

            return UnknownLogger;
        }
    }
}
