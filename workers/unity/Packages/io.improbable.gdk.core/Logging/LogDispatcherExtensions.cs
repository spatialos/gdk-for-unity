using System;
using UnityEngine;

namespace Improbable.Gdk.Core
{
    public static class LogDispatcherExtensions
    {
        public static void Info(this ILogDispatcher logDispatcher, string message)
        {
            logDispatcher.HandleLog(LogType.Log, new LogEvent(message));
        }

        public static void Warn(this ILogDispatcher logDispatcher, string message)
        {
            logDispatcher.HandleLog(LogType.Warning, new LogEvent(message));
        }

        public static void Error(this ILogDispatcher logDispatcher, string message)
        {
            logDispatcher.HandleLog(LogType.Error, new LogEvent(message));
        }

        public static void Error(this ILogDispatcher logDispatcher, string message, Exception exception)
        {
            logDispatcher.HandleLog(LogType.Exception, new LogEvent(message).WithException(exception));
        }
    }
}
