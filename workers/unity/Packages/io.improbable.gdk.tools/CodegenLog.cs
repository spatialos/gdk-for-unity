using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
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
