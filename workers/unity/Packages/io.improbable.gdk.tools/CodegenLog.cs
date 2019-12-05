using System;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    internal enum CodegenLogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }

    internal struct CodegenLog
    {
        public readonly string Time;
        public readonly CodegenLogLevel Level;
        public readonly string Logger;
        public readonly string Message;
        public readonly string Exception;
        public bool isException => !string.IsNullOrEmpty(Exception);

        private CodegenLog(string time, CodegenLogLevel level, string logger, string message, string exception)
        {
            Time = time;
            Level = level;
            Logger = logger;
            Message = message;
            Exception = exception;
        }

        public LogType GetUnityLogType()
        {
            switch (Level)
            {
                case CodegenLogLevel.Trace:
                case CodegenLogLevel.Debug:
                case CodegenLogLevel.Info:
                    return LogType.Log;
                case CodegenLogLevel.Warn:
                    return LogType.Warning;
                case CodegenLogLevel.Error:
                case CodegenLogLevel.Fatal:
                    return LogType.Error;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Serializable]
        private struct RawCodegenLog
        {
#pragma warning disable 649
            public string time;
            public string level;
            public string logger;
            public string message;
            public string exception;
#pragma warning restore
        }

        public static CodegenLog FromRaw(string rawLog)
        {
            var rawCodegenLog = JsonUtility.FromJson<RawCodegenLog>(rawLog);

            return new CodegenLog
            (
                rawCodegenLog.time,
                (CodegenLogLevel) Enum.Parse(typeof(CodegenLogLevel), rawCodegenLog.level, true),
                rawCodegenLog.logger,
                rawCodegenLog.message,
                rawCodegenLog.exception
            );
        }
    }
}
