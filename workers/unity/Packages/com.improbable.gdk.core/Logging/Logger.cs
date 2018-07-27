using UnityEngine;

namespace Improbable.Gdk.Core
{
    public class Logger : ILogger
    {
        private readonly ILogDispatcher dispatcher;
        private readonly string loggerName;

        public Logger(ILogDispatcher dispatcher, string loggerName)
        {
            this.dispatcher = dispatcher;
            this.loggerName = loggerName;
        }

        public void Log(LogType type, LogEvent logEvent)
        {
            dispatcher.HandleLog(type, logEvent.WithField("LoggerName", loggerName));
        }
    }
}
