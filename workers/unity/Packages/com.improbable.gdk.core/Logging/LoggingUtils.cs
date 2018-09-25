using System.Collections.Generic;
using Improbable.Worker;

#region Diagnostic control

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace Improbable.Gdk.Core
{
    public static class LoggingUtils
    {
        private const string UnknownLogger = "UnknownLogger";
        public const string EntityId = "EntityId";
        public const string LoggerName = "LoggerName";
        public const string WorkerType = "WorkerType";

        public static EntityId? ExtractEntityId(Dictionary<string, object> data)
        {
            if (!data.TryGetValue(EntityId, out var dataEntityId))
            {
                return null;
            }

            switch (dataEntityId)
            {
                case EntityId asEntityId:
                    return asEntityId;
                case long asLong:
                    return new EntityId(asLong);
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
