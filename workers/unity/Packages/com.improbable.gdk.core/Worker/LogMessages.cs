using Improbable.Worker.CInterop;

namespace Improbable.Gdk.Core
{
    public readonly struct LogMessageReceived
    {
        public readonly string Message;
        public readonly LogLevel LogLevel;

        public LogMessageReceived(string message, LogLevel logLevel)
        {
            Message = message;
            LogLevel = logLevel;
        }
    }

    public readonly struct LogMessageToSend
    {
        public readonly string Message;
        public readonly string LoggerName;
        public readonly LogLevel LogLevel;
        public readonly long? EntityId;

        public LogMessageToSend(string message, string loggerName, LogLevel logLevel, long? entityId)
        {
            Message = message;
            LoggerName = loggerName;
            LogLevel = logLevel;
            EntityId = entityId;
        }
    }
}
